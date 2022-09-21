using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReflectWall : ObjectTile
{
    public bool isHorizontalWall = true;
    
    public Sprite[] horSprites; // 0 기본 1  오른쪽으로  2 왼쪽으로
    public Sprite[] verSprites; // 0 기본 1  위쪽으로    2 아래쪽으로
    public Sprite[] horEffect;
    public Sprite[] verEffect;
    private SpriteRenderer sr;

    WaitForSeconds changeTerm = new WaitForSeconds(0.175f);

    protected override void Awake()
    { 
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }


    IEnumerator ChangeSprite(Sprite[] targetSpriteArray, int targetNumber)
    {
        Sprite origin = sr.sprite;
        sr.sprite = targetSpriteArray[targetNumber];
        yield return changeTerm;
        sr.sprite = origin;
    }

    

    public override void InteractionTile(Ball tb)
    {
        Sprite[] sprites = isHorizontalWall ? horEffect : verEffect;
        int target = 0;

        StopCoroutine(ChangeSprite(sprites, target));

        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Wall");

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
                    target = 1;
                    break;

                case Vector2 v when v.Equals(Vector2.up):
                    tb.SetBall(Vector2.left, tb.speed);
                    target = 1;
                    break;
            }
        }
        else
        {
            switch (tb.direction)
            {
                case Vector2 v when v.Equals(Vector2.right):
                    tb.SetBall(Vector2.up, tb.speed);
                    target = 1;
                    break;

                case Vector2 v when v.Equals(Vector2.up):
                    tb.SetBall(Vector2.right, tb.speed);
                    target = 1;
                    break;

                case Vector2 v when v.Equals(Vector2.down):
                    tb.SetBall(Vector2.left, tb.speed);
                    break;

                case Vector2 v when v.Equals(Vector2.left):
                    tb.SetBall(Vector2.down, tb.speed);
                    break;


            }
        }

        StartCoroutine(ChangeSprite(sprites, target));
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
