using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

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

    private List<(float, float)> ratioPairList = new List<(float, float)>();

    public UnityEvent<int> AddHearts;

    private void Awake()
    {
        instance = this;

        float a = (float)Screen.height / Screen.width;


        ratioPairList.Add((19f / 9f, 10f));
        ratioPairList.Add((4f / 3f, 6.5f));
        ratioPairList.Add((6f / 5f, 6f));
        ratioPairList.Add((16f / 10f, 8f));
        ratioPairList.Add((16f / 9f, 8.5f));
        ratioPairList.Add((18.5f / 9f, 10f));
        ratioPairList.Add((19f / 9f, 10f));
        ratioPairList.Add((19.5f / 9f, 10f));
        ratioPairList.Add((20f / 9f, 10.5f));
        ratioPairList.Add((21f / 9f, 11f));
        ratioPairList.Add((23.1f / 9f, 12f));



        float minDif = float.MaxValue;
        float minValue = 0f;

        for (int i = 0; i < ratioPairList.Count; i++)
        {
            float dif = Mathf.Abs(ratioPairList[i].Item1 - a);
            if (dif < minDif)
            {
                minDif = dif;
                minValue = ratioPairList[i].Item1;
            }
        }
        cvCam.m_Lens.OrthographicSize = ratioPairList.Find(x => x.Item1.Equals(minValue)).Item2;



        managers.Add(GetComponent<UIManager>());

        managers.Add(gameObject.AddComponent<GameManager>());
        managers.Add(gameObject.AddComponent<StageManager>());
        managers.Add(gameObject.AddComponent<SaveManager>());
        managers.Add(gameObject.AddComponent<SoundManager>());
        managers.Add(gameObject.AddComponent<TutorialManager>());
        managers.Add(gameObject.AddComponent<LifeManager>());

        Application.targetFrameRate = 300;

        PlayerPrefs.SetInt("IsFirst", PlayerPrefs.GetInt("IsFirst",1));

        CanvasGroup tutoCanvas = GetManager<UIManager>().canvas[3].GetComponent<CanvasGroup>();

        if (PlayerPrefs.GetInt("IsFirst") == 1)
        { 
            FirstCall();
            tutoCanvas.interactable = true;
            tutoCanvas.blocksRaycasts = true;
        }
        else
        {
            
            tutoCanvas.interactable = false;
            tutoCanvas.blocksRaycasts = false; 
        }



     
        UpdateState(eUpdateState.Init);
    }

    public void Start()
    {
        UpdateState(eUpdateState.Start);
    }

    /* public void Update()
     {
         Debug.Log(Time.timeScale);
     }
 */

    private void FirstCall()
    {
        PlayerPrefs.SetInt("ClearMapsCount", 65);
        PlayerPrefs.SetInt("IsFirst", 0);
        GetManager<UIManager>().canvas[0].GetComponent<TitleUI>().
            titleBtns[1].onClick.AddListener(() =>
            {
                StartCoroutine(GetManager<TutorialManager>().StartTurotial());
            });
        PlayerPrefs.Save();
    }

    public static Vector2 GetIsoDir(TileDirection dir) 
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
