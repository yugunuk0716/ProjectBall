using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GellWall : ObjectTile
{
    public TileDirection wallDirection;

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

            tb.Move(vec, 5);
        }
    }
}
