using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance 
    {
        get
        { 
            return instance; 
        }
    }


    public List<Goal> goalList = new List<Goal>();

    public float limitTime = 1f;
    public float firstTime = 0f;

    private void Awake()
    {
        instance = this;
    }



    public void CheckClear()
    {
        if(firstTime == 0f)
        {
            firstTime = Time.time;
        }

        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if(list.Count <= 0 && firstTime + limitTime >= Time.time)
        {
            print("클리어");
            print(Time.time - firstTime);
            firstTime = 0f;
        }
        else if(list.Count <= 0)
        {
            print("실패");
            print(Time.time - firstTime );
            foreach (Goal goal in goalList)
            {
                goal.ResetFlag();
            }
            firstTime = 0f;

        }
    }
}
