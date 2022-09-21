using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class BallControllUI : UIBase, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button directionSetBtn;
    public TextMeshProUGUI orderText; // 순서
    [HideInInspector] public int order = 0;

    [SerializeField] private Image directionImg;
    Image bgImage;

    public Action BeginDrag;
    public Action<BallControllUI> EndDrag;
    public Canvas canvas { get; set; }
    public RectTransform rt { get; set; }

    private void Awake()
    {
        bgImage = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

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
        directionSetBtn.image.color = Color.white;
        directionSetBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50, 0);
        orderText.SetText(string.Empty);
        order = 0;
        directionImg.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (order > 10) return;

        BeginDrag();
        ChangeColor(false);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (order > 10) return;

        BallControllUI ballControll = null;
        GameObject obj = eventData.hovered.Find((x) => x.name.Contains("BallControllUI") && !ReferenceEquals(x, this.gameObject));
        if (obj != null)
        {
            ballControll = obj.GetComponent<BallControllUI>();
        }

        EndDrag(ballControll);
        transform.localScale = new Vector3(0f, 1, 1);
        transform.DOScaleX(1, 0.4f);
        ChangeColor(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (order > 10) return;
        Vector3 pos = eventData.position;
        pos.z = 0;

        rt.anchoredPosition = pos;
    }

    public void ChangeColor(bool on)
    {
        bgImage.color = on ? Color.white : new Color(1,0,0,0.5f);

        bgImage.maskable = false;
        directionImg.maskable = false;
        directionSetBtn.image.maskable = false;
    }
}
