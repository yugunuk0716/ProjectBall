using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class UIManager : ManagerBase
{
    
    public List<UIBase> uis;

    public List<CanvasGroup> canvas = new List<CanvasGroup>();




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

    public UIBase FindUI(string name)
    {
        return uis.Find(x => x.name.Equals(name));
    }


    








}
