using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ESheet
{
   Default = 80333382,
   Color = 2065586561,
   Time = 1585233606,
   Order  = 1872519807
}

[CreateAssetMenu(menuName = "SO/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public Ball[] balls;
    public float countDown;
    public TileDirection shooterDirections;
    public string range;
    public ESheet eSheet;

}
