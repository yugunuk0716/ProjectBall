using System.Collections;
using UnityEngine;
using DG.Tweening;

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

        if (GameManager.CanNotInteract || gm.myBallList.Count == 0 || gm.myBallList.Count < gm.maxBallCount )
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
        //TargetPointUI parent = ballControllUI.transform.parent.GetComponent<TargetPointUI>();

        //ballControllUI.transform.SetParent(ballControllUI.transform.root); // 캔버스를 부모로 놓고.

        //PoolManager.Instance.Push(parent);
        PoolManager.Instance.Push(ballControllUI);

        //float duration = 0.15f;
        //ballControllUI.transform.DOShakePosition(duration);
        //ballControllUI.directionSetBtn.image.DOFade(0f, duration);
        //ballControllUI.directionSetBtn.transform.DOMove(this.transform.position, duration).OnComplete(() =>
        //{
        //    
        //});
        
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
