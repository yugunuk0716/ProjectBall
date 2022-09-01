using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Goal : ObjectTile
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator animator;

    public bool isChecked;

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + "}";
    }

    public override void SettingTile(string info)
    {
        base.SettingTile(info);
        info = info.Substring(1, info.Length - 2);
        ObjectTileInfo goalInfo = JsonUtility.FromJson<ObjectTileInfo>(info);
        myType = (TileType)goalInfo.tileType;
    }

    public void ResetFlag(bool isClear)
    {
        isChecked = isClear;
        animator.SetBool("isClear", isClear);
    }

    public override void InteractionTile(Ball tb)
    {
        PoolManager.Instance.Push(tb);
        Debug.Log("공 제거");
        if (false == isChecked)
        {
            ResetFlag(true);
            IsometricManager.Instance.GetManager<GameManager>().CheckClear();
        }
    }

        

    public override void Reset()
    {
        ResetFlag(false);
        StopCoroutine("Transition");
    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
