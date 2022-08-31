using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase
{
    public List<Goal> goalList = new List<Goal>();
    public List<Teleporter> portalList = new List<Teleporter>();
    public List<Ball> myBallList = new List<Ball>();
    public List<GameObject> ballUIList = new List<GameObject>(); // 삭제시킬 UI 리스트?

    public Dictionary<Vector2, ObjectTile> tileDict = new Dictionary<Vector2, ObjectTile>();

    [HideInInspector] public int maxBallCount;

    [HideInInspector] public bool isPlayStarted = false;
    public bool isFirstBallNotArrived = true;

    public float limitTime = 2f;
    public float firstTime = 0f;
    private float realTime;

    public Action<string, Color?> SetTimerText;
    [HideInInspector] public IEnumerator timerCo;

    public string[] mapRangeStrArray =
    {
        "A10:I18",
        "L10:T18",
        "W10:AE18",
        "AH10:AP18",

        "A21:I29",
        "L21:T29",
        "W21:AE29",
        "AH21:AP29",

        "A33:I41",
        "L33:T41",
        "W33:AE41",
        "AH33:AP41",

        "A44:I52",
        "L44:T52",
        "W44:AE52",
        "AH44:AP52",
        "A55:I63",
        "L55:T63",
        "W55:AE63",
        "AH55:AP63",
    };


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

        Ball ball = Resources.Load<Ball>("Balls/DefaultBall");
        PoolManager.Instance.CreatePool(ball, null, 5);

        for (int i = 0; i < Enum.GetNames(typeof(TileType)).Length - 1; i++) // None은 취급안하려구
        {
            ball = Resources.Load<Ball>($"Balls/Destroy{(TileType)i}");
            if(ball != null)
                PoolManager.Instance.CreatePool(ball, null, 5);

                ball = Resources.Load<Ball>($"Balls/Ignore{(TileType)i}");
            if (ball != null)
                PoolManager.Instance.CreatePool(ball, null, 5);
        }
    }

    public void ResetData()
    {
        ballUIList.Clear();
        myBallList.Clear();
        SetTimerText("Ready", Color.black);
        realTime = 0f;
        firstTime = 0f;
        isFirstBallNotArrived = true;
        timerCo = Timer();
    }

    public void CheckClear()
    {
        if (isFirstBallNotArrived)
        {
            isFirstBallNotArrived = false;
            firstTime = Time.time;
            StartCoroutine(timerCo);
        }

        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if (list.Count == 0 && firstTime + limitTime >= Time.time)
        {
            StopTimer();

            StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
            sm.stageIndex++;
            SetTimerText("Clear", Color.green);
            sm.LoadStage(mapRangeStrArray[sm.stageIndex-1]);
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
                yield return new WaitForSeconds(1f);
                SetTimerText("Press the 'Load' to Play", Color.white);

                break;
            }

            yield return null;
            realTime += Time.deltaTime;
            SetTimerText(string.Format("{0:0.00}", limitTime - realTime <= 0 ? "0:00" : limitTime - realTime), Color.black);
        }
    }

    public void StopTimer()
    {
         StopCoroutine(timerCo);
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
