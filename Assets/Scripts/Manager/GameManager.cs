using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class GameManager : ManagerBase
{
    public static bool CanNotInteract = false;  // 아무것도 못함 그냥!

    public List<Goal> goalList = new List<Goal>();
    public List<Teleporter> portalList = new List<Teleporter>();
    public List<ButtonTile> buttonTileList = new List<ButtonTile>();

    /*[HideInInspector]*/ public List<Ball> lastBallList  = new List<Ball>();
    /*[HideInInspector]*/ public List<Ball> myBallList    = new List<Ball>(); // 사용 가능한 공들
    /*[HideInInspector]*/ public List<Ball> aliveBallList = new List<Ball>(); // 쏘아진 공들
    /*[HideInInspector]*/ public List<BallControllUI> ballUIList = new List<BallControllUI>(); // 삭제시킬 UI 리스트?

    public Dictionary<Vector2, ObjectTile> tileDict = new Dictionary<Vector2, ObjectTile>();

    public ParticleSystem clearParticle_Left;
    public ParticleSystem clearParticle_Right;

    public int checkedFlags = 0;
    [HideInInspector] public int maxBallCount;
    [HideInInspector] public int curSetBallCount;
    [HideInInspector] public bool isShooting = false;
    public bool isFirstBallNotArrived = true;

    public float limitTime = 2f;
    public float firstTime = 0f;
    private float realTime;

    public Action<bool> ActiveGameOverPanel = null;
    public Action<string, Color?> SetTimerText;
    public Action<float> SetTimerSize;
    public Action<int> SetStageText;
    public Action<Ball, bool, int> MakeNewBallUI;
    public Action<int> OnClear;
    public Action Shoot;
    public Action UpdateUIContents;
    public Action TakeMapLoadVideo;

    [HideInInspector] public IEnumerator timerCo;
    

    public override void Init()
    {
        realTime = 0;

        PoolingObjectInit();
        
        clearParticle_Left = Instantiate(Resources.Load<ParticleSystem>("Effects/LeftParticle"));
        clearParticle_Right = Instantiate(Resources.Load<ParticleSystem>("Effects/RightParticle"));

        CloudHandler Cloud = Instantiate(Resources.Load<CloudHandler>("Objects/CloudHandler"));
        StartCoroutine(Cloud.CloudMove());
    }


    public void ResetData(StageDataSO stageData, bool isSameStageLoaded)
    {
        ballUIList.Clear();
        myBallList.Clear();
        aliveBallList.Clear();

       
        firstTime = 0f;
        realTime = 0f;
        curSetBallCount = 0;

        isFirstBallNotArrived = true;
        isShooting = false;
        timerCo = Timer();

        SaveManager sm = IsometricManager.Instance.GetManager<SaveManager>();

        goalList = sm.mainMap.GetComponentsInChildren<Goal>().ToList();
        goalList.ForEach(x => x.ResetFlag(false));

        portalList = sm.mainMap.GetComponentsInChildren<Teleporter>().ToList();
        portalList.ForEach(portal => portal.FindPair());

        buttonTileList = sm.mainMap.GetComponentsInChildren<ButtonTile>().ToList();
        buttonTileList.ForEach(btn => btn.FindTarget());

        limitTime = stageData.countDown;
        maxBallCount = stageData.balls.Length;

        SetBallUI(stageData.balls.Length, isSameStageLoaded);
    }

    public void CheckFail() 
    {
        if(myBallList.Count == 0 && aliveBallList.Count == 0 && goalList.FindAll(goal => !goal.isChecked).Count > 0)
        {
            StopTimer(); // 리셋 먼저하면 timerCo가 가리키는 포인터가 달라지는 듯?
            SetTimerText("off", Color.white);
            ActiveGameOverPanel(false);
        }
    }

    public void BallUiSort()
    {
        ballUIList.Sort((x, y) => x.order.CompareTo(y.order));
        for(int i = 0; i < ballUIList.Count; i++)
        {
            ballUIList[i].order = i;
        }
    }

    public void SetBallUI(int ballCount, bool isSameStageLoaded)
    {
        if (isSameStageLoaded && lastBallList.Count >= ballCount)
        {
            for (int i = 0; i < ballCount; i++)
            {
                MakeNewBallUI(lastBallList[i], true, i);
            }
            lastBallList = lastBallList.GetRange(0, ballCount);
        }
        else
        {
            for (int i = 0; i < ballCount; i++)
            {
                Ball ball = PoolManager.Instance.Pop($"DefaultBall") as Ball;
                MakeNewBallUI(ball, false, i);
            }
        }
    }

    public void CheckClear()
    {
        if (isFirstBallNotArrived || myBallList.Count != 0)
        {
            isFirstBallNotArrived = false;
            firstTime = Time.time;
            StartCoroutine(timerCo);
            SetTimerText("on", Color.white);
        }
        checkedFlags++;

        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if (list.Count == 0 && limitTime >= realTime)
        {
            Vibration.Vibrate(500);
            
            UpdateUIContents?.Invoke();
            StopTimer();
            float clearTime = limitTime - realTime;
            SetTimerText("off", Color.white);

            StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
            int star = sm.CalcStar(clearTime);
            sm.SaveStar(sm.stageIndex - 1, star); 

            if(sm.stageIndex - 1 == sm.clearMapCount) // 맨 마지막걸 깨야  다음거 열어줘야 하니까!
            {
                sm.clearMapCount++;
                PlayerPrefs.SetInt("ClearMapsCount", sm.clearMapCount);
            }
            ActiveGameOverPanel(true);
            OnClear?.Invoke(star);
        }
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            if (limitTime < realTime)
            {
                goalList.ForEach((x) => x.ResetFlag(false));
                StopTimer();
                SetTimerText("off", Color.white);
                break;
            }

            yield return null;
            realTime += Time.deltaTime;
            SetTimerText(string.Format("{0:0.00}", limitTime < realTime ? "0:00" : limitTime - realTime), Color.white);
            SetTimerSize(limitTime - realTime);
        }
    }

    public void StopTimer() => StopCoroutine(timerCo);

    private void PoolingObjectInit()
    {
        ObjectTile tile = Resources.Load<ObjectTile>("Tiles/Arrow1");
        PoolManager.Instance.CreatePool(tile, "DirectionChanger", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Flag");
        PoolManager.Instance.CreatePool(tile, "Goal", 10);

        tile = Resources.Load<ObjectTile>("Tiles/JumpPad");
        PoolManager.Instance.CreatePool(tile, "JumpPad", 1);

        tile = Resources.Load<ObjectTile>("Tiles/Portal_Hole");
        PoolManager.Instance.CreatePool(tile, "Teleporter", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Slow");
        PoolManager.Instance.CreatePool(tile, "Slow", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Wall1");
        PoolManager.Instance.CreatePool(tile, "Reflect", 10);

        tile = Resources.Load<ObjectTile>("Tiles/None");
        PoolManager.Instance.CreatePool(tile, "None", 10);


        tile = Resources.Load<ObjectTile>("Tiles/Thorn");
        PoolManager.Instance.CreatePool(tile, "Thon", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Line");
        PoolManager.Instance.CreatePool(tile, "Line", 10);

        tile = Resources.Load<ObjectTile>("Tiles/BtnTile");
        PoolManager.Instance.CreatePool(tile, "ButtonTile", 10);

        Cloud cloud = Resources.Load<Cloud>("Objects/Cloud");
        PoolManager.Instance.CreatePool(cloud, "Cloud", 10);
        
        Ball ball = Resources.Load<Ball>("Balls/DefaultBall");
        PoolManager.Instance.CreatePool(ball, null, 5);

        BallDestryParticle pMono = Resources.Load<BallDestryParticle>("Effects/BallDestroyParticle");
        PoolManager.Instance.CreatePool(pMono, null, 10);

        BallControllUI ballControll = Resources.Load<BallControllUI>("UIs/BallControllUI");
        PoolManager.Instance.CreatePool(ballControll, null, 10);

        TargetPointUI targetPointUI = Resources.Load<TargetPointUI>("UIs/TargetPointUI");
        PoolManager.Instance.CreatePool(targetPointUI, null, 10);
    }

    public override void UpdateState(eUpdateState state)
    {
        switch (state)
        {
            case eUpdateState.Init:
                Init();
                break;
        }
    }

    public override void Load()
    {
        
    }
}

[System.Serializable]
public class Mapinfo
{
    public string range;
    public string sheet;
}
