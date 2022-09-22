using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class BallControllUI : UIBase
{
    public Button directionSetBtn;
    public TextMeshProUGUI orderText; // 순서
    [HideInInspector] public int order = 0;

    [SerializeField] private Image directionImg;

    public void SetBallSprites(Sprite ballSprite)
    {
        directionSetBtn.image.sprite= ballSprite;
    }
    
    public void SetDirection(TileDirection dir, bool active = true)
    {
        float z = 0;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                z = 45f;
                break;
            case TileDirection.RIGHTDOWN:
                z = 315f;
                break;
            case TileDirection.LEFTDOWN:
                z = 225f;
                break;
            case TileDirection.LEFTUP:
                z = 135;
                break;
        }

        directionImg.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        directionImg.gameObject.SetActive(active);
    }

    public override void Init()
    {
        
    }

    public override void Load()
    {
        // 할 거 없음
    }

    public override void Reset()
    {
        directionSetBtn.interactable = true;
        directionSetBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50, 0);
        orderText.SetText(string.Empty);
        directionImg.gameObject.SetActive(false);
    }
}
