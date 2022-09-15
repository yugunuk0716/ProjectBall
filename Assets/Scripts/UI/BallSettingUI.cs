using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSettingUI : UIBase
{
    [Space(10)]
    [Header("Content")]
    public Transform ballContent;
    [SerializeField] private Transform targetPointContent;

    int order = 0;

    [Space(10)]
    [Header("Prefab")]
    [SerializeField] BallControllUI ballControllUIPrefab;
    [SerializeField] GameObject targetPointObjPrefab;

    [SerializeField] private SelectDirectionUI selectDirectionUI;

    public override void Init()
    {
        GameManager gm = IsometricManager.Instance.GetManager<GameManager>();

        gm.MakeNewBallUI += (ball, isAutoSet) =>
        {
            BallControllUI newBallControllUI = Instantiate(ballControllUIPrefab, ballContent);
            newBallControllUI.SetBallSprites(ball.uiSprite);
            gm.ballUIList.Add(newBallControllUI);
            newBallControllUI.order = 1000; // 정렬 안되도록

            bool isAdded = false;

            if (isAutoSet)
            {
                order++;
                isAdded = true;
                gm.myBallList.Add(ball);
                newBallControllUI.SetDirection(ball.shootDir);
            }

            newBallControllUI.directionSetBtn.onClick.AddListener(() =>
            {
                if (isAdded) // 다시 돌아오려는
                {
                    order--;

                    newBallControllUI.order = 1000;
                    newBallControllUI.SetDirection(TileDirection.RIGHTDOWN, false);

                    gm.myBallList.Remove(ball);
                    gm.BallUiSort();
                }
                else // 추가 하려는
                {
                    order++;
                    newBallControllUI.order = order;
                    selectDirectionUI.Set(ball, newBallControllUI, order);
                    selectDirectionUI.ScreenOn(true);
                }

                isAdded = !isAdded;
            });
        };
    }

    public override void Load()
    {
        order = 0;
        
        MakeTargetPoints();

    }

    public void MakeTargetPoints()
    {
        for (int i = 0; i < Resources.Load<StageDataSO>($"Stage {IsometricManager.Instance.GetManager<StageManager>().stageIndex}").balls.Length; i++)
        {
            Instantiate(targetPointObjPrefab, targetPointContent);
        }
    }
}
