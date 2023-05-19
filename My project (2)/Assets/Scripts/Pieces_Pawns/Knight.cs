using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Pieces
{
    // Start is called before the first frame update
    void Start()
    {
        maxMoveAmount = 1;

        moveDirections.Add(new Vector2Int(-1, -2));
        moveDirections.Add(new Vector2Int(-2, -1));
        moveDirections.Add(new Vector2Int(-2, 1));
        moveDirections.Add(new Vector2Int(2, 1));
        moveDirections.Add(new Vector2Int(2, -1));
        moveDirections.Add(new Vector2Int(-1, 2));
        moveDirections.Add(new Vector2Int(1, 2));
        moveDirections.Add(new Vector2Int(1, -2));
    }
}
