using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IntListBox : ListBox
{
    public Text _contentText;
    public Image minimapImage;
    public Image lockImage;
    public List<Sprite> minimapSpriteList;

    public int lastIndex = 0;

    public List<Image> starList;
    public GameObject lockObj;
    public GameObject starObj;

    protected override void UpdateDisplayContent(object content)
    {
        lastIndex = (int)content;
        _contentText.text = lastIndex.ToString();
        UpdateContents?.Invoke(this, lastIndex);


        if (lastIndex - 1 < minimapSpriteList.Count)
        {
            lockImage.sprite = minimapSpriteList[lastIndex - 1];
            minimapImage.sprite = minimapSpriteList[lastIndex - 1];
        }

        //int star = PlayerPrefs.GetInt($"{lastIndex - 1}Stage", 0);
        //print(lastIndex);
        //int clearStage = PlayerPrefs.GetInt("ClearMapsCount", 0);
        //print($"별 {star}");

       


    }

    public void SetLock(bool on)
    {
        lockObj.SetActive(on);
    }

    public void UpdateContent()
    {
        UpdateDisplayContent(lastIndex);
    }
}

