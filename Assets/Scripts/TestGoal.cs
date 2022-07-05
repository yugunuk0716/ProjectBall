using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGoal : MonoBehaviour
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        TestBall tb = collision.gameObject.GetComponent<TestBall>();

        if (tb != null)
        {
            tb.rigid.velocity = Vector3.zero;
            tb.transform.position = transform.position;
        }
    }


}
