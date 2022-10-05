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
        BallDestroyParticle bdp = GameObjectPoolManager.Instance.GetGameObject("Effects/BallDestroyParticle", 
            GameObjectPoolManager.Instance.transform).GetComponent<BallDestroyParticle>();

        if (bdp != null)
        {
            bdp.transform.position = tb.transform.position;
            bdp.PlayParticle();
        }
        GameObjectPoolManager.Instance.UnusedGameObject(tb.gameObject);

        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Thorn");
        anim.SetTrigger("TriggerBall");
        
    }

    public override IEnumerator Transition()
    {
        yield return null;
    }

}
