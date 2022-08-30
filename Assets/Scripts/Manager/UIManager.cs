using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : ManagerBase
{
    [SerializeField] List<UIBase> uis;


    public override void Init() => uis.ForEach(x => x.Init());

    public override void Load() => uis.ForEach(x => x.Load());

    public override void UpdateState(eUpdateState state)
    {
        switch (state)
        {
            case eUpdateState.Init:
                Init();
                break;
            case eUpdateState.Load:
                Load();
                break;
        }
    }
}
