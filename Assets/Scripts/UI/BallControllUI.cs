using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BallControllUI : UIBase, IBeginDragHandler, IEndDragHandler, IDragHandler, IPoolableComponent
{
    [HideInInspector] public RectTransform rt;
    public Button directionSetBtn;
    public int order;
    public Image directionImg;
    [SerializeField] private Image bgImage;
    [HideInInspector] public SwapUI swapUI; // it have evry data and controll others

    public Ball ball;

    public bool isTutoOrShooting = false;
    float pressedTime = 0f;
    bool bPressed;

    float checkTime = 0.2f;

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
       
    }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        bgImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (bPressed)
        {
            pressedTime += Time.deltaTime;
            if (pressedTime >= checkTime)
            {
                BeginSwapping();
                bPressed = false;
            }
        }
    }

    public void SetInteractValues(bool on)
    {
        if (on)
        {
            directionSetBtn.interactable = on;
            directionSetBtn.image.raycastTarget = on;
        }
        bgImage.raycastTarget = on;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1 || isTutoOrShooting) return;

        bPressed = true;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1) return;

        if (isTutoOrShooting)
        {
            return;
        }
        bPressed = false;
        rt.DOComplete();
        if (pressedTime > checkTime)
        {
            swapUI.OnEndDrag();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            bPressed = false;
            rt.DOComplete();
            if (pressedTime > checkTime)
            {
                swapUI.OnEndDrag();
            }
            pressedTime = 0f;
        }

    }

    public void BeginSwapping()
    {
        GameManager.canInteract = false;
        swapUI.ballControllUI = this;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        swapUI.transform.position = pos;

        swapUI.On(true);

        this.order = 10;
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        gm.BallUiSort();
        gm.ballUIList.ForEach((x) =>
        {
            x.directionSetBtn.interactable = false;
            x.directionSetBtn.image.raycastTarget = false;
        });

        rt.sizeDelta = new Vector2(0, 190);
        directionImg.rectTransform.localScale = new Vector2(0, 1);
    }


    public void Despawned()
    {

    }

    public void Spawned()
    {
        directionSetBtn.onClick.RemoveAllListeners();
        directionImg.gameObject.SetActive(false);
        isTutoOrShooting = false;
    }

    public void SetDisable()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
    }
}
