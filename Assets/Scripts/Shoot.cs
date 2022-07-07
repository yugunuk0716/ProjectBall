using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Shooter shooter;
    public Shooter curShooter;

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
        Shooter _shooter = Instantiate(shooter);

      

        _shooter.transform.position = this.transform.position;
        curShooter = _shooter;
    }
    
}