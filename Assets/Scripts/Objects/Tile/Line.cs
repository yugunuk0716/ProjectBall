using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : ObjectTile
{
    [SerializeField]
    private GameObject LT, RT, RB, LB;
    public override void InteractionTile(Ball tb)
    {

    }

    public override IEnumerator Transition()
    {
        yield return null;
    }

    public void SetLineDir(bool lt = false, bool rt = false, bool lb = false, bool rb = false)
    {
        LT.SetActive(lt);
        RT.SetActive(rt);
        LB.SetActive(lb);
        RB.SetActive(rb);
    }
}
