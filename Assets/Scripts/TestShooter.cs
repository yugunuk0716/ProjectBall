using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    public Vector2 shootDir;

    public TestBall ball;

    private void Update()
    {
       
    }


    public void Shoot()
    {
        TestBall ball2 = Instantiate(ball);
        ball2.transform.position = this.transform.position;

        ball2.Move(shootDir, 5f);

        Destroy(this.gameObject);
    }
}
