using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IntListBox : ListBox
{
    public Text _contentText;
    public Image minimapImage;
    public List<Sprite> minimapSpriteList;

    private int lastIndex = 0;

    public List<Image> starList;
    public GameObject lockObj;
    public GameObject starObj;

    protected override void UpdateDisplayContent(object content)
    {
        lastIndex = (int)content;
        _contentText.text = lastIndex.ToString();

        
        if (lastIndex - 1 < minimapSpriteList.Count)
        {
            //minimapImage.sprite = minimapSpriteList[lastIndex];
        }

        int star = PlayerPrefs.GetInt($"{lastIndex - 1}Stage", 0);
        print(lastIndex - 1);
        int clearStage = PlayerPrefs.GetInt("ClearMapsCount", 0);
        print($"별 {star}");

        if (star == 0 && lastIndex - 1 != 0 && clearStage + 1 < lastIndex - 1)
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

    public void UpdateContent()
    {
        UpdateDisplayContent(lastIndex);
    }
}

