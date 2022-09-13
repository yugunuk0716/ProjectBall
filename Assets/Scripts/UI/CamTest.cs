using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CamTest : MonoBehaviour
{
    #region Consts
    private const float CAMERA_MAX_SIZE = 8.5f;
    private const float CAMERA_MIN_SIZE = 4f;
    private const float HORIZONTAL_MAX = 4.6f;
    private const float HORIZONTAL_MIN = -4.6f;
    private const float VERTICAL_MAX = 8f;
    private const float VERTICAL_MIN = -8f;
    private const float MOVE_DELAY = 0.15f;
    #endregion




    public CinemachineVirtualCamera vCam;


    private Vector2 vec1;
    private Vector2 vec2;


    private Vector2 prevT1Pos;
    private Vector2 prevT2Pos;
    readonly float camMoveCool = 0.15f;
    float lastCamMoveTime = 0f;

    bool isTouch = false;
    Tween t;

    private void Start()
    {
        vCam.m_Lens.OrthographicSize = CAMERA_MAX_SIZE;
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            #region PC Test
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A))
            {
                if (vCam.m_Lens.OrthographicSize < CAMERA_MIN_SIZE)
                {
                    return;
                }

                vCam.m_Lens.OrthographicSize -= 0.025f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (vCam.m_Lens.OrthographicSize >= CAMERA_MAX_SIZE)
                {
                    return;
                }

                vCam.m_Lens.OrthographicSize += 0.025f;
            }

            if (Input.GetMouseButtonDown(0))
            {

                vec1 = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                vec2 = Input.mousePosition;
                SetPos();
            }
#endif
            #endregion


            if (lastCamMoveTime + camMoveCool < Time.time)
            {
                if (Input.touchCount == 2)
                {
                    CameraZoom();
                }

                else if (Input.touchCount == 1)
                {
                    CameraMove();
                }
            }

        }

    }

    public void SetPos()
    {

        if (lastCamMoveTime + camMoveCool > Time.time)
        {
            return;
        }

        lastCamMoveTime = Time.time;
        Vector3 vec = vec1 - vec2;
        Vector2 pos = transform.position;


        float x = 0f;
        float y = 0f;

#if UNITY_EDITOR
        x = Mathf.Clamp(pos.x + vec.normalized.x, HORIZONTAL_MIN, HORIZONTAL_MAX);
        y = Mathf.Clamp(pos.y + vec.normalized.y, VERTICAL_MIN, VERTICAL_MAX);
#else
        x = Mathf.Clamp(pos.x + vec.normalized.x * 2f,  HORIZONTAL_MIN, HORIZONTAL_MAX);
        y = Mathf.Clamp(pos.y + vec.normalized.y * 2f, VERTICAL_MIN, VERTICAL_MAX);
#endif

        transform.DOMove(new Vector2(x, y), MOVE_DELAY);
     

    }

    public void CameraMove()
    {

        if(Input.touches.Length <= 0 || lastCamMoveTime + camMoveCool > Time.time)
        {
            return;
        }

        Touch t = Input.GetTouch(0);


        for(int i = 0; i < Input.touchCount; i++)
        {
            t = Input.touches[i];

            switch (t.phase)
            {
                case TouchPhase.Began:
                    vec1 = t.position;
                    break;
                case TouchPhase.Ended:
                    vec2 = t.position;
                    SetPos();
                    break;
                case TouchPhase.Canceled:
                    return;
            }
        }
    }


    public void CameraZoom()
    {
        if (Input.touches.Length == 2 && lastCamMoveTime + camMoveCool < Time.time)
        {

            if(t != null)
            {
                t.Kill();
            }

            lastCamMoveTime = Time.time;

            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            switch (t1.phase)
            {
            
                case TouchPhase.Ended:
                    isTouch = false;
                    break;
            }


            Vector2 touchZeroPrevPos = t1.position - t1.deltaPosition;
            Vector2 touchOnePrevPos = t2.position - t2.deltaPosition;

            if (!isTouch)
            {
                isTouch = true;
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector2((touchZeroPrevPos.x + touchOnePrevPos.x) / 2, (touchZeroPrevPos.y + touchOnePrevPos.y) / 2));
                transform.DOMove(worldPos, 0.2f);
                Debug.LogError(worldPos);

            }
            
            


            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (t1.position - t2.position).magnitude;

            t = DOTween.To(() => vCam.m_Lens.OrthographicSize, x => vCam.m_Lens.OrthographicSize = x, Mathf.Clamp(vCam.m_Lens.OrthographicSize + (prevTouchDeltaMag - touchDeltaMag) * 0.02f, CAMERA_MIN_SIZE, CAMERA_MAX_SIZE), 0.1f);
          

            if (prevT2Pos == Vector2.zero && prevT1Pos == Vector2.zero)
            {
                prevT1Pos = t1.position;
                prevT2Pos = t2.position;

            }
        }

    }


}
