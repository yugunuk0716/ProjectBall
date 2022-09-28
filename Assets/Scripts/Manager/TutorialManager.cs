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
    private BallControllUI currentBallUI;
    private Canvas currentSelectedCanvas;
    private int ballCount = 0;
    private int shootTextCount = 1;
    private UnityAction tempAction = null;
    //ManagerBase 구현하기
    public override void Init()
    {
        ballCount = 0;
    }


    public void Start()
    {
        um = IsometricManager.Instance.GetManager<UIManager>();
        gm = IsometricManager.Instance.GetManager<GameManager>();
        arrowText = GameObject.Find("Arrow").GetComponent<RectTransform>();


        turoritalUI = um.canvas[3].GetComponent<TuroritalUI>();

        turoritalUI.TutoPanels.ForEach(x => tutoPanels.Add(x.GetComponent<CanvasGroup>()));

        tutoPanels.ForEach(x => x.DOFade(0, .5f));

        ballCount = 0;

        gm.OnClear += Tiles;
    }

    public override void Load()
    {
    }

    public override void UpdateState(eUpdateState state)
    {
    }

    public IEnumerator StartTurotial()
    {
        um.canvas[3].interactable = true;
        um.canvas[3].blocksRaycasts = true;
        um.canvas[3].DOFade(1f, .5f);



        yield return null;
        SelectBall();
    }

    public void MakeObjectHighLight(GameObject obj)
    {
        obj.gameObject.AddComponent<GraphicRaycaster>();
        obj.gameObject.AddComponent<Canvas>();
        currentSelectedCanvas = obj.gameObject.GetComponent<Canvas>();
        currentSelectedCanvas.overrideSorting = true;
        currentSelectedCanvas.sortingOrder = 210;
    }

    public void MakeObjectUnHighLight(GameObject obj)
    {
        Destroy(obj.GetComponent<GraphicRaycaster>());
        Destroy(obj.GetComponent<Canvas>());
    }

    public void SelectBall()
    {
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        tutoPanels[0].DOFade(1, .5f);

        if (gm.ballUIList.Count <= ballCount)
        {
            Confirm();
        }
        else
        {
            arrowText.offsetMax = new Vector2(arrowText.offsetMax.x + 380 * ballCount, arrowText.offsetMax.y);
            currentBallUI = gm.ballUIList[ballCount];

            MakeObjectHighLight(currentBallUI.gameObject);
            currentBallUI.directionSetBtn.onClick.AddListener(ChooseDir);
        }
    }

    public void ChooseDir()
    {
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        tutoPanels[1].DOFade(1, .5f);

        Destroy(currentBallUI.GetComponent<GraphicRaycaster>());
        Destroy(currentBallUI.GetComponent<Canvas>());


        Button selectDirectionBtn =
            currentBallUI.transform.GetComponentInParent<BallSettingUI>().selectDirectionUI.selectDirectionBtns[ballCount == 0 ? 0 : 2];

        MakeObjectHighLight(selectDirectionBtn.gameObject);

        ballCount++;

        selectDirectionBtn.onClick.AddListener(() =>
        {
            MakeObjectUnHighLight(selectDirectionBtn.gameObject);
            SelectBall();
        });


    }

    public void Confirm()
    {
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        tutoPanels[2].DOFade(1, .5f);

        Button confirmButton = currentBallUI.transform.GetComponentInParent<BallSettingUI>().confirmBtn;

        MakeObjectHighLight(confirmButton.gameObject);
       
        confirmButton.onClick.AddListener(() =>
        {
            MakeObjectUnHighLight(confirmButton.gameObject);
            StartCoroutine(Shoot());
        });
    }

    public IEnumerator Shoot()
    {
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        yield return new WaitForSecondsRealtime(2.5f);
        tutoPanels[3].DOFade(1, .5f);
        Button shootbtn = GameObject.Find("ShootBtn").GetComponent<Button>();

        MakeObjectHighLight(shootbtn.gameObject);

        tempAction = () =>
        {
            MakeObjectUnHighLight(shootbtn.gameObject);
            StartCoroutine(Shooted());
        };

       shootbtn.onClick.AddListener(tempAction);
    }

    public IEnumerator Shooted()
    {
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
                um.canvas[3].interactable = false;
                um.canvas[3].blocksRaycasts = false;
                shootbtn.onClick.RemoveListener(tempAction);
                shootbtn.onClick.AddListener(() =>
                {
                    if(Time.timeScale.Equals(0))
                    {
                        Time.timeScale = 1;
                    }
                });
                break;
        }
        shootTextCount++;
    }

    public void Tiles(int a)
    {
        if(shootTextCount != 1)
        {
            Debug.Log("이제 제발..");
        }
    }
}
