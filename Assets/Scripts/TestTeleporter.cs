using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeleporter : MonoBehaviour
{

    public TestTeleporter pairTeleporter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            if(tb.tpCool + tb.tpLastTime < Time.time)
            {
              
                tb.tpLastTime = Time.time;
                tb.transform.position = pairTeleporter.transform.position;
            }
        }
    }
}
