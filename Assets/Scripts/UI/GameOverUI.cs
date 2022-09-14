using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button reloadBtn;
    [SerializeField] private Button loadNextBtn;
    [SerializeField] private TextMeshProUGUI gameOverText;

    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        IsometricManager.Instance.GetManager<GameManager>().ActiveGameOverPanel = (bool isClear) => OnGameOver(isClear);

        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        reloadBtn.onClick.AddListener(() => 
        {
            sm.LoadStage(Resources.Load<StageDataSO>($"Stage {sm.stageIndex}"));
            ScreenOn(false);
        });

        loadNextBtn.onClick.AddListener(() =>
        {
            sm.stageIndex++;
            sm.LoadStage(Resources.Load<StageDataSO>($"Stage {sm.stageIndex}"));
            ScreenOn(false);
        });
    }

    public void OnGameOver(bool isClear)
    {
        ScreenOn(true);

        loadNextBtn.interactable = isClear;
        loadNextBtn.image.color = isClear ? Color.white : Color.gray;
        gameOverText.text = isClear ? "Clear!" : "Failed..";
    }

    public void ScreenOn(bool on)
    {
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.alpha = on ? 1 : 0;
    }
}
