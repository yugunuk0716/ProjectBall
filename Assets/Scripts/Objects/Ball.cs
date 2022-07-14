using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PoolableMono
{
    public Rigidbody2D rigid;
    public Animator anim;
    public SpriteRenderer sr;
    public GameObject spriteObject;

    public float tpCool;
    public float tpLastTime;

    public float afCool;
    public float afLastTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        tpCool = 0.5f;
    }


    public void Move(Vector2 dir, float power = 5f)
    {
        anim.SetBool("isMoving", false);
        sr.flipX = dir.x > 0 || dir.y > 0;
        anim.SetFloat("MoveX", Mathf.Abs(dir.x));
       
        anim.SetBool("isMoving", true);
        rigid.velocity = dir * power;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            spriteObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void Reset()
    {
        anim.SetBool("isMoving", false);
        sr.flipX = false;
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);
        rigid.velocity = Vector2.zero;
        spriteObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
