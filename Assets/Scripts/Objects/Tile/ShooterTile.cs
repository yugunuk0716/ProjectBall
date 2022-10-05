using System.Collections;
using UnityEngine;

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
        gm.Shoot = () =>
        {
            if (gm.canInteract) Shoot();
        };
    }

    public void Shoot()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        Vibration.Vibrate(5);

        if (gm.ballUIList.Count <= 0)
        {
            return;
        }

        anim.SetBool("isClick", true);
        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Cannon");
        Ball ball = gm.ballUIList[0].ball; // 실제 데이터는 얘만 가짐.
        ball.Spawned();

        ball.transform.position = transform.position - new Vector3(0, 0.25f, 0) + ((Vector3)IsometricManager.GetRealDir(ball.shootDir) * 0.3f);
        
        Vector2 shootDir = IsometricManager.GetIsoDir(ball.shootDir);
        
        anim.SetFloat("MouseX", shootDir.x);
        anim.SetFloat("MouseY", shootDir.y);

        ball.SetBall(shootDir, ball.speed);
        ball.SetPos(new Vector2(jumpPad.gridPos.x, jumpPad.gridPos.y));
        ball.SetMove();
        ball.Rollin();

        BallControllUI ballControllUI = gm.ballUIList[0];

        gm.ballUIList.Remove(ballControllUI);
        ballControllUI.SetDisable();
    }


    public void SetEnd()
    {
        anim.SetBool("isClick", false);
    }

    public override void InteractionTile(Ball tb)
    {

    }

    public override IEnumerator Transition()
    {
        yield return null;
    }
}
