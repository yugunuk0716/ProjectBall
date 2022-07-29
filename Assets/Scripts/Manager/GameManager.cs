using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : ManagerBase
{
    public List<Goal> goalList = new List<Goal>();
    public List<Teleporter> portalList = new List<Teleporter>();
    public List<Ball> myBallList = new List<Ball>();


    public float limitTime = 2f;
    public float firstTime = 0f;
    private float realTime;

    public ShooterTile shooter = null; 
    public Action<string,Color?> SetTimerText;
    private IEnumerator timerCo;


    public override void Init()
    {
        realTime = 0;

        timerCo = Timer();

        Ball ball = Resources.Load<Ball>("Ball");
        PoolManager.Instance.CreatePool(ball, null, 25);
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

        if(list.Count <= 0 && firstTime + limitTime >= Time.time)
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
        while(true)
        {
            yield return null;
            realTime += Time.deltaTime;
            if(limitTime - realTime <= 0)
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
        switch(state)
        {
            case eUpdateState.Init:
                Init();
                break;
        }
    }
}
