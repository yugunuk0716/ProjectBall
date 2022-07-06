using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAirFlow : MonoBehaviour
{
    public float flowAmount;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            if (tb.afCool + tb.afLastTime < Time.time)
            {

                tb.afLastTime = Time.time;
                tb.rigid.velocity *= flowAmount;
            }
        }
    }
}
