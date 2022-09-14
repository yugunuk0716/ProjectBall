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
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.Shoot = () => Shoot();
    }

    public void Shoot()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        if (!gm.isShooting && GameManager.CanNotInteract || gm.myBallList.Count == 0)
        {
            return;
        }

        anim.SetBool("isClick", true);

        Ball copyBall = gm.myBallList[0]; // 실제 데이터는 얘만 가짐.
        Ball ball = PoolManager.Instance.Pop($"DefaultBall") as Ball;
        ball.transform.position = transform.position - new Vector3(0, 0.25f, 0) + ((Vector3)IsometricManager.GetRealDir(copyBall.shootDir) * 0.3f);
        
        Vector2 shootDir = IsometricManager.GetIsoDir(copyBall.shootDir);
        
        anim.SetFloat("MouseX", shootDir.x);
        anim.SetFloat("MouseY", shootDir.y);

        

        ball.shootDir = copyBall.shootDir;
        ball.SetBall(shootDir, ball.speed);
        ball.SetPos(new Vector2(jumpPad.gridPos.x, jumpPad.gridPos.y));
        ball.SetMove();
        ball.Rollin();

        BallControllUI ballControllUI = gm.ballUIList[0];

        Destroy(ballControllUI.gameObject);
        Destroy(ballControllUI.transform.parent.gameObject);

        gm.maxBallCount--; // 하나 쏘면 이제 하나 줄여줘야 다음 공을 던져용
        gm.ballUIList.Remove(ballControllUI);
        gm.myBallList.Remove(copyBall); // 얘는 데이터만 가지고 있는 더미 볼 리스트니까.
        gm.aliveBallList.Add(ball);

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
