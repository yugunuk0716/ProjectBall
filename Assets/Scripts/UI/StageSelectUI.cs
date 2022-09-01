using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class StageSelectUI : MonoBehaviour
{
    RectTransform myRectTrm;
    [SerializeField] private Transform stageLoadBtnsContent;
    public Button stageLoadBtnPrefab;
    public Button exitBtn;

    bool isHiden = true;
    bool isMoving = false;
    float moveDist = 0f;

    void Start()
    {
        moveDist = Screen.height;
        myRectTrm = GetComponent<RectTransform>();

        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();
        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();

        for (int i = 0; i < gm.mapRangeStrArray.Length; i++)
        {
            Button btn = Instantiate(stageLoadBtnPrefab, stageLoadBtnsContent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            btn.onClick.AddListener(() =>
            {
                sm.LoadStage(gm.mapinfos[i]);
                Move(1f);
            });
        }

        exitBtn.onClick.AddListener(() => Move());
    }

    public void Move(float duration = 0.5f)
    {
        if (isMoving) return;

        isMoving = true;
        float value = isHiden ? moveDist : -moveDist;

        Ease ease = isHiden ? Ease.InOutQuad : Ease.OutBounce;
        myRectTrm.DOMoveY(myRectTrm.anchoredPosition.y + value, duration).SetEase(ease).OnComplete(() =>
        {
            isMoving = false;
            isHiden = !isHiden;
        });
    }

}
