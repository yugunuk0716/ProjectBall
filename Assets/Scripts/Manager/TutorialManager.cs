using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class TutorialManager : ManagerBase
{
    private List<CanvasGroup> tutoPanels = new List<CanvasGroup>();
    private TuroritalUI turoritalUI;
    private RectTransform arrowText;
    private GameManager gm;
    private UIManager um;
    //private BallControllUI currentBallUI;
    private Canvas currentSelectedCanvas;
    private int ballCount = 0;
    private int shootTextCount = 2;
    private List<UnityAction> tempAction = new List<UnityAction>();
    private bool istuto = false;


    public override void Init()
    {
        ballCount = 0;
        tempAction.Clear();
        istuto = false;
    }


    public void Start()
    {
        um = IsometricManager.Instance.GetManager<UIManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();
        arrowText = GameObject.Find("Arrow").GetComponent<RectTransform>();


        turoritalUI = um.canvas[3].GetComponent<TuroritalUI>();

        turoritalUI.TutoPanels.ForEach(x => tutoPanels.Add(x.GetComponent<CanvasGroup>()));

        tutoPanels.ForEach(x => x.DOFade(0, .5f));
    }

    public override void Load()
    {
    }

    public override void UpdateState(eUpdateState state)
    {
    }

    public IEnumerator StartTurotial()
    {
        IsometricManager.Instance.GetManager<GameManager>().ballUIList.ForEach((x) => x.isTutoOrShooting = true);
        um.canvas[3].interactable = true;
        um.canvas[3].blocksRaycasts = true;
        um.canvas[3].DOFade(1f, .5f);



        yield return null;
        SelectBall();
    }

    public void MakeObjectHighLight(GameObject obj)
    {
        obj.gameObject.AddComponent<Canvas>();
        obj.gameObject.AddComponent<GraphicRaycaster>();
        
        currentSelectedCanvas = obj.gameObject.GetComponent<Canvas>();
        currentSelectedCanvas.overrideSorting = true;
        currentSelectedCanvas.sortingOrder = 210;
    }

    public void MakeObjectUnHighLight(GameObject obj)
    {
        Destroy(obj.GetComponent<GraphicRaycaster>());
        Destroy(obj.GetComponent<Canvas>());
    }

    public UnityAction MakeDisposableAction(UnityAction inputAction,Button button)
    {
        UnityAction action = null;
        
        action = () =>
        {
            inputAction();
            button.onClick.RemoveListener(action);
        };
        
        return action;
    }

    public void SelectBall()
    {
        Debug.Log("SelectBall");
        Debug.Log(ballCount);
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        tutoPanels[0].DOFade(1, .5f);

        if (2 == ballCount)
        {
            Confirm();
        }
        else
        {
            arrowText.offsetMax = new Vector2(arrowText.offsetMax.x + 380 * ballCount, arrowText.offsetMax.y);

            MakeObjectHighLight(gm.ballUIList[ballCount].gameObject);
            tempAction.Add(() => ChooseDir());
            gm.ballUIList[ballCount].directionSetBtn.onClick.AddListener(
                MakeDisposableAction(tempAction[^1], gm.ballUIList[ballCount].directionSetBtn));
        }
    }

    public void ChooseDir()
    {
        Debug.Log("ChooseDir");
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        tutoPanels[1].DOFade(1, .5f);

        MakeObjectUnHighLight(gm.ballUIList[ballCount].gameObject);


        Button selectDirectionBtn =
            gm.ballUIList[ballCount].transform.GetComponentInParent<BallSettingUI>().selectDirectionUI.selectDirectionBtns[ballCount == 0 ? 0 : 2];

        MakeObjectHighLight(selectDirectionBtn.gameObject);

        ballCount++;

        tempAction.Add(() =>
        {
            MakeObjectUnHighLight(selectDirectionBtn.gameObject);
            SelectBall();
        });

        selectDirectionBtn.onClick.AddListener(MakeDisposableAction(tempAction[^1], selectDirectionBtn));


    }

    public void Confirm()
    {
        Debug.Log("Confirm");
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        tutoPanels[2].DOFade(1, .5f);

        Button confirmButton = gm.ballUIList[ballCount-1].transform.GetComponentInParent<BallSettingUI>().confirmBtn;

        MakeObjectHighLight(confirmButton.gameObject);

        tempAction.Add(() =>
        {
            MakeObjectUnHighLight(confirmButton.gameObject);
            StartCoroutine(Shoot());
        });

        confirmButton.onClick.AddListener(MakeDisposableAction(tempAction[^1],confirmButton));
    }

    public IEnumerator Shoot()
    {
        Debug.Log("Shoot");
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        yield return new WaitForSecondsRealtime(2.5f);
        tutoPanels[3].DOFade(1, .5f);
        Button shootbtn = GameObject.Find("ShootBtn").GetComponent<Button>();

        MakeObjectHighLight(shootbtn.gameObject);

        tempAction.Add(() =>
        {
            MakeObjectUnHighLight(shootbtn.gameObject);
            StartCoroutine(Shooted());
        });

        shootbtn.onClick.AddListener(MakeDisposableAction(tempAction[^1], shootbtn));
    }

    public IEnumerator Shooted()
    {
        Debug.Log("Shooted");
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        yield return new WaitForSecondsRealtime(1.3f);
        tutoPanels[4].DOFade(1, .5f).SetUpdate(true);

        TextMeshProUGUI timer = IsometricManager.Instance.GetManager<UIManager>().uis[1].GetComponent<IngameUI>().timer_text;

        MakeObjectHighLight(timer.gameObject);
        tutoPanels[4].GetComponent<Button>().onClick.AddListener(ShootTextChange);
        tutoPanels[4].transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        tutoPanels[4].transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(1, 1f).SetUpdate(true);
        
        Time.timeScale = 0;
    }

    public void ShootTextChange()
    {
        Debug.Log("ShootTextChange");
        TextMeshProUGUI timer = IsometricManager.Instance.GetManager<UIManager>().uis[1].GetComponent<IngameUI>().timer_text;
        Tilemap map = IsometricManager.Instance.GetManager<SaveManager>().mainMap;
        

        switch (shootTextCount)
        {
            
            case 2:
                tutoPanels[4].transform.GetChild(shootTextCount - 1).GetComponent<CanvasGroup>().DOFade(0, .5f).SetUpdate(true);
                MakeObjectUnHighLight(timer.gameObject);
                //DOTween.To(() => map.color, x => map.color = x, new Color(1,1,1,0), .5f).SetUpdate(true);
                tutoPanels[4].transform.GetChild(shootTextCount).GetComponent<CanvasGroup>().DOFade(1, .5f).SetUpdate(true);
                break;
            case 3:
                tutoPanels[4].DOFade(0, .5f).SetUpdate(true);
                //DOTween.To(() => map.color, x => map.color = x, new Color(1, 1, 1, 1), .5f).SetUpdate(true);
                Button shootbtn = GameObject.Find("ShootBtn").GetComponent<Button>();
                um.canvas[3].alpha = 0f;
                um.canvas[3].interactable = false;
                um.canvas[3].blocksRaycasts = false;
                istuto = true;
                //Tiles();
                //shootbtn.onClick.RemoveListener(tempAction[^1]);
                shootbtn.onClick.AddListener(MakeDisposableAction(() =>
                {
                    Time.timeScale = 1;
                    //tutoPanels[4].GetComponent<Button>().onClick.RemoveListener(ShootTextChange);
                }, shootbtn));
                
                break;
        }
        shootTextCount++;
    }


}
