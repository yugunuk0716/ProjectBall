using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShooterTile : MonoBehaviour
{
    public int maxAmmoCount = 10;
    public int curAmmoCount = 0;

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
        if (maxAmmoCount <= curAmmoCount) return;

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        if (gm.myBallList.Count < gm.maxBallCount) return;

        gm.maxBallCount--; // �ϳ� ��� ���� �ϳ� �ٿ���� ���� ���� ������
        GameObject ballControllUI = gm.ballUIList[0];
        gm.ballUIList.Remove(ballControllUI);
        Destroy(ballControllUI);

        if (anim.GetBool("isClick"))
        {
            anim.SetBool("isClick", true);
        }

        anim.SetBool("isClick", true);

        try
        {
            Ball copyBall = IsometricManager.Instance.GetManager<GameManager>().myBallList[0]; // ���� �����ʹ� �길 ����.
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
            Debug.Log("copy" + copyBall.shootDir);
            anim.SetFloat("MouseX", shootDir.x);
            anim.SetFloat("MouseY", shootDir.y);

            ball.Move(shootDir, 3.5f);

            curAmmoCount++;

            gm.myBallList.RemoveAt(0);
        }
        catch
        {
            Debug.Log("�׽�Ʈ������ �ε��� ������ �����մϴ�.");
        }
    }

    Vector2 GetIsoDir(TileDirection dir) // ������� �ɸ´� ���ͷ�..
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

}
