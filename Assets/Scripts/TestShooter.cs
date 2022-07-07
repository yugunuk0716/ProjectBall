using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    public Vector2 shootDir;

    public Ball ball;

    public void Shoot()
    {
        Ball ball2 = Instantiate(ball);
        ball2.transform.position = this.transform.position;
        Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if (vec.x > 0)
            {
                shootDir = Vector2.right;
            }
            else
            {
                shootDir = Vector2.left;
            }
        }
        else
        {
            if (vec.y > 0)
            {
                shootDir = Vector2.up;
            }
            else
            {
                shootDir = Vector2.down;
            }
        }
        ball2.Move(shootDir, 5f);

        Destroy(this.gameObject);
    }
}
