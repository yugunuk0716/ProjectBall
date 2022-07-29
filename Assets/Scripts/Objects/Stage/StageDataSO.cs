using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public int ballCount;
    public Ball[] balls;
    public float countDown;
    public TileDirection shooterDirection;
    public string jsonString;

}
