using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GellWallDirection
{
    UP,
    UPLEFT,
    UPRIGHT,
    DOWN,
    DOWNLEFT,
    DOWNRIGHT,
    LEFT,
    RIGHT
}


public class TestGellWall : MonoBehaviour
{

    public GellWallDirection wallDirection;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();
       
        if (tb != null)
        {

            Vector3 vec = Vector3.zero;

            switch (wallDirection)
            {
                case GellWallDirection.UP:
                    vec = Vector3.up;
                    break;
                case GellWallDirection.UPLEFT:
                    vec.Set(-1f, 1f, 0f);
                    break;
                case GellWallDirection.UPRIGHT:
                    vec.Set(1f, 1f, 0f);
                    break;
                case GellWallDirection.DOWN:
                    vec = Vector3.down;
                    break;
                case GellWallDirection.DOWNLEFT:
                    vec.Set(-1f, -1f, 0f);
                    break;
                case GellWallDirection.DOWNRIGHT:
                    vec.Set(1f, -1f, 0f);
                    break;
                case GellWallDirection.LEFT:
                    vec = Vector3.left;
                    break;
                case GellWallDirection.RIGHT:
                    vec = Vector3.right;
                    break;

            }

            tb.transform.position = transform.position;
            tb.rigid.velocity = vec * 5;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if(tb != null)
        {

            Vector3 vec = Vector3.zero;

            switch (wallDirection)
            {
                case GellWallDirection.UP:
                    vec = Vector3.up;   
                    break;
                case GellWallDirection.UPLEFT:
                    vec.Set(-1f, 1f, 0f);
                    break;
                case GellWallDirection.UPRIGHT:
                    vec.Set(1f, 1f, 0f);
                    break;
                case GellWallDirection.DOWN:
                    vec = Vector3.down;
                    break;
                case GellWallDirection.DOWNLEFT:
                    vec.Set(-1f, -1f, 0f);
                    break;
                case GellWallDirection.DOWNRIGHT:
                    vec.Set(1f, -1f, 0f);
                    break;
                case GellWallDirection.LEFT:
                    vec = Vector3.left;
                    break;
                case GellWallDirection.RIGHT:
                    vec = Vector3.right;
                    break;
              
            }

            tb.transform.position = transform.position;
            tb.rigid.velocity = vec * 5;
        }
    }
}
