using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            tb.rigid.velocity = Vector3.zero;
            //tb.transform.position = transform.position;
            Destroy(tb.gameObject);
            sr.color = Color.green;
        }
    }


}
