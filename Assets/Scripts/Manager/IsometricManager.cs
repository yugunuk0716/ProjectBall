using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class IsometricManager : MonoBehaviour
{

    private static IsometricManager instance;
    public static IsometricManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool isFirst = true;
    public List<ManagerBase> managers;

    public CinemachineVirtualCamera cvCam;

    private Dictionary<float, float> ratioDic = new Dictionary<float, float>();

    private void Awake()
    {
        instance = this;

        float a = (float)Screen.height / (float)Screen.width;

        ratioDic.Add(1f, 5f);
        ratioDic.Add(2f, 10f);
        ratioDic.Add(Mathf.Floor(4f / 3f * 100f) / 100f, 6.5f);
        ratioDic.Add(Mathf.Floor(6f / 5f * 100f) / 100f, 6f);
        ratioDic.Add(Mathf.Floor(16f / 10f * 100f) / 100f, 8f);
        ratioDic.Add(Mathf.Floor(16f / 9f * 100f) / 100f, 8.5f);
        ratioDic.Add(Mathf.Floor(18.5f / 9f * 100f) / 100f, 10f);
        ratioDic.Add(Mathf.Floor(19f / 9f * 100f) / 100f, 10f);
        ratioDic.Add(Mathf.Floor(19.5f / 9f * 100f) / 100f, 10f);
        ratioDic.Add(Mathf.Floor(20f / 9f * 100f) / 100f, 10.5f);
        ratioDic.Add(Mathf.Floor(21f / 9f * 100f) / 100f, 11f);
        ratioDic.Add(Mathf.Floor(23.1f / 9f * 100f) / 100f, 12f);


        print($"캠{Mathf.Floor(4f / 3f * 100f) / 100f} 비{a}");

        if (ratioDic.ContainsKey(Mathf.Floor(a * 100f) / 100f))
        {
            cvCam.m_Lens.OrthographicSize = ratioDic[Mathf.Floor(a * 100f) / 100f];

        }

        managers.Add(GetComponent<UIManager>());

        managers.Add(gameObject.AddComponent<GameManager>());
        managers.Add(gameObject.AddComponent<StageManager>());
        managers.Add(gameObject.AddComponent<SaveManager>());
        managers.Add(gameObject.AddComponent<SoundManager>());
        managers.Add(gameObject.AddComponent<TutorialManager>());

        Application.targetFrameRate = 300;

        PlayerPrefs.SetInt("IsFirst", PlayerPrefs.GetInt("IsFirst",1));

        if (PlayerPrefs.GetInt("IsFirst") == 1)
        {
            FirstCall();
            Debug.Log("최초실행");
        }
        else
        {
            CanvasGroup canvasGroup = GetManager<UIManager>().canvas[3].GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false; 
            Debug.Log("최초실행 아님");
        }



     
        UpdateState(eUpdateState.Init);
    }

    /*public void Update()
    {
        Debug.Log(Time.timeScale);
    }*/


    private void FirstCall()
    {
        PlayerPrefs.SetInt("ClearMapsCount", 1);
        PlayerPrefs.SetInt("IsFirst", 0);
        GetManager<UIManager>().canvas[0].GetComponent<TitleUI>().
            titleBtns[1].onClick.AddListener(() =>
            {
                StartCoroutine(GetManager<TutorialManager>().StartTurotial());
            });
        PlayerPrefs.Save();
    }

    public static Vector2 GetIsoDir(TileDirection dir) // 등각투형에 걸맞는 벡터로..
    {
        Vector2 vec = Vector2.zero;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                vec = Vector2.right;
                break;
            case TileDirection.LEFTDOWN:
                vec = Vector2.left;
                break;
            case TileDirection.LEFTUP:
                vec = Vector2.up;
                break;
            case TileDirection.RIGHTDOWN:
                vec = Vector2.down;
                break;
        }
        return vec;
    }

    public static Vector2 GetRealDir(TileDirection dir)
    {
        Vector2 vec = Vector2.zero;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                vec = new Vector2(1, 1);
                break;
            case TileDirection.LEFTDOWN:
                vec = new Vector2(-1, -1);
                break;
            case TileDirection.LEFTUP:
                vec = new Vector2(-1, 1);
                break;
            case TileDirection.RIGHTDOWN:
                vec = new Vector2(1, -1);
                break;
        }
        return vec;
    }

    public void UpdateState(eUpdateState state)
    {
        foreach (var item in managers)
        {
            item.UpdateState(state);
        }
    }

    public T GetManager<T>() where T : ManagerBase
    {
        var value = default(T);

        foreach (var component in managers.OfType<T>())
            value = component;

        return value;
    }
}
