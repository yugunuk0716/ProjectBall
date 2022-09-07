using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class StageSelectUI : MonoBehaviour
{
    RectTransform myRectTrm;
    [SerializeField] private RectTransform stageLoadBtnsContent;
    public Button stageLoadBtnPrefab;
    public Button exitBtn;

    bool isHiden = true;
    bool isMoving = false;
    float moveDist = 0f;

    IEnumerator CoStart()
    {
        yield return null;
        moveDist = stageLoadBtnsContent.rect.size.y;

        moveDist = Screen.height / 6;
        myRectTrm = GetComponent<RectTransform>();

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();

        int loadMapCount = (PlayerPrefs.GetInt("ClearMapsCount") / 3 + 1) * 3;

        gm.MakeNewStageBtn += ((x) =>
        {
            Button btn = Instantiate(stageLoadBtnPrefab, stageLoadBtnsContent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = (x + 1).ToString();
            btn.onClick.AddListener(() =>
            {
                sm.stageIndex = x + 1; // 현재 플레이중인 스테이지..를 알고 있음 뭔가 도움되겠지 뭐
                sm.LoadStage(Resources.Load<StageDataSO>($"Stage {sm.stageIndex}"));

                Move();
            });
        });

        for (int i = 0; i < loadMapCount; i++)
        {
            gm.MakeNewStageBtn(i);
        }

        exitBtn.onClick.AddListener(() => Move());
    }

    void Start()
    {
        StartCoroutine(CoStart());
    }

    public void Move(float duration = 0.5f)
    {
        if (isMoving) return;

        isMoving = true;
        float value = isHiden ? 320 : -320;
        Time.timeScale = isHiden ? 0 : 1;

        Ease ease = isHiden ? Ease.InOutQuad : Ease.OutBounce;
        Debug.Log($"현재 포지션 : {myRectTrm.anchoredPosition.y}");

        myRectTrm.DOMoveY(myRectTrm.anchoredPosition.y + value, duration).SetEase(ease).SetUpdate(true).OnComplete(() =>
        {
            isMoving = false;
            isHiden = !isHiden;
        });
    }

}
