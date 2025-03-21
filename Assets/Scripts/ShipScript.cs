using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ShipScript : MonoBehaviour
{
    List<GameObject> touchTiles = new List<GameObject>();
    public float xOffset = 0;
    public float zOffset = 0;


    public void ClearTileList()
    {
        touchTiles.Clear();
    }

    public Vector3 GetOffsetVec(Vector3 tilePos)
    {
        return new Vector3(tilePos.x + xOffset,44, tilePos.z + zOffset);
    }



    public void SetPosition(Vector3 newVec)
    {
        transform.localPosition = new Vector3(newVec.x + xOffset, 44, newVec.z + zOffset);
    }

}