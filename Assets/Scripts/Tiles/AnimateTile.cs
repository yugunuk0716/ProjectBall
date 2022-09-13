using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimateTile
{
    

    public void SetTile(Tilemap tileMap, TileBase animateTile, TileBase endTile, Vector3Int pos, string target)
    {
        tileMap.SetTile(pos, animateTile);

        Action updateAction = null;

        updateAction = () =>
        {
            string currentName = tileMap.GetSprite(pos).name;

            if (currentName.Equals(target))
            {
                tileMap.SetTile(pos, endTile);

                FunctionUpdater.Delete(updateAction);
            }
        };
        FunctionUpdater.Create(updateAction);
    }

}
