using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(tb.gameObject);
        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Thorn");
    }

    public override IEnumerator Transition()
    {
        yield return null;
    }
}
