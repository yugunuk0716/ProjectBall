using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallTest : MonoBehaviour
{

    public Dictionary<Vector2Int, ObjectTile> dummy;

    public Vector2Int direction;
    public float speed = 1f;

    void Start()
    {
        SetMove(direction, speed);
    }

    void Update()
    {
        
    }

    //추후에 매개변수로 objectTile을 받아서 트윈이 끝났을 때 그 타일의 상호작용을 실행하게끔 만들죠
    public void SetMove(Vector2 targetPos, float speed)
    {
        transform.DOMove(targetPos, speed).SetEase(Ease.Linear);
    }


}
