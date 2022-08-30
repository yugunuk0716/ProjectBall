using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControllUI : UIBase
{
    [SerializeField] private Image ballImg;
    [SerializeField] private Image directionImg;
    [SerializeField] private Image abilityImg;
    [SerializeField] private BallControllUI ballControllUI;

    public void SetBallSprites(Sprite ballSprite, Sprite stateSprite)
    {
        ballImg.sprite= ballSprite;
        abilityImg.sprite= stateSprite;
    }

    public void SetDirection(TileDirection dir, bool active = true)
    {
        float z = 0;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                z = 45f;
                break;
            case TileDirection.RIGHTDOWN:
                z = 315f;
                break;
            case TileDirection.LEFTDOWN:
                z = 225f;
                break;
            case TileDirection.LEFTUP:
                z = 135;
                break;
        }

        directionImg.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        directionImg.gameObject.SetActive(active);
    }

    public override void Init()
    {
        
    }

    public override void Load()
    {
        // 할 거 없음
    }
}
