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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            if (isChecked)
                return;

            tb.rigid.velocity = Vector3.zero;
            tb.gameObject.SetActive(false);
            //sr.sprite = changeSprite;
            isChecked = true;
            IsometricManager.Instance.GetManager<GameManager>().CheckClear();
        }
    }

    public void ResetFlag()
    {
        //sr.sprite = mySprite;
        isChecked = false;
    }

}
