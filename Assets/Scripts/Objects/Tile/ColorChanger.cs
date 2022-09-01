using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : ObjectTile
{
    public Color targetColor;

    public override void InteractionTile(Ball tb)
    {
        tb.ColorChange(targetColor);
    }

    public override void Reset()
    {

    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
