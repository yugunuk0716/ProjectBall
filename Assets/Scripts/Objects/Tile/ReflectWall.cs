using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReflectWall : ObjectTile
{
    public bool isHorizontalWall = true;
    
    public Sprite[] horSprites; // 0 기본 1  오른쪽으로  2 왼쪽으로
    public Sprite[] verSprites; // 0 기본 1  위쪽으로    2 아래쪽으로
    private SpriteRenderer sr;

    WaitForSeconds changeTerm = new WaitForSeconds(0.15f);

    protected override void Awake()
    { 
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }


    IEnumerator ChangeSprite(Sprite[] targetSpriteArray, int targetSpriteIndex)
    {
        sr.sprite = targetSpriteArray[targetSpriteIndex];
        yield return changeTerm;
        sr.sprite = targetSpriteArray[0];
    }

    

    public override void InteractionTile(Ball tb)
    {
        StopCoroutine("ChangeSprite");
        if (isHorizontalWall)
        {
            switch(tb.direction)
            {
                case Vector2 v when v.Equals(Vector2.right):
                    tb.SetBall(Vector2.down, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.down):
                    tb.SetBall(Vector2.right, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.left):
                    tb.SetBall(Vector2.up, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.up):
                    tb.SetBall(Vector2.left, tb.speed);
                    break;
            }
        }
        else
        {
            switch (tb.direction)
            {
                case Vector2 v when v.Equals(Vector2.right):
                    tb.SetBall(Vector2.up, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.down):
                    tb.SetBall(Vector2.left, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.left):
                    tb.SetBall(Vector2.down, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.up):
                    tb.SetBall(Vector2.up, tb.speed);
                    break;
            }
        }
        

        tb.SetMove();
    }

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + "}";
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        base.SettingTile(info);
        ObjectTileInfo mirrorInfo = JsonUtility.FromJson<ObjectTileInfo>(info);
        myType = (TileType)mirrorInfo.tileType;
    }

    public override void SetDirection()
    {
        base.SetDirection();

        if (dataString.Equals("\\"))
        {
            isHorizontalWall = true;
            transform.Translate(0, -0.15f, 0);
            sr.sprite = horSprites[0];
        }
        else if (dataString.Equals("/"))
        {
            isHorizontalWall = false;
            sr.sprite = verSprites[0];
        }
    }

    public override void Reset()
    {
        StopCoroutine("Transition");
    }

    public override IEnumerator Transition()
    {
        while(true)
        {
            isHorizontalWall = !isHorizontalWall;
            transform.Translate(new Vector3(0, 0.15f, 0) * (isHorizontalWall ? -1 : 1)) ;
            sr.sprite = isHorizontalWall ? horSprites[0] : verSprites[0];
            yield return new WaitForSeconds(3f);
        }
    }
}
