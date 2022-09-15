using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootBallUI : UIBase
{
    [SerializeField] Button shootBtn;

    public override void Init()
    {
        shootBtn.onClick.AddListener(() => IsometricManager.Instance.GetManager<GameManager>().Shoot());
    }

    public override void Load()
    {
        
    }

}
