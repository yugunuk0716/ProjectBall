using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FlagColors
{
    Red,
    Orange,
    Yello,
    LightYello,
    YelloGreen,
    Green,
    SkyBlue,
    Blue,
    Navy,
    Purple
};

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

        switch (ColorUtility.ToHtmlStringRGB(successColor))
        {
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[0])):
                animator.SetInteger("ColorIndex", 0);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[1])):
                animator.SetInteger("ColorIndex", 1);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[2])):
                animator.SetInteger("ColorIndex", 2);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[3])):
                animator.SetInteger("ColorIndex", 3);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[4])):
                animator.SetInteger("ColorIndex", 4);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[5])):
                animator.SetInteger("ColorIndex", 5);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[6])):
                animator.SetInteger("ColorIndex", 6);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[7])):
                animator.SetInteger("ColorIndex", 7);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[8])):
                animator.SetInteger("ColorIndex", 8);
                break;
            case string s when s.Equals(ColorUtility.ToHtmlStringRGB(colors[9])):
                animator.SetInteger("ColorIndex", 9);
                break;
        }
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
