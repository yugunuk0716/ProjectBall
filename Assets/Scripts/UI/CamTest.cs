using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTest : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;


    private Vector2 vec1;
    private Vector2 vec2;


    private Vector2 prevT1Pos;
    private Vector2 prevT2Pos;

    private void Update()
    {
        #region PC Test
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
        {
            if(vCam.m_Lens.OrthographicSize < 4f)
            {
                return;
            }

            vCam.m_Lens.OrthographicSize -= 0.025f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (vCam.m_Lens.OrthographicSize >= 8.5f)
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


        CameraZoom();
              
        
        CameraMove();
        
    }

    public void SetPos()
    {
        Vector3 vec = vec2 - vec1;
        Vector2 pos = transform.position;


        float x = 0f;
        float y = 0f;

        x = Mathf.Clamp(pos.x + vec.normalized.x * 0.01f, -4.6f, 4.6f);
        y = Mathf.Clamp(pos.y + vec.normalized.y * 0.01f, -8.4f, 8.4f);

        print($"{x}, {y}");

        transform.position = new Vector2(x, y);
     

    }

    public void CameraMove()
    {

        if(Input.touches.Length <= 0)
        {
            return;
        }

        Touch t = Input.GetTouch(0);

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


    public void CameraZoom()
    {
        if (Input.touches.Length == 2)
        {

            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = t1.position - t1.deltaPosition;
            Vector2 touchOnePrevPos = t2.position - t2.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (t1.position - t2.position).magnitude;

            vCam.m_Lens.OrthographicSize = Mathf.Clamp((prevTouchDeltaMag - touchDeltaMag) * 0.1f, 4f, 8.5f);

            if (prevT2Pos == Vector2.zero && prevT1Pos == Vector2.zero)
            {
                prevT1Pos = t1.position;
                prevT2Pos = t2.position;

            }
        }

    }


}
