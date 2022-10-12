using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeManager : ManagerBase
{
    private DateTime lastTime;
    private readonly int coolTime = 300;
    private int currentTime = 300;
    int min = 0;
    int sec = 0;

    private StageScrollUI ssUI;
    private TitleSettingUI tsUI;
    private int heartCount = 5;

    private void Start()
    {
        Debug.Log("Start, Update 전부 지우기");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            heartCount = 5;
        }
    }

    public override void Init()
    {
        heartCount = PlayerPrefs.GetInt("heartCount", 5);
        string lastTimeStr = PlayerPrefs.GetString("startTime", DateTime.Now.ToString());
        lastTime = Convert.ToDateTime(lastTimeStr);

        DateTime curTime = DateTime.Now;
        TimeSpan timeDif = curTime - lastTime;
       
        int totalSec  = (int)timeDif.TotalSeconds;
        ssUI = IsometricManager.Instance.GetManager<UIManager>().FindUI("StageNumberPanel").GetComponent<StageScrollUI>();
        tsUI = IsometricManager.Instance.GetManager<UIManager>().FindUI("TitleSettingPopUp").GetComponent<TitleSettingUI>();
        int plusHeartCount = totalSec / coolTime;
        heartCount = Mathf.Clamp(heartCount + plusHeartCount, 0, 5);

        StartCoroutine(HeartCoolRoutine());

    }

    public bool CanEnterStage()
    {
        return true;
        //return heartCount > 0;
    }

    public void EnterStage()
    {
        heartCount--;
        if (ssUI != null && tsUI != null)
        {
            ssUI.UpdateHeartText(heartCount, $"{min}:{sec}");
            tsUI.UpdateHeartText(heartCount, $"{min}:{sec}");
        }
    }

    public override void Load()
    {
        
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


    IEnumerator HeartCoolRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            if(heartCount != 5)
            {
                currentTime -= 1;
             
            }

            min = currentTime / 60;
            sec = (currentTime - min * 60) % 60;

            if (min == 0 && sec == 0)
            {
                ResetTimer();
            }

            if (ssUI != null && tsUI != null)
            {
                ssUI.UpdateHeartText(heartCount, $"{min}:{sec}");
                tsUI.UpdateHeartText(heartCount, $"{min}:{sec}");
            }
        }
    }

    public void ResetTimer()
    {
        lastTime = DateTime.Now;
        currentTime = coolTime;
        heartCount++;
        PlayerPrefs.SetString("startTime", lastTime.ToString());
        PlayerPrefs.SetInt("heartCount", heartCount);
    }

    private void OnApplicationQuit()
    {
        lastTime = DateTime.Now;
        PlayerPrefs.SetString("startTime", lastTime.ToString());
        PlayerPrefs.SetInt("heartCount", heartCount);
    }
                               
}
