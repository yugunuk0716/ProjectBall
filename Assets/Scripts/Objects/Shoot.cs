using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Shooter shooter;
    public Shooter curShooter;

    public const int ammoCount = 10;
    public int curAmmoCount = 0;

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
        if (ammoCount <= curAmmoCount)
            return;
        Shooter _shooter = Instantiate(shooter);

        curAmmoCount++;

        _shooter.transform.position = this.transform.position;
        curShooter = _shooter;
    }
    
}
