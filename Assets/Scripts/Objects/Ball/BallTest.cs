using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallTest : MonoBehaviour
{

    public Dictionary<Vector2Int, ObjectTile> dummy;

    public Vector2Int direction;
    public Vector2Int myPos;
    public float speed = 1f;

    void Start()
    {
        SetMove(speed);
    }

    void Update()
    {
        
    }


    public void SetDirction(Vector2Int dir, float speed, Vector2Int pos)
    {
        direction = dir;
        this.speed = speed;
        myPos = pos;
    }

    //추후에 매개변수로 objectTile을 받아서 트윈이 끝났을 때 그 타일의 상호작용을 실행하게끔 만들죠
    public void SetMove(float speed)
    {
        myPos += direction;
        ObjectTile tile = dummy[myPos];
        transform.DOMove(tile.transform.position, speed).SetEase(Ease.Linear).OnComplete(() => tile.InteractionTile());
    }


}
