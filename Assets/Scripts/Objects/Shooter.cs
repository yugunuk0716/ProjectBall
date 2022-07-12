using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Vector2 shootDir;

    public Ball ball;

    public void Shoot()
    {
        Ball ball2 = Instantiate(ball, transform.position, Quaternion.identity); // 공 만들고
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치 받고
        Vector2 plusPos = (mousePos - (Vector2)ball2.transform.position); // 혹시 슈터 위치 바뀔 수 있으니 위치 빼주기

        // 더 의도와 가까운 방향 남기기.
        if (Mathf.Abs(plusPos.x) > Mathf.Abs(plusPos.y)) plusPos.y = 0;
        else plusPos.x = 0;

        shootDir = plusPos.normalized; // 정규화해서 shootDir에 할당
        ball2.Move(shootDir, 5f); 
        Destroy(this.gameObject);
    }
}
