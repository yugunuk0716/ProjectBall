using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : ObjectTile
{
    public List<ObjectTile> target = new List<ObjectTile>();
    public override void InteractionTile(Ball tb)
    {
        tb.SetMove();
        InvokeData();
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

        tiles.FindAll(x => x.btnIndex == this.btnIndex && x != this).ForEach((x) => Debug.Log(x));

        target = tiles.FindAll(x => x.btnIndex == this.btnIndex && x != this);

        SetData();
    }

    private void SetData()
    {
        //target 내용중 .dataString에 targetstring을 넣는다
        target.ForEach(x => x.dataString = x.btnString);

        /*//target에 있는 .btnstring을 유니티 콘솔에 출력한다
        target.ForEach(x => Debug.Log(x));*/
    }

    private void InvokeData()
    {
        //target 의 Setdiraction()을 실행
        target.ForEach(x => x.SetDirection());
    }
}
