using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : ManagerBase
{
    public static bool canInteract = true;

    public List<Goal> goalList = new List<Goal>();
    public List<Teleporter> portalList = new List<Teleporter>();
    public List<ButtonTile> buttonTileList = new List<ButtonTile>();

    public List<TileDirection> lastBallList  = new List<TileDirection>();
    public List<BallControllUI> ballUIList = new List<BallControllUI>(); 
    public List<Ball> usableBallList = new List<Ball>();

    public Dictionary<Vector2, ObjectTile> tileDict = new Dictionary<Vector2, ObjectTile>();

    public ParticleSystem clearParticle_Left;
    public ParticleSystem clearParticle_Right;

    public int checkedFlags = 0;
    [HideInInspector] public int maxBallCount;
    [HideInInspector] public int curShootBallCount;
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
    public Action<int,float> OnClear;
    public Action Shoot;
    public Action UpdateUIContents;
    public Action TakeMapLoadVideo;

    [HideInInspector] public IEnumerator timerCo;

    public override void Init()
    {
        realTime = 0;
        
        clearParticle_Left = Instantiate(Resources.Load<ParticleSystem>("Effects/LeftParticle"));
        clearParticle_Right = Instantiate(Resources.Load<ParticleSystem>("Effects/RightParticle"));

        CloudHandler Cloud = Instantiate(Resources.Load<CloudHandler>("Objects/CloudHandler"));
        StartCoroutine(Cloud.CloudMove());
    }

    public override void Load()
    {
        curShootBallCount = 0;
    }

    public void ResetData(StageDataSO stageData, bool isSameStageLoaded)
    {
        ballUIList.ForEach((x) => x.SetDisable());
        ballUIList.Clear();

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

        canInteract = true;
    }

    public void CheckFail() 
    {
        if(ballUIList.Count == 0 && goalList.FindAll(goal => !goal.isChecked).Count > 0)
        {
            StopGame(); 
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
        usableBallList.Clear();

        for (int i = 0; i < ballCount; i++)
        {
            Ball ball = GameObjectPoolManager.Instance.GetGameObject($"Balls/DefaultBall", GameObjectPoolManager.Instance.transform).GetComponent<Ball>();
            if (isSameStageLoaded && lastBallList.Count >= ballCount)
            {
                ball.shootDir = lastBallList[i];
                MakeNewBallUI(ball, true, i);
            }
            else
            {
                MakeNewBallUI(ball, false, i);
            }
            usableBallList.Add(ball);
        }
    }

    public void CheckClear()
    {
        if (isFirstBallNotArrived)
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
            StopGame();
            float clearTime = limitTime - realTime;

            StageManager sm = IsometricManager.Instance.GetManager<StageManager>();
            int star = sm.CalcStar(clearTime);
            sm.SaveStar(sm.stageIndex - 1, star); 

            if(sm.stageIndex - 1 == sm.clearMapCount)
            {
                sm.clearMapCount++;
                PlayerPrefs.SetInt("ClearMapsCount", sm.clearMapCount);
            }
            ActiveGameOverPanel(true);
            OnClear(star, clearTime);
        }
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            if (limitTime < realTime)
            {
                goalList.ForEach((x) => x.ResetFlag(false));
                StopGame();
                IsometricManager.Instance.GetManager<GameManager>().usableBallList.ForEach(x => x.SetDisable());
                ActiveGameOverPanel(false);
                break;
            }

            yield return null;
            realTime += Time.deltaTime;
            SetTimerText(string.Format("{0:0.00}", limitTime < realTime ? "0:00" : limitTime - realTime), Color.white);
            SetTimerSize(limitTime - realTime);
        }
    }

    public void StopGame()
    {
        if(timerCo != null)
        {
            StopCoroutine(timerCo);
        }
        SetTimerText("off", Color.white);
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

[System.Serializable]
public class Mapinfo
{
    public string range;
    public string sheet;
}
