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

    public float curActiveTime = 0f;
    public float maxActiveTime = 15f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        tpCool = 0.5f;
    }

    private void Update()
    {
        curActiveTime += Time.deltaTime;
        if (curActiveTime >= maxActiveTime) gameObject.SetActive(false);

        Vector3 myPos = transform.position;
        myPos.z = transform.position.y * -0.1f + 1.7f;
        transform.position = myPos;
    }

    public void Move(Vector2 dir, float power = 5f)
    {
        sr.flipX = dir.x > 0 || dir.y > 0;
        rigid.velocity = dir * power;
    }

    public override void Reset()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        curActiveTime = 0f; // 충돌 안하면 MaxActiveTime뒤 사라지게
    }
}
