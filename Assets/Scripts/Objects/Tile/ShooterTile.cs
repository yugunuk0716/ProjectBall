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
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        gm.Shoot = () =>
        {
            if (GameManager.canInteract && !sm.isMapLoading)
            {
                gm.isShooting = true;
                Shoot();
            }
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
        Ball ball = gm.ballUIList[0].ball; 
        ball.Spawned();

        ball.transform.position = transform.position - new Vector3(0, 0.25f, 0) + ((Vector3)IsometricManager.GetRealDir(ball.shootDir) * 0.3f);
        
        Vector2 shootDir = GetIsoDir(ball.shootDir);
        
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

    public Vector2 GetIsoDir(TileDirection dir)
    {
        Vector2 vec = Vector2.zero;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                vec = Vector2.right;
                break;
            case TileDirection.LEFTDOWN:
                vec = Vector2.left;
                break;
            case TileDirection.LEFTUP:
                vec = Vector2.up;
                break;
            case TileDirection.RIGHTDOWN:
                vec = Vector2.down;
                break;
        }
        return vec;
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
