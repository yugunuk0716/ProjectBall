using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

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
        Button Shootbtn = GameObject.Find("ShootBtn").GetComponent<Button>();

        MakeObjectHighLight(Shootbtn.gameObject);

        Shootbtn.onClick.AddListener(() =>
        {
            MakeObjectUnHighLight(Shootbtn.gameObject);
            StartCoroutine(Shooted());
        });
    }

    public IEnumerator Shooted()
    {
        tutoPanels.ForEach(x => x.DOFade(0, .5f));
        yield return new WaitForSecondsRealtime(2f);
        tutoPanels[4].DOFade(1, .5f).SetUpdate(true);

        TextMeshProUGUI timer = IsometricManager.Instance.GetManager<UIManager>().uis[1].GetComponent<IngameUI>().timer_text;

        MakeObjectHighLight(timer.gameObject);
        Time.timeScale = 0;
    }
}
