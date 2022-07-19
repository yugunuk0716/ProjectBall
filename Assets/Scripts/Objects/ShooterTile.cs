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
        ball.transform.position = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���콺 ��ġ �ް�
        Vector2 plusPos = (mousePos - (Vector2)ball.transform.position); // Ȥ�� ���� ��ġ �ٲ� �� ������ ��ġ ���ֱ�

        // �� �ǵ��� ����� ���� �����.
        if (Mathf.Abs(plusPos.x) > Mathf.Abs(plusPos.y)) plusPos.y = 0;
        else plusPos.x = 0;

        ball.Move(plusPos.normalized, 5f);

        curAmmoCount++;


        //Vector2(-0.9f, 0.45f)
        //Vector2(0.9f, -0.45f)
        //Vector2(-0.5f, -0.25f)
        //Vector2(0.5f, 0.25f)
    }
}
