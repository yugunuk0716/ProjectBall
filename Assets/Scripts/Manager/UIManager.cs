using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : ManagerBase
{
    [SerializeField] List<UIBase> uis;


    public override void Init()
    {
        foreach(var item in uis)
        {
            item.Init();
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
