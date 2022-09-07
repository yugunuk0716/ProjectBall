using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestryParticle : PoolableMono
{
    public ParticleSystem ps;



    public void PlayParticle()
    {
        ps.Play();

    }

   


    public override void Reset()
    {

       
    }
}
