using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : ObjectTile
{

    private SpriteRenderer sr;
    private SpriteRenderer srC;

    public List<Sprite> flagSprite;

    public bool isChecked;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        srC = transform.GetChild(0).GetComponent<SpriteRenderer>();
        srC.sprite = flagSprite[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            tb.rigid.velocity = Vector3.zero;
            //tb.transform.position = transform.position;
            Destroy(tb.gameObject);
            sr.color = Color.green;
            srC.sprite = flagSprite[1];
            isChecked = true;
            GameManager.Instance.CheckClear();
        }
    }


}
