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

public class Ball : MonoBehaviour, IPoolableComponent
{
    public Animator anim;
    public SpriteRenderer sr;
    public Sprite uiSprite; // UI sprite

    public Animator slowAnim;
    public GameObject spriteObject;

    public float tpCool;
    public float tpLastTime;

    public float afCool;
    public float afLastTime;

    public float curActiveTime = 0f;
    public float maxActiveTime = 15f;

    public TileType collisionTileType;
    public TileDirection shootDir;

    private Vector3 baseVec;
    public Vector2 direction;
    public Vector2 myPos;
    public float speed = 0.25f;

    public Color currentColor = Color.white;


    private ParticleSystem interactParticle;

    private void Awake()
    {
        interactParticle = GetComponentInChildren<ParticleSystem>();
        tpCool = 0.1f;
    }

    IEnumerator SetBaseVector()
    {
        while(true)
        {
            switch(shootDir)
            {
                case TileDirection.LEFTUP:
                case TileDirection.RIGHTUP:
                    baseVec = Vector3.forward;
                    break;

                case TileDirection.LEFTDOWN:
                case TileDirection.RIGHTDOWN:
                    baseVec = -Vector3.forward;

                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        curActiveTime += Time.deltaTime;
        if (curActiveTime >= maxActiveTime) gameObject.SetActive(false);

        Vector3 myPos = transform.position;
        myPos.z = transform.position.y * -0.1f + 1.7f;
        transform.position = myPos;


        sr.transform.Rotate(baseVec * 500 * Time.deltaTime); 
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

    public void SetMove()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        myPos += direction;
        if (gm.tileDict.ContainsKey(myPos))
        {
            ObjectTile tile = gm.tileDict[myPos];

            transform.DOMove((Vector3)tile.worldPos, speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                tile.InteractionTile(this);
                if (!tile.myType.Equals(TileType.None))
                {
                    interactParticle.Play();
                }
                if (tile.myType.Equals(TileType.Slow))
                {
                    slowAnim.gameObject.SetActive(true);
                    slowAnim.Play("SlowEffect");
                }

            });
        }
        else
        {
            BallDestroyParticle bdp = GameObjectPoolManager.Instance.GetGameObject("Effects/BallDestroyParticle", GameObjectPoolManager.Instance.transform).GetComponent<BallDestroyParticle>();

            if (bdp != null)
            {
                bdp.transform.position = this.transform.position;
                bdp.PlayParticle();
            }

            GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
        }
    }

    public void Rollin()
    {
        StartCoroutine(SetBaseVector());
    }

   public void ColorChange(Color newColor)
    {
        currentColor = newColor;
        sr.color = currentColor;
    }

    public void Despawned()
    {
        this.DOKill();
        speed = 0.4f;
        curActiveTime = 0;

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.curDestroyedBallsCount++;

        if (gm.maxBallCount == gm.curDestroyedBallsCount)
        {
            gm.CheckFail();
        }
        StopCoroutine(SetBaseVector());
    }

    public void Spawned()
    {
        gameObject.SetActive(false);
        slowAnim.gameObject.SetActive(false);
        speed = 0.4f;
        ColorChange(Color.white);
    }

    public void SetDisable()
    {
        this.DOKill();
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
    }
}
