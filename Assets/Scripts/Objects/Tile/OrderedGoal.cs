using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderedGoal : Goal
{
    public int order;


    public override void InteractionTile(Ball tb)
    {
        if(IsometricManager.Instance.GetManager<GameManager>().checkedFlags != order)
        {
            PoolManager.Instance.Push(tb);
            return;
        }

        base.InteractionTile(tb);
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
