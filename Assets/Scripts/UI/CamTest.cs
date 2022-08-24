using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTest : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;


    private Vector2 vec1;
    private Vector2 vec2;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if(vCam.m_Lens.OrthographicSize < 4f)
            {
                return;
            }

            vCam.m_Lens.OrthographicSize -= 0.05f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (vCam.m_Lens.OrthographicSize >= 8.5f)
            {
                return;
            }

            vCam.m_Lens.OrthographicSize += 0.05f;
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

    }

    public void SetPos()
    {
        Vector3 vec = vec2 - vec1;
        Vector2 pos = transform.position;


        float x = 0f;
        float y = 0f;

        x = Mathf.Clamp(pos.x + vec.normalized.x * 0.1f, -4.6f, 4.6f);
        y = Mathf.Clamp(pos.y + vec.normalized.y * 0.1f, -8.4f, 8.4f);

        transform.position = new Vector2(x, y);

        //if(Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        //{
        //    float x = 0f;

        //    if(vec.x  > 0)
        //    {
        //        x = Mathf.Clamp(transform.position.x + 0.1f, -4.6f, 4.6f);
        //    }
        //    else
        //    {
        //        x = Mathf.Clamp(transform.position.x - 0.1f, -4.6f, 4.6f);
        //    }

        //    transform.position = new Vector2(x, pos.y);
        //}
        //else
        //{
        //    float y = 0f;

        //    if (vec.y > 0)
        //    {
        //        y = Mathf.Clamp(transform.position.y + 0.1f, -8.4f, 8.4f);
        //    }
        //    else
        //    {
        //        y = Mathf.Clamp(transform.position.y - 0.1f, -8.4f, 8.4f);
        //    }
        //    transform.position = new Vector2(pos.x, y);
        //}

    }



}
