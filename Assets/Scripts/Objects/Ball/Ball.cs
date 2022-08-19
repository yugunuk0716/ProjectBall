using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ECollisionTile
{
    NoCollision,
    Teleporter,
    DirChanger,
    ReflectWall,
    BallDestroyer,
    Goal,
    AirFlow,
}

public enum BallState
{
    None,
    Destroy,
    Ignore,
}

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

    public TileType collisionTileType;
    public BallState ballState;
    public TileDirection shootDir;



    public Vector2 direction;
    public Vector2 myPos;
    public float speed = 2f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        tpCool = 0.1f;
    }

    private void OnEnable()
    {
        curActiveTime = 0;
    }

    private void Update()
    {
        curActiveTime += Time.deltaTime;
        if (curActiveTime >= maxActiveTime) gameObject.SetActive(false);

        Vector3 myPos = transform.position;
        myPos.z = transform.position.y * -0.1f + 1.7f;
        transform.position = myPos;
    }

    public void SetBall(Vector2 dir, float speed)
    {
        direction = dir;
        this.speed = speed;
    }

    public void SetPos(Vector2 pos)
    {
        myPos = pos;
    }

    public void Move(Vector2 dir, float power = 5f)
    {
        sr.flipX = dir.x > 0 || dir.y > 0;
        rigid.velocity = dir * power;
    }

    public void SetMove()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        myPos += direction;
        if (IsometricManager.Instance.GetManager<GameManager>().tileDict.ContainsKey(myPos))
        {
            ObjectTile tile = IsometricManager.Instance.GetManager<GameManager>().tileDict[myPos];
            transform.DOMove((Vector3)tile.worldPos, speed).SetEase(Ease.Linear).OnComplete(() => tile.InteractionTile(this));
        }
        else
        {

            PoolManager.Instance.Push(this);
        }
    }

    public override void Reset()
    {
        gameObject.SetActive(false);
        print(gameObject.activeSelf);
    }

    public void RemoveSpecialEffect()
    {
        this.ballState = BallState.None;
        OnRemoveSE();
    }

    public virtual void OnRemoveSE()  // RemoveSpecialEffect callback
    {
        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }    

   
}
