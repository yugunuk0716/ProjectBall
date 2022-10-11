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


    public CanvasGroup descCG;
    public Button descCancleBtn;


    StageManager sm;
    GameManager gm;
    SoundManager soundm;
    UIManager um;
    LifeManager lm;
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
        recordText.text = $"Left Time : {0.ToString("F2")}";
    }

    public override void Init()
    {
        GetCanvasGroup();

        sm = IsometricManager.Instance.GetManager<StageManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();
        um = IsometricManager.Instance.GetManager<UIManager>();
        lm = IsometricManager.Instance.GetManager<LifeManager>();

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
            if (!lm.CanEnterStage())
            {
                print("광고보기");
                um.FindUI("WatchAddPanel").ScreenOn(true);
                return;
            }
            lm.EnterStage();

            canRaiseStageIdx = false;
            sm.LoadStage(sm.stageIndex);
            ScreenOn(false);
            starList.ForEach(s => s.gameObject.SetActive(false));
        });

        loadNextBtn.onClick.AddListener(() =>
        {
            if (!lm.CanEnterStage())
            {
                print("광고보기");
                sm.clearMapCount++;
                PlayerPrefs.SetInt("ClearMapsCount", sm.clearMapCount);
                um.FindUI("WatchAddPanel").ScreenOn(true);
                return;
            }
            lm.EnterStage();

            canRaiseStageIdx = false;
            sm.stageIndex++;
            sm.LoadStage(sm.stageIndex);
            ScreenOn(false);
            starList.ForEach(s => s.gameObject.SetActive(false));
        });

        descriptionBtn.onClick.AddListener(() =>
        {
            descCG.interactable = true;
            descCG.blocksRaycasts = true;
            descCG.DOFade(1f, 0.5f).SetUpdate(true);
        });

        descCancleBtn.onClick.AddListener(() =>
        {
            descCG.interactable = false;
            descCG.blocksRaycasts = false;
            descCG.DOFade(0, 0.5f).SetUpdate(true);
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
