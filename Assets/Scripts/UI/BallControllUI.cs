using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BallControllUI : UIBase, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public RectTransform rt;
    public Button directionSetBtn;
    public int order;
     public Image directionImg;
    [SerializeField] private Image bgImage;
    [HideInInspector] public SwapUI swapUI; // 이 친구한테 데이터를 넣어주고 얘가 알아서 조종하거

    public Ball ball;

    public void SetDirection(TileDirection dir, bool active = true)
    {
        float z = 0;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                z = 0f;
                break;
            case TileDirection.RIGHTDOWN:
                z = 270f;
                break;
            case TileDirection.LEFTDOWN:
                z = 180f;
                break;
            case TileDirection.LEFTUP:
                z = 90;
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

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        bgImage = GetComponent<Image>();
    }

    public override void Reset()
    {
        directionSetBtn.interactable = true;
        directionSetBtn.image.raycastTarget = true;
        directionImg.gameObject.SetActive(false);
    }

    public void SetInteractValues(bool on)
    {
        if(on)
        {
            directionSetBtn.interactable = on;
            directionSetBtn.image.raycastTarget = on;
        }
        bgImage.raycastTarget = on;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        swapUI.ballControllUI = this;
        swapUI.gameObject.SetActive(true);

        this.order = 10;
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.BallUiSort();
        gm.ballUIList.ForEach((x) =>
        {
            x.directionSetBtn.interactable = false;
            x.directionSetBtn.image.raycastTarget = false;
        });

        directionImg.transform.DOScaleX(0,  0.45f);
        rt.DOSizeDelta(new Vector2(0, 190), 0.45f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rt.DOComplete();
        swapUI.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
