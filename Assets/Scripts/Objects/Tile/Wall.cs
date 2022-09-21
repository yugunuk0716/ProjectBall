using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        PoolManager.Instance.Push(tb);
        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Thorn");
    }

    public override void Reset()
    {
        StopCoroutine("Transition");
    }

    public override IEnumerator Transition()
    {
        yield return null;
    }
}
