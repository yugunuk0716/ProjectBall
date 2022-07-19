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
        //if (maxAmmoCount <= curAmmoCount)
        //    return;

        if (anim.GetBool("isClick"))
        {
            anim.SetBool("isClick", true);
        }

        anim.SetBool("isClick", true);

        Ball ball = PoolManager.Instance.Pop("Ball") as Ball;
        ball.transform.position = transform.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치 받고
        Vector2 shootDir = GetIsoDir((mousePos - (Vector2)ball.transform.position).normalized);

        anim.SetFloat("MouseX", shootDir.x);
        anim.SetFloat("MouseY", shootDir.y);

        ball.Move(shootDir, 5f);

        curAmmoCount++;
        
    }

    Vector2 GetIsoDir(Vector2 vec) // 등각투형에 걸맞는 벡터로..
    {
        bool isAxisXPositive = vec.x > 0;
        bool isAxisYPositive = vec.y > 0;

        if (isAxisXPositive && isAxisYPositive) vec = new Vector2(0.5f, 0.25f);
        else if (!isAxisXPositive && !isAxisYPositive) vec = new Vector2(-0.5f, -0.25f);
        else if (!isAxisXPositive && isAxisYPositive) vec = new Vector2(-0.9f, 0.45f);
        else vec = new Vector2(0.9f, -0.45f);

        
        
        return vec.normalized;
    }

    public void SetEnd()
    {
        anim.SetBool("isClick", false);
    }

}
