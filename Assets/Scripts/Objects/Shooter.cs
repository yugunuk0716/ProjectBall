using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Vector2 shootDir;

    public Ball ball;

    public void Shoot()
    {
        Ball ball2 = Instantiate(ball, transform.position, Quaternion.identity); // �� �����
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���콺 ��ġ �ް�
        Vector2 plusPos = (mousePos - (Vector2)ball2.transform.position); // Ȥ�� ���� ��ġ �ٲ� �� ������ ��ġ ���ֱ�

        // �� �ǵ��� ����� ���� �����.
        if (Mathf.Abs(plusPos.x) > Mathf.Abs(plusPos.y)) plusPos.y = 0;
        else plusPos.x = 0;

        shootDir = plusPos.normalized; // ����ȭ�ؼ� shootDir�� �Ҵ�
        ball2.Move(shootDir, 5f); 
        Destroy(this.gameObject);
    }
}
