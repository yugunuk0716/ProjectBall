using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GellWallDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}


public class GellWall : ObjectTile
{

    public GellWallDirection wallDirection;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {

            Vector3 vec = Vector3.zero;
            Vector3 vec2 = tb.rigid.velocity;

            //print(collision.contacts[0].normal);
            tb.transform.position = transform.position;
            vec = Vector3.Cross(Vector3.forward, vec2).normalized;

            int co = 1;

            switch (wallDirection)
            {
                case GellWallDirection.UP:
                case GellWallDirection.RIGHT:
                    co = -1;
                    break;
            }

            tb.Move(vec * co, 5);
        }

    }


}
