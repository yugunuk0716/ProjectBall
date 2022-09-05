using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGoal : Goal
{
    public Sprite[] headSprites;
    public Color[] colors;
    public Color successColor;

    private Dictionary<Color, Sprite> spriteDictionary = new Dictionary<Color, Sprite>();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < headSprites.Length; i++)
        {
            spriteDictionary.Add(colors[i], headSprites[i]);
        }
    }

    public void SetSuccessColor(Color successColor)
    {
        this.successColor = successColor;
        sr.sprite = spriteDictionary[successColor];
    }

    public override void InteractionTile(Ball tb)
    {
        if (!successColor.Equals(tb.currentColor))
        {
            PoolManager.Instance.Push(tb);
            return;
        }

        base.InteractionTile(tb);
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
