using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : ObjectTile
{
    public List<ObjectTile> target = new List<ObjectTile>();
    public List<Sprite> spriteList = new List<Sprite>();
    private SpriteRenderer sr;
    public override void InteractionTile(Ball tb)
    {
        InvokeData();

        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Button");

        if (sr.sprite.Equals(spriteList[0]))
        {
            sr.sprite = spriteList[1];
        }

        PoolManager.Instance.Push(tb);
    }

    public override void Reset()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = spriteList[0];
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
