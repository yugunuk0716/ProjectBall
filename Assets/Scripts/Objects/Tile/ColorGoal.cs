using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGoal : Goal
{

    [SerializeField] Color successColor;

    public override void InteractionTile(Ball tb)
    {
        if (!successColor.Equals(tb.currentColor))
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
