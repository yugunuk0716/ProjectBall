using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectTile
{
    public override void OnTriggerBall(Ball tb)
    {
        Destroy(tb.gameObject);
    }
}
