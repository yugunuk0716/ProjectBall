using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShooterTile : MonoBehaviour
{
    public int maxAmmoCount = 10;
    public int curAmmoCount = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && false == EventSystem.current.IsPointerOverGameObject())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (maxAmmoCount <= curAmmoCount)
            return;

        Ball ball = PoolManager.Instance.Pop("Ball") as Ball;
        ball.transform.position = this.transform.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치 받고
        Vector2 shootDir = SetVectorScaleOne((mousePos - (Vector2)ball.transform.position).normalized);

        ball.Move(shootDir, 5f);

        curAmmoCount++;
    }

    Vector2 SetVectorScaleOne(Vector2 vec)// 벡터의 특정 방향의 크기를 1 혹은 0으로 만들기
    {
        bool isAxisXPositive = vec.x > 0;
        bool isAxisYPositive = vec.y > 0;

        if (isAxisXPositive && isAxisYPositive) vec = new Vector2(0.5f, 0.25f);
        else if (!isAxisXPositive && !isAxisYPositive) vec = new Vector2(-0.5f, -0.25f);
        else if (!isAxisXPositive && isAxisYPositive) vec = new Vector2(-0.9f, 0.45f);
        else vec = new Vector2(0.9f, -0.45f);

        return vec;
    }
}
