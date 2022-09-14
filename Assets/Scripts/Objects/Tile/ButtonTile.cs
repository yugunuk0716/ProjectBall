using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : ObjectTile
{
    private ObjectTile target;
    public int index;
    public string targetstring;

    public override void InteractionTile(Ball tb)
    {
        InvokeData();
        tb.SetMove();
    }

    public override void Reset()
    {

    }

/*    public void SetTarget(ObjectTile t,string ts)
    {
        target = t;
        targetString = ts;
    }*/

    public override IEnumerator Transition()
    {
        yield return null;
    }

    public void FindTarget()
    {
        List<ObjectTile> tiles = new List<ObjectTile>(
            IsometricManager.Instance.GetManager<GameManager>().tileDict.Values);

        tiles.ForEach((x) =>
        {
            if(x.btnIndex.Equals(this.btnIndex))
            {
                target = x;
            }
        });

        SetData();
    }

    private void SetData()
    {
        target.dataString = targetstring;
    }

    private void InvokeData()
    {
        target.SetDirection();
    }
}
