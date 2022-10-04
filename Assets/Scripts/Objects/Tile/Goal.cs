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
        BallDestryParticle bdp = PoolManager.Instance.Pop("BallDestroyParticle") as BallDestryParticle;

        if (bdp != null)
        {
            bdp.transform.position = tb.transform.position;
            bdp.PlayParticle();
        }
        tb.gameObject.SetActive(false);

        if (!isChecked)
        {
            SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
            sm.Play("Flag");
            ResetFlag(true);
            IsometricManager.Instance.GetManager<GameManager>().CheckClear();
        }
    }

        

    public override void Reset()
    {
        ResetFlag(false);
        StopCoroutine("Transition");
    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
