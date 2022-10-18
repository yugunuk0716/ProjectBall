using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeManager : ManagerBase
{
    private DateTime lastTime;

    int standard = 300;
    private int coolTime = 0;
    private int currentTime = 0;
    int min = 0;
    int sec = 0;

    public bool isADSkip = false;

    private StageScrollUI ssUI;
    private TitleSettingUI tsUI;
    private HeartProvideUI hpUIl;
    private HeartProvideUI naUI;
    public int heartCount = 5;
    private UIManager um;
    private GameManager gm;

    public override void Init()
    {
        coolTime = standard;
        currentTime = standard;

        um = IsometricManager.Instance.GetManager<UIManager>();
        ssUI = um.FindUI("StageNumberPanel").GetComponent<StageScrollUI>();
        tsUI = um.FindUI("TitleSettingPopUp").GetComponent<TitleSettingUI>();
        hpUIl = um.FindUI("RewardSuppliedPanel").GetComponent<HeartProvideUI>();
        naUI = um.FindUI("NoAnyAdPanel").GetComponent<HeartProvideUI>();

        heartCount = PlayerPrefs.GetInt("heartCount", 5);

        Save();
        StartCoroutine(HeartCoolRoutine());
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            heartCount = 5;
        }
    }

    public void EnterStage()
    {
        if(gm == null)
        {
            gm = IsometricManager.Instance.GetManager<GameManager>();
        }

        DecreaseHeart(1);
        if (ssUI != null && tsUI != null)
        {
            ssUI.UpdateHeartText(heartCount, $"{min}:{sec}", isADSkip);
            tsUI.UpdateHeartText(heartCount, $"{min}:{sec}", isADSkip);
        }
        lastTime = DateTime.Now;
        PlayerPrefs.SetString("startTime", lastTime.ToString());
        GameManager.canInteract = true;
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

    private void CheckTimer()
    {
        if (heartCount != 5)
        {
            currentTime -= 1;
        }

        min = currentTime / 60;
        sec = (currentTime - min * 60) % 60;

        if (min <= 0 && sec <= 0)
        {
            ResetTimer();
        }

        if (ssUI != null && tsUI != null)
        {
            ssUI.UpdateHeartText(heartCount, $"{min}:{sec}", isADSkip);
            tsUI.UpdateHeartText(heartCount, $"{min}:{sec}", isADSkip);
        }
    }

    IEnumerator HeartCoolRoutine()
    {
        while (true)
        {
            CheckTimer();
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void ResetTimer()
    {
        lastTime = DateTime.Now;
        currentTime = coolTime;
        IncreaseHeart(1);
        PlayerPrefs.SetString("startTime", lastTime.ToString());
        PlayerPrefs.SetInt("heartCount", heartCount);
    }

    private void OnApplicationQuit()
    {
        lastTime = DateTime.Now;
        PlayerPrefs.SetString("startTime", lastTime.ToString());
        PlayerPrefs.SetInt("heartCount", heartCount);
        PlayerPrefs.SetInt("remainTime", currentTime);

    }

    bool isShouldInitDatas = true;
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            lastTime = DateTime.Now;

            PlayerPrefs.SetString("startTime", lastTime.ToString());
            PlayerPrefs.SetInt("remainTime", currentTime);
            isShouldInitDatas = true;
        }
        else
        {
            if(isShouldInitDatas)
            {
                Save();
                isShouldInitDatas = false;
            }
        }
    }

    private void Save()
    {
        string lastTimeStr = PlayerPrefs.GetString("startTime", DateTime.Now.ToString());
        lastTime = Convert.ToDateTime(lastTimeStr);

        DateTime curTime = DateTime.Now;
        TimeSpan timeDif = curTime - lastTime;

        int totalSec = (int)timeDif.TotalSeconds;
        int plusHeartCount = totalSec / coolTime;

        currentTime = PlayerPrefs.GetInt("remainTime", standard) - totalSec % coolTime;
        IncreaseHeart(plusHeartCount);
        CheckTimer();
    }

    private void OnHeartProvideUI()
    {
        hpUIl.ScreenOn(true);
    }

    private void OnNoAdvertiseUI()
    {
        naUI.ScreenOn(true);
    }

    public void IncreaseHeart(int amount)
    {
        heartCount = Mathf.Clamp(heartCount + amount, 0, 5);
    }

    public void DecreaseHeart(int amount)
    {
        heartCount = Mathf.Clamp(heartCount - amount, 0, 5);
    }


    public bool CanEnterStage()
    {
        return heartCount > 0;
    }
}
