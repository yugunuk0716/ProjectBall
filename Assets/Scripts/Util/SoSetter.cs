using UnityEngine;

public class SoSetter : MonoBehaviour
{
    [SerializeField] private StageDataSO[] stageDatas;

    public void SetStageData_Base(ESheet targetSheet)
    {
        foreach(var stageData in stageDatas)
        {
            stageData.range = "";
            stageData.eSheet = targetSheet;

        }
    }

}