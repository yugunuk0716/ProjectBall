using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : ObjectTile
{
    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void InteractionTile(Ball tb)
    {
        print("네");
        BallDestryParticle bdp = PoolManager.Instance.Pop("BallDestroyParticle") as BallDestryParticle;

        if (bdp != null)
        {
            bdp.transform.position = tb.transform.position;
            bdp.PlayParticle();
        }
        tb.gameObject.SetActive(false);

        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Thorn");
        anim.SetTrigger("TriggerBall");
        
    }

    public override void Reset()
    {
        
    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }

}
