using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameOverUI : UIBase
{
    [SerializeField] private Button reloadBtn;
    [SerializeField] private Button loadNextBtn;
    [SerializeField] private Button descriptionBtn;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI recordText;

    public CanvasGroup descriptionPanel;
    public Button descriptionCancleBtn;

    StageManager sm;
    GameManager gm;
    SoundManager soundm;
    public List<Image> starList = new List<Image>();
    public bool isClear = false;

    private bool canRaiseStageIdx;
 

    public void OnGameOver(bool isClear)
    {
        ScreenOn(true);

        this.isClear = isClear;
        loadNextBtn.interactable = isClear;
        loadNextBtn.image.color = isClear ? Color.white : new Color(1,1,1, 0.4f);
        gameOverText.text = isClear ? "Clear!" : "Failed..";
    }

    public override void Init()
    {
        GetCanvasGroup();

        sm = IsometricManager.Instance.GetManager<StageManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();
        soundm = IsometricManager.Instance.GetManager<SoundManager>();
      
        gm.ActiveGameOverPanel = (bool isClear) => OnGameOver(isClear);
        gm.OnClear += SetStar;

        reloadBtn.onClick.AddListener(() =>
        {
            if (isClear)
            {
                sm.clearMapCount++;
                
            }
            PlayerPrefs.SetInt("ClearMapsCount", sm.clearMapCount);
            canRaiseStageIdx = false;
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

        descriptionBtn.onClick.AddListener(() =>
        {

            descriptionPanel.interactable = true;
            descriptionPanel.blocksRaycasts = true;
            descriptionPanel.DOFade(1f, 0.5f).SetUpdate(true);
        });

        descriptionCancleBtn.onClick.AddListener(() =>
        {
    
            descriptionPanel.interactable = false;
            descriptionPanel.blocksRaycasts = false;
            descriptionPanel.DOFade(0f, 0.5f).SetUpdate(true);
        });


    }

    public void SetStar(int starCount, float clearTime)
    {
        StartCoroutine(StarOnRoutine(starCount, clearTime));
    }


    IEnumerator StarOnRoutine(int starCount, float clearTime)
    {
        reloadBtn.interactable = false;
        loadNextBtn.interactable = false;

      
        recordText.text = $"Left Time : {clearTime.ToString("F2")}";
    

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < starCount; i++)
        {
            starList[i].gameObject.SetActive(true);
            soundm.Play("Star");
            yield return new WaitForSeconds(0.5f);
        }

        gm.clearParticle_Left.Play();
        gm.clearParticle_Right.Play();

        yield return new WaitForSeconds(0.25f);
        soundm.Play("Trumpet");

        canRaiseStageIdx = true;

 
        reloadBtn.interactable = true;
        loadNextBtn.interactable = isClear;


        
        isClear = false;
    }

    public override void ScreenOn(bool on)
    {
       
        base.ScreenOn(on);
    }

    public override void Load()
    {

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
