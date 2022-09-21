using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class BallControllUI : UIBase, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button directionSetBtn;
    public TextMeshProUGUI orderText; // 순서
    [HideInInspector] public int order = 0;

    [SerializeField] private Image directionImg;
    Image bgImage;

    public Action BeginDrag;
    public Action EndDrag;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
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

    Transform beforeParent = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (order > 10) return;

        beforeParent = transform.parent;
        transform.SetParent(transform.parent.parent);
        transform.SetAsLastSibling();
        BeginDrag();
        MaskOn(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (order > 10) return;
        Debug.Log("OnEndDrag");
        transform.SetParent(beforeParent);
        EndDrag();
        MaskOn(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (order > 10) return;
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        pos.y = 100;
        pos.x = Math.Clamp(pos.x, 100, 700);
        transform.localPosition = pos;
    }

    public void MaskOn(bool on)
    {
        bgImage.maskable = on;
        directionSetBtn.image.maskable = on;
        directionImg.maskable = on;
    }
}
