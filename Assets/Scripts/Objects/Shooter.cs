using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public const int maxAmmoCount = 10;
    public int curAmmoCount = 0;

    private void Start()
    {
        //Shoot();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    

    public void Shoot()
    {
        Debug.Log($"{this.name} Shooted");

        if (maxAmmoCount <= curAmmoCount)
            return;

        Ball ball = PoolManager.Instance.Pop("Ball") as Ball;
        ball.transform.position = this.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치 받고
        Vector2 plusPos = (mousePos - (Vector2)ball.transform.position); // 혹시 슈터 위치 바뀔 수 있으니 위치 빼주기

        // 더 의도와 가까운 방향 남기기.
        if (Mathf.Abs(plusPos.x) > Mathf.Abs(plusPos.y)) plusPos.y = 0;
        else plusPos.x = 0;

        ball.Move(plusPos.normalized, 5f);

        curAmmoCount++;
    }
}
