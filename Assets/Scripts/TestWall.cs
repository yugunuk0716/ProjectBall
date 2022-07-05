using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TestBall tb = collision.gameObject.GetComponent<TestBall>();

        if(tb != null)
        {
            Vector2 normal = collision.contacts[0].normal;

            Vector3 incoming = (transform.position - collision.transform.position).normalized;

            Vector3 reflect = Vector3.Reflect(incoming, normal);

            if(Mathf.Abs(reflect.x) > Mathf.Abs(reflect.y))
            {
                if(reflect.x > 0)
                {
                    reflect = Vector2.right;
                }
                else
                {
                    reflect = Vector2.left;
                }
            }
            else
            {
                if (reflect.y > 0)
                {
                    reflect = Vector2.up;
                }
                else
                {
                    reflect = Vector2.down;
                }
            }


            tb.rigid.velocity = reflect * 5;

            print(reflect);
        }
    }
}
