using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IntListBox : ListBox
{
    public Text _contentText;
    public Image minimapImage;
    public List<Sprite> minimapSpriteList;


    public List<Image> starList;
    public GameObject lockObj;
    public GameObject starObj;

    protected override void UpdateDisplayContent(object content)
    {
        _contentText.text = ((int)content).ToString();

        
        if ((int)content - 1 < minimapSpriteList.Count)
        {
            minimapImage.sprite = minimapSpriteList[(int)content - 1];
        }

        int star = PlayerPrefs.GetInt($"{(int)content - 1}Stage", 0);
        print($"별 {star}, 슼떼이쥐 {(int)content - 1}");
        if(star == 0 && (int)content - 1 != 1)
        {
            lockObj.SetActive(true);
            starObj.SetActive(false);
        }
        else
        {
            lockObj.SetActive(false);
            starObj.SetActive(true);

            for (int i = 0; i < star; i++)
            {
                starList[i].gameObject.SetActive(true);
            }
        }

    }
}

