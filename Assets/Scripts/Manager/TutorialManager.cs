using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : ManagerBase
{
    //ManagerBase 구현하기
    public override void Init()
    {
        if (PlayerPrefs.GetInt("IsFirst") != 1)
        {
            PlayerPrefs.SetInt("IsFirst", 0);
            StartTurotial();
        }
        else
        {
            return;
        }
    }

    public override void Load()
    {
    }

    public override void UpdateState(eUpdateState state)
    {
    }

    public void StartTurotial()
    {
        
    }
}
