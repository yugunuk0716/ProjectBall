using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAirFlow : MonoBehaviour
{
    public float flowAmount;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TestBall tb = collision.gameObject.GetComponent<TestBall>();

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
