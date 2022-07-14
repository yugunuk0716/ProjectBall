using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public float limitTime = 2f;
    public float firstTime = 0f;

    public TextMeshProUGUI timer_text;

    private float realTime;
    private IEnumerator timerCo;

    private void Awake()
    {
        instance = this;
        realTime = 0;
    }

    public void Start()
    {
        timerCo = Timer();

        Ball ball = Resources.Load<Ball>("Ball");
        PoolManager.Instance.CreatePool(ball, null, 10);
    }

    public void CheckClear()
    {
        
        if (firstTime == 0f)
        {
            firstTime = Time.time;
            StartCoroutine(timerCo);
            timer_text.text = "Ready";
            timer_text.color = Color.white;
        }

        List<Goal> list = goalList.FindAll(goal => !goal.isChecked);

        if(list.Count <= 0 && firstTime + limitTime >= Time.time)
        {
            StageManager.instance.stageIndex++;
            StageManager.instance.LoadStage();
            print("클리어");
            print(Time.time - firstTime);
            firstTime = 0f;
            realTime = 0f;
            StopCoroutine(timerCo);
            timer_text.text = "Clear";
            timer_text.color = Color.green;
        }
       
    }

    public IEnumerator Timer()
    {
        while(true)
        {
            yield return null;
            realTime += Time.deltaTime;
            if(limitTime - realTime <= 0)
            {
                print("실패");
                print(Time.time - firstTime);
                foreach (Goal goal in goalList)
                {
                    goal.ResetFlag();
                }
                firstTime = 0f;
                realTime = 0f;
                StopCoroutine(timerCo);
                timer_text.text = "Reset";
                timer_text.color = Color.red;
                yield return new WaitForSeconds(0.2f);
                timer_text.color = Color.white;
                timer_text.text = "Ready";
            }
            timer_text.text = string.Format("{0:0.00}", limitTime - realTime <= 0 ? "0:00" : limitTime - realTime);
        }
    }
}
