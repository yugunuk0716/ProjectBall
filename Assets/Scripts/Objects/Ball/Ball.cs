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

    public TileType collisionTileType;
    public TileDirection shootDir;

    private Vector3 baseVec;
    public Vector2 direction;
    public Vector2 myPos;
    public float speed = 0.25f;

    public Color currentColor = Color.white;

    private ParticleSystem interactParticle;

    private GameManager gm = null;
    private StageManager sm = null;

    private void Start()
    {
        interactParticle = GetComponentInChildren<ParticleSystem>();
        tpCool = 0.1f;

        gm = IsometricManager.Instance.GetManager<GameManager>();
        sm = IsometricManager.Instance.GetManager<StageManager>();
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
        if (gm == null)
        {
            gm = IsometricManager.Instance.GetManager<GameManager>();
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        myPos += direction;

        if(gm.tileDict.ContainsKey(myPos))
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
            SetDisable();
        }
    }

    public void Rollin()
    {
        StartCoroutine(SetBaseVector());
    }

    public void Despawned()
    {
        if (gm == null)
        {
            gm = IsometricManager.Instance.GetManager<GameManager>();
            sm = IsometricManager.Instance.GetManager<StageManager>();
        }

        this.DOKill();
        speed = 0.4f;

        if(!sm.isMapLoading)
        {
            StopCoroutine(SetBaseVector());
        }
    }

    public void Spawned()
    {
        gameObject.SetActive(false);
        slowAnim.gameObject.SetActive(false);
        speed = 0.4f;
    }

    public void SetDisable()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
    }
}
