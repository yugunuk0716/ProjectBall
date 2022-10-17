using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTile : ObjectTile
{
    public List<ObjectTile> target = new List<ObjectTile>();
    public List<Sprite> spriteList = new List<Sprite>();
    private SpriteRenderer sr;
    public bool isChecked = false;

    public void OnEnable()
    {
        Spawned();
    }

    public override void InteractionTile(Ball tb)
    {
        if (isChecked)
        {
            tb.SetMove();
            return;
        }

        InvokeData();
        isChecked = true;
        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Button");
        if (sr.sprite.Equals(spriteList[0]))
        {
            sr.sprite = spriteList[1];
        }

        tb.SetDisable();

        StageManager stageManager = IsometricManager.Instance.GetManager<StageManager>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        if (!stageManager.isMapLoading)
        {
            ++gm.curDestroyedBallsCount;
            gm.CheckFail();
        }
    }

    public new void Spawned()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = spriteList[0];
        isChecked = false;
    }

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
       
        target.ForEach(x => x.dataString = x.btnString);


    }

    private void InvokeData()
    {

        target.ForEach(x => x.SetDirection());
    }
}
