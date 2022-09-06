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
        PoolManager.Instance.Push(tb);
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
