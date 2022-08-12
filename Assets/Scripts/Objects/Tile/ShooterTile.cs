using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShooterTile : ObjectTile
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && false == EventSystem.current.IsPointerOverGameObject())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        if (gm.myBallList.Count < gm.maxBallCount || 0 >= gm.myBallList.Count) return;

        if(false == gm.isPlayStarted)
        {
            gm.ballUIList.ForEach((x) => x.GetComponent<Button>().interactable = false);
        }

        if (anim.GetBool("isClick"))
        {
            anim.SetBool("isClick", true);
        }

        anim.SetBool("isClick", true);

        Ball copyBall = IsometricManager.Instance.GetManager<GameManager>().myBallList[0]; // 실제 데이터는 얘만 가짐.
        Ball ball = null;
        if (copyBall.ballState != BallState.None)
        {
            ball = PoolManager.Instance.Pop($"{copyBall.ballState}{copyBall.collisionTileType}") as Ball;
        }
        else
        {
            ball = PoolManager.Instance.Pop($"DefaultBall") as Ball;
        }

        ball.transform.position = transform.position;
        Vector2 shootDir = GetIsoDir(copyBall.shootDir);
        anim.SetFloat("MouseX", shootDir.x);
        anim.SetFloat("MouseY", shootDir.y);

        //ball.SetBall(shootDir, 0.1f, myKeyPos);
        //ball.SetMove();

        gm.myBallList.RemoveAt(0);
        gm.maxBallCount--; // 하나 쏘면 이제 하나 줄여줘야 다음 공을 던져용
        GameObject ballControllUI = gm.ballUIList[0];
        gm.ballUIList.Remove(ballControllUI);
        Destroy(ballControllUI);
    }

    Vector2 GetIsoDir(TileDirection dir) // 등각투형에 걸맞는 벡터로..
    {
        Vector2 vec = Vector2.zero;
        switch(dir)
        {
            case TileDirection.RIGHTUP:
                vec = new Vector2(0.5f, 0.25f);
                break;
            case TileDirection.LEFTDOWN:
                vec = new Vector2(-0.5f, -0.25f);
                break;
            case TileDirection.LEFTUP:
                vec = new Vector2(-0.9f, 0.45f);
                break;
            case TileDirection.RIGHTDOWN:
                vec = new Vector2(0.9f, -0.45f);
                break;
        }

        return vec.normalized;
    }

    public void SetEnd()
    {
        anim.SetBool("isClick", false);
    }

    public override void OnTriggerBall(Ball tb)
    {

    }

    public override void Reset()

    {
    }
}
