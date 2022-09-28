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
    StageManager sm;
    GameManager gm;
    SoundManager soundm;
    public List<Image> starList = new List<Image>();
    public bool isClear = false;

    private bool canRaiseStageIdx;
 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print($"cc: {sm.clearMapCount + 1}  idx: {sm.stageIndex}");
        }
    }

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

        sm = IsometricManager.Instance.GetManager<StageManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();
        soundm = IsometricManager.Instance.GetManager<SoundManager>();
      

        gm.OnClear = SetStar;

        reloadBtn.onClick.AddListener(() =>
        {
            if (isClear)
            {
                sm.clearMapCount++;
            }
            PlayerPrefs.SetInt("ClearMapsCount", sm.clearMapCount);
            canRaiseStageIdx = false;
            print(sm.stageIndex);
            sm.LoadStage(sm.stageIndex);
            ScreenOn(false);
            starList.ForEach(s => s.gameObject.SetActive(false));
        });

        loadNextBtn.onClick.AddListener(() =>
        {
            canRaiseStageIdx = false;
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
            soundm.Play("Star");
            yield return new WaitForSeconds(0.75f);
        }

        gm.clearParticle_Left.Play();
        gm.clearParticle_Right.Play();

        yield return new WaitForSeconds(0.25f);
        soundm.Play("Trumpet");

        canRaiseStageIdx = true;
        reloadBtn.interactable = true;
        loadNextBtn.interactable = isClear;
        //loadNextBtn.image.color = isClear ? Color.white : Color.gray;
    }

    public override void ScreenOn(bool on)
    {
       
        base.ScreenOn(on);
    }

    public override void Load()
    {

    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }


    private void OnApplicationQuit()
    {
        if (canRaiseStageIdx)
        {
            canRaiseStageIdx = false;
            sm.stageIndex++;
            PlayerPrefs.SetInt("ClearMapsCount", sm.stageIndex);
        }
    }
}
