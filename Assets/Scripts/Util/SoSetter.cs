using UnityEngine;

public class SoSetter : MonoBehaviour
{
    [SerializeField] private StageDataSO[] stageDatas;

    public string[] arr =
    {
        "A", "I", "L", "T", "W", "AE", "AH", "AP"
    };


    public void SetStageData_Base(ESheet targetSheet)
    {
        int index = 0;

        foreach(var stageData in stageDatas)
        {
            stageData.range = $"{arr[index]} {arr[index + 1]}";
            stageData.eSheet = targetSheet;

            index += 2;
            if(index > arr.Length)
            {
                index = 0;
            }
        }
    }

}