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
    [SerializeField] private Image directionImg;

    [HideInInspector] public SwapUI swapUI; // 이 친구한테 데이터를 넣어주고 얘가 알아서 조종하거

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
    }

    public override void Reset()
    {
        directionSetBtn.interactable = true;
        directionSetBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50, 0);
        directionImg.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        swapUI.ballControllUI = this;
        swapUI.gameObject.SetActive(true);
        gameObject.transform.localScale = new Vector3(0, 1, 1);

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        for (int i = order + 1; i < gm.ballUIList.Count; i++)
        {
            gm.ballUIList[i].order--;
        }
        gm.BallUiSort();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        swapUI.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
