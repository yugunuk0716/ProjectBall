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
        base.Awake();
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
            if (Input.mousePosition.y > Screen.height / 3)
            {
                return;
            }
            Shoot();
        }
    }

    public void Shoot()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        if (gm.myBallList.Count < gm.maxBallCount || 0 >= gm.myBallList.Count)
        {
            return;
        }


        

        if (false == gm.isPlayStarted)
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

        ball.transform.position = transform.position - new Vector3(0, 0.25f, 0);
        
        Vector2 shootDir = IsometricManager.GetIsoDir(copyBall.shootDir);
        
        anim.SetFloat("MouseX", shootDir.x);
        anim.SetFloat("MouseY", shootDir.y);

        ball.shootDir = copyBall.shootDir;
        ball.SetBall(shootDir, ball.speed);
        ball.SetPos(new Vector2(jumpPad.gridPos.x, jumpPad.gridPos.y));
        ball.SetMove();
        ball.Rollin();
        gm.maxBallCount--; // 하나 쏘면 이제 하나 줄여줘야 다음 공을 던져용
        GameObject ballControllUI = gm.ballUIList[0];
        gm.ballUIList.Remove(ballControllUI);
        Destroy(ballControllUI);

        gm.myBallList.Remove(copyBall); // 얘는 데이터만 가지고 있는 더미 볼 리스트니까.
        gm.aliveBallList.Add(ball);
        Debug.Log("공 추가");
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

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
