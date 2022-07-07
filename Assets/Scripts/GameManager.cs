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

    private void Awake()
    {
        instance = this;
    }



    public void CheckClear()
    {
        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if(list.Count <= 0)
        {
            print("Å¬¸®¾î");
        }
    }
}
