using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IntListBox : ListBox
{
    public Text _contentText;
    public Image minimapImage;
    public List<Sprite> minimapSpriteList;


    protected override void UpdateDisplayContent(object content)
    {
        _contentText.text = ((int)content).ToString();

        if((int)content - 1 < minimapSpriteList.Count)
        {
            minimapImage.sprite = minimapSpriteList[(int)content - 1];
        }
        
    }
}

