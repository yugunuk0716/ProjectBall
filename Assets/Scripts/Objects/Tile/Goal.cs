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
        PoolManager.Instance.Push(tb);
        if (!isChecked)
        {
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
