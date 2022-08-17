using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShooterTile : ObjectTile
{
    private Animator anim;
    private JumpPad jumpPad;

    protected override void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        jumpPad = GetComponentInParent<JumpPad>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && false == EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.mousePosition.y < Screen.height / 3)
            {
                Debug.Log("하단 1/3을 클릭해야 공이 나가요!");
                return;
            }
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

        ball.transform.position = transform.position - new Vector3(0, .25f, 0);
        Vector2 shootDir = IsometricManager.GetIsoDir(copyBall.shootDir);
        anim.SetFloat("MouseX", shootDir.x);
        anim.SetFloat("MouseY", shootDir.y);

        ball.SetBall(shootDir, 0.5f);
        ball.SetPos(new Vector2(jumpPad.gridPos.x, jumpPad.gridPos.y));
        ball.SetMove();

        gm.myBallList.RemoveAt(0);
        gm.maxBallCount--; // 하나 쏘면 이제 하나 줄여줘야 다음 공을 던져용
        GameObject ballControllUI = gm.ballUIList[0];
        gm.ballUIList.Remove(ballControllUI);
        Destroy(ballControllUI);
    }

    

    public void SetEnd()
    {
        anim.SetBool("isClick", false);
    }

    public override void Reset()
    {

    }

    public override void InteractionTile(Ball tb)
    {

    }
}
