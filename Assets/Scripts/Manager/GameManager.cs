using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase
{
    public List<Goal> goalList = new List<Goal>();
    public List<Teleporter> portalList = new List<Teleporter>();

    public List<Ball> lastBallList = new List<Ball>();
    public List<Ball> myBallList = new List<Ball>(); // 사용 가능한 공들
    public List<Ball> aliveBallList = new List<Ball>(); // 쏘아진 공들

    public List<GameObject> ballUIList = new List<GameObject>(); // 삭제시킬 UI 리스트?

    public Dictionary<Vector2, ObjectTile> tileDict = new Dictionary<Vector2, ObjectTile>();

    public int checkedFlags = 0;
    [HideInInspector] public int maxBallCount;
    [HideInInspector] public bool isPlayStarted = false;
    public bool isFirstBallNotArrived = true;

    public float limitTime = 2f;
    public float firstTime = 0f;
    private float realTime;

    public Action<bool> ActiveGameOverPanel = null;
    public Action<string, Color?> SetTimerText;
    public Action<int> MakeNewStageBtn;
    public Action<Ball, bool> MakeNewBallUI;

    [HideInInspector] public IEnumerator timerCo;


    public override void Init()
    {
        realTime = 0;

        ObjectTile tile = Resources.Load<ObjectTile>("Tiles/Arrow1");
        PoolManager.Instance.CreatePool(tile, "DirectionChanger", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Flag");
        PoolManager.Instance.CreatePool(tile, "Goal", 10);

        tile = Resources.Load<ObjectTile>("Tiles/JumpPad");
        PoolManager.Instance.CreatePool(tile, "JumpPad", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Portal_Hole");
        PoolManager.Instance.CreatePool(tile, "Teleporter", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Slow");
        PoolManager.Instance.CreatePool(tile, "Slow", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Wall1");
        PoolManager.Instance.CreatePool(tile, "Reflect", 10);

        tile = Resources.Load<ObjectTile>("Tiles/None");
        PoolManager.Instance.CreatePool(tile, "None", 10);

        tile = Resources.Load<ObjectTile>("Tiles/ColorChanger");
        PoolManager.Instance.CreatePool(tile, "ColorChanger", 10);

        tile = Resources.Load<ObjectTile>("Tiles/ColorFlag");
        PoolManager.Instance.CreatePool(tile, "ColorGoal", 10);

        tile = Resources.Load<ObjectTile>("Tiles/Thorn");
        PoolManager.Instance.CreatePool(tile, "Thon", 10);

	    tile = Resources.Load<ObjectTile>("Tiles/Line");
        PoolManager.Instance.CreatePool(tile, "Line", 10);

        Ball ball = Resources.Load<Ball>("Balls/DefaultBall");
        PoolManager.Instance.CreatePool(ball, null, 5);

        BallDestryParticle pMono = Resources.Load<BallDestryParticle>("Particles/BallDestroyParticle");
        PoolManager.Instance.CreatePool(pMono, null, 10);

    }

    
    public void ResetData()
    {
        ballUIList.Clear();
        myBallList.Clear();
        aliveBallList.Clear();

        SetTimerText("Ready", Color.black);
        realTime = 0f;
        firstTime = 0f;
        isFirstBallNotArrived = true;
        timerCo = Timer();
    }

    public void CheckFail() 
    {
        if(myBallList.Count == 0 && aliveBallList.Count == 0 && goalList.FindAll(goal => !goal.isChecked).Count > 0)
        {
            StopTimer(); // 리셋 먼저하면 timerCo가 가리키는 포인터가 달라지는 듯?
            ActiveGameOverPanel(false);
        }
    }

    public void CheckClear()
    {
        if (isFirstBallNotArrived)
        {
            isFirstBallNotArrived = false;
            firstTime = Time.time;
            StartCoroutine(timerCo);
        }
        checkedFlags++;

        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if (list.Count == 0 && firstTime + limitTime >= Time.time)
        {
            StopTimer();
            SetTimerText("Clear", Color.green);

            StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
            if(sm.stageIndex -1 == sm.clearMapCount) // 맨 마지막걸 깨야  다음거 열어줘야 하니까!
            {
                sm.clearMapCount++;
                PlayerPrefs.SetInt("ClearMapsCount", sm.clearMapCount);
                
                if(sm.clearMapCount % 3 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        MakeNewStageBtn(i + sm.stageIndex);
                    }
                }
            }
            ActiveGameOverPanel(true);
        }
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            if (limitTime - realTime <= 0)
            {
                goalList.ForEach((x) => x.ResetFlag(false));
                StopTimer();
                SetTimerText("Failed", Color.red);
                break;
            }

            yield return null;
            realTime += Time.deltaTime;
            SetTimerText(string.Format("{0:0.00}", limitTime - realTime <= 0 ? "0:00" : limitTime - realTime), Color.black);
        }
    }

    public void StopTimer() => StopCoroutine(timerCo);

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
