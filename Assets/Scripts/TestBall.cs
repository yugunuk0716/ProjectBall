using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{
    public Rigidbody2D rigid;

    public float tpCool;
    public float tpLastTime;

    public float afCool;
    public float afLastTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        tpCool = 0.5f;
    }


    public void Move(Vector2 dir, float power)
    {
        rigid.velocity = dir * power;
    }
}
