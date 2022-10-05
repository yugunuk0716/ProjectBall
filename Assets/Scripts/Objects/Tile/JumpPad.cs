using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        tb.SetMove();
    }

    public override IEnumerator Transition()
    {
        yield return null;
    }
}
