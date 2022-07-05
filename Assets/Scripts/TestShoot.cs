using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    public TestShooter shooter;
    public TestShooter curShooter;

    private void Start()
    {
        SpawnShooter();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(curShooter != null)
            {
                curShooter.Shoot();
            }
            SpawnShooter();
        }
    }
    

    public void SpawnShooter()
    {
        TestShooter _shooter = Instantiate(shooter);
        int idx = Random.Range(0, 4);
        switch (idx)
        {
            case 0:
                _shooter.shootDir = Vector2.up;
                break;
            case 1:
                _shooter.shootDir = Vector2.down;
                break;
            case 2:
                _shooter.shootDir = Vector2.right;
                break;
            case 3:
                _shooter.shootDir = Vector2.left;
                break;
        }
        _shooter.transform.position = this.transform.position;
        curShooter = _shooter;
    }
    
}
