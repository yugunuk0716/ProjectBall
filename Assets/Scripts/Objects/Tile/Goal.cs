using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : ObjectTile
{
    public SpriteRenderer sr;
    public Animator animator;

    public bool isChecked;

    public void ResetFlag(bool isClear)
    {
        isChecked = isClear;
        animator.SetBool("isClear", isClear);
    }

    public override void InteractionTile(Ball tb)
    {
        BallDestroyParticle bdp = GameObjectPoolManager.Instance.GetGameObject("Effects/BallDestroyParticle", GameObjectPoolManager.Instance.transform).GetComponent<BallDestroyParticle>();

        if (bdp != null)
        {
            bdp.transform.position = tb.transform.position;
            bdp.PlayParticle();
        }

        if (!isChecked)
        {
            SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
            sm.Play("Flag");
            ResetFlag(true);
        }

        GameObjectPoolManager.Instance.UnusedGameObject(tb.gameObject);

        StageManager stageManager = IsometricManager.Instance.GetManager<StageManager>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        if (!stageManager.isMapLoading)
        {
            ++gm.curDestroyedBallsCount;
            gm.CheckClear();
        }
    }
        

    public new void Spawned()
    {
        ResetFlag(false);
        StopCoroutine("Transition");
    }

    public override IEnumerator Transition()
    {
        yield return null;
    }
}
