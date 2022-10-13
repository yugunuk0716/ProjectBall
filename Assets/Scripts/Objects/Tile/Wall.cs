using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(tb.gameObject);
        StageManager stageManager = IsometricManager.Instance.GetManager<StageManager>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        if (!stageManager.isMapLoading)
        {
            gm.curDestroyedBallsCount++;
            gm.CheckFail();
        }

        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Thorn");
    }

    public override IEnumerator Transition()
    {
        yield return null;
    }
}
