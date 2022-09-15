using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : UIBase
{
    [SerializeField] private Button reloadBtn;
    [SerializeField] private Button loadNextBtn;
    [SerializeField] private TextMeshProUGUI gameOverText;

    public List<Image> starList = new List<Image>();
    public bool isClear = false;

    public void OnGameOver(bool isClear)
    {
        ScreenOn(true);

        this.isClear = isClear;
        gameOverText.text = isClear ? "Clear!" : "Failed..";
    }

    public override void Init()
    {
        GetCanvasGroup();
        IsometricManager.Instance.GetManager<GameManager>().ActiveGameOverPanel = (bool isClear) => OnGameOver(isClear);

        StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        gm.OnClear = SetStar;

        reloadBtn.onClick.AddListener(() =>
        {
            sm.LoadStage(sm.stageIndex);
            ScreenOn(false);
            starList.ForEach(s => s.gameObject.SetActive(false));
        });

        loadNextBtn.onClick.AddListener(() =>
        {
            sm.stageIndex++;
            sm.LoadStage(sm.stageIndex);
            ScreenOn(false);
            starList.ForEach(s => s.gameObject.SetActive(false));
        });
    }

    public void SetStar(int starCount)
    {
        StartCoroutine(StarOnRoutine(starCount));
    }


    IEnumerator StarOnRoutine(int starCount)
    {
        reloadBtn.interactable = false;
        loadNextBtn.interactable = false;

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < starCount; i++)
        {
            starList[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.75f);
        }

        reloadBtn.interactable = true;
        loadNextBtn.interactable = isClear;
        loadNextBtn.image.color = isClear ? Color.white : Color.gray;
    }

    public override void Load()
    {

    }
}
