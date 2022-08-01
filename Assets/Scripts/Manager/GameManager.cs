using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : ManagerBase
{
    public List<Goal> goalList = new List<Goal>();
    public List<Teleporter> portalList = new List<Teleporter>();
    public List<Ball> myBallList = new List<Ball>();
    public List<GameObject> ballUIList = new List<GameObject>(); // 삭제시킬 UI 리스트?

    [HideInInspector] public int maxBallCount;

    public float limitTime = 2f;
    public float firstTime = 0f;
    private float realTime;

    public ShooterTile shooter = null;
    public Action<string, Color?> SetTimerText;
    private IEnumerator timerCo;


    public override void Init()
    {
        realTime = 0;

        timerCo = Timer();


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

    public void ResetGameData()
    {
        shooter.curAmmoCount = 0;
    }

    public void CheckClear()
    {
        if (firstTime == 0f)
        {
            firstTime = Time.time;
            StartCoroutine(timerCo);
            SetTimerText("Ready", Color.red);
        }

        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if (list.Count <= 0 && firstTime + limitTime >= Time.time)
        {
            StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
            sm.stageIndex++;
            sm.LoadStage();
            sm.ClearAllBalls();
            firstTime = 0f;
            realTime = 0f;
            StopCoroutine(timerCo);
            SetTimerText("Clear", Color.green);
        }

    }

    public IEnumerator Timer()
    {
        while (true)
        {
            yield return null;
            realTime += Time.deltaTime;
            if (limitTime - realTime <= 0)
            {
                foreach (Goal goal in goalList)
                {
                    goal.ResetFlag();
                }
                firstTime = 0f;
                realTime = 0f;
                StopCoroutine(timerCo);

                SetTimerText("Reset", Color.red);
                yield return new WaitForSeconds(0.2f);
                SetTimerText("Ready", Color.white);
            }
            SetTimerText(string.Format("{0:0.00}", limitTime - realTime <= 0 ? "0:00" : limitTime - realTime), Color.black);
        }
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
}
