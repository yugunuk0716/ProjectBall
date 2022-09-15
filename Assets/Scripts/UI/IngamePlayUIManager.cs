using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngamePlayUIManager : UIBase
{
    //[SerializeField]
    //private Scrollbar scrollBar;

    [Header("Panel"), Space(10)] 
    [SerializeField] RectTransform settingPanel; // scroll value = 0
    [SerializeField] RectTransform shootPanel; // scroll value = 1

    [Header("Button"), Space(10)]
    [SerializeField] Button settingPanelOnBtn;
    [SerializeField] Button shootPanelOnBtn;

    [Header("Float"), Space(10)]
    [SerializeField] float swipeTime = 0.2f;

    [Header("UI About Ingame PlayUI"), Space(10)]
    [SerializeField] List<UIBase> playUIs = new List<UIBase>();

    private float startTouchX;
    private float endTouchX;

    private bool isSetPanelActive = true;
    private bool isSwiping = false;

    private Vector3 big = new Vector3(1.2f, 1.2f, 1.2f);

    private void Update()
    {
#if UNITY_EDITOR
        // 마우스 왼쪽 버튼을 눌렀을 때 1회
        if (Input.GetMouseButtonDown(0))
        {
            // 터치 시작 지점 (Swipe 방향 구분)
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 터치 종료 지점 (Swipe 방향 구분)
            endTouchX = Input.mousePosition.x;
            bool isLeft = startTouchX < endTouchX ? true : false;
            Swipe(isLeft);
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 터치 시작 지점 (Swipe 방향 구분)
                startTouchX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // 터치 종료 지점 (Swipe 방향 구분)
                endTouchX = touch.position.x;

                // 넘겨
                bool isLeft = startTouchX < endTouchX ? true : false;
                Swipe(isLeft);
            }
        }
#endif
    }


    private void Swipe(bool isLeft)
    {
        if (isSwiping)
            return;

        #region
        //if(!isLeft_setted.HasValue && Mathf.Abs(endTouchX - startTouchX) < 80)
        //{
        //    // 뒤로 돌아가기
        //    isActivePanelEuqalSettingPanel = !isActivePanelEuqalSettingPanel;
        //    StartCoroutine(CoSwipeOtherPanel());
        //    return;
        //}
        #endregion

        if (isLeft && false == isSetPanelActive) // 왼쪽으로 이동, 슈팅 패널이 켜있음
        {
            DoTweenMove(shootPanel, settingPanel);
        }
        else if(!isLeft && isSetPanelActive) // 오른쪽 이동, 세팅 패널이 켜있음
        {
            DoTweenMove(settingPanel, shootPanel);
        }
    }

    public void DoTweenMove(RectTransform activedPanel, RectTransform activePanel)
    {
        isSwiping = true;

        if (isSetPanelActive)
        {
            BtnCloseUp(settingPanelOnBtn, shootPanelOnBtn);
        }
        else
        {
            BtnCloseUp(shootPanelOnBtn, settingPanelOnBtn);
        }

        int targetPos = isSetPanelActive ? -1080 : 1080;

        Sequence seq = DOTween.Sequence();
        seq.Append(activedPanel.DOAnchorPosX(targetPos, 0.6f).SetEase(Ease.OutCubic));
        seq.Append(activePanel.GetComponent<RectTransform>().DOAnchorPosX(0, 1f).SetEase(Ease.OutBack).
            OnComplete(() =>
            {
                isSetPanelActive = !isSetPanelActive;
                isSwiping = false;
            }));
    }

    //private IEnumerator CoSwipeOtherPanel()
    //{
    //    float start = scrollBar.value;
    //    float current = 0;
    //    float percent = 0;
    //
    //    isSwiping = true;
    //
    //    float end = isActivePanelEuqalSettingPanel ? 1 : 0;
    //
    //
    //    if (isActivePanelEuqalSettingPanel)
    //    {
    //        BtnCloseUp(settingPanelOnBtn, shootPanelOnBtn);
    //
    //    }
    //    else
    //    {
    //        BtnCloseUp(shootPanelOnBtn, settingPanelOnBtn);
    //    }
    //
    //
    //    while (percent < 1)
    //    {
    //        current += Time.deltaTime;
    //        percent = current / swipeTime;
    //
    //        scrollBar.value = Mathf.Lerp(start, end, percent);
    //
    //        yield return null;
    //    }
    //
    //
    //    isActivePanelEuqalSettingPanel = end == 0 ? true : false;
    //    isSwiping = false;
    //}

    public void BtnCloseUp(Button btn1, Button btn2)
    {
        btn1.transform.DOScale(Vector3.one, swipeTime);
        btn2.transform.DOScale(big, swipeTime);

        btn1.transform.SetSiblingIndex(0);
        btn2.transform.SetSiblingIndex(1);

        btn1.image.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        btn2.image.color = Color.white;
    }

    public override void Init()
    {
        settingPanelOnBtn.transform.DOScale(big, 0.5f);
        settingPanelOnBtn.onClick.AddListener(() => Swipe(true));
        shootPanelOnBtn.onClick.AddListener(() => Swipe(false));

        playUIs.ForEach((x) => x.Init());
    }

    public override void Load()
    {
        playUIs.ForEach((x) => x.Load());
        if (false == isSetPanelActive) // 왼쪽으로 이동, 슈팅 패널이 켜있음
        {
            DoTweenMove(shootPanel, settingPanel);
        }
    }


}
