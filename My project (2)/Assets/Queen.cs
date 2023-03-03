using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Pieces
{
    // Start is called before the first frame update
    void Start()
    {

        GameBoard.Instance.SetPieceAtLocation(gridPosition, this);

        moveDirections.Add(new Vector2Int(-1, -1));
        moveDirections.Add(new Vector2Int(1, 1));
        moveDirections.Add(new Vector2Int(1, -1));
        moveDirections.Add(new Vector2Int(-1, 1));
        moveDirections.Add(new Vector2Int(-1, 0));
        moveDirections.Add(new Vector2Int(1, 0));
        moveDirections.Add(new Vector2Int(0, -1));
        moveDirections.Add(new Vector2Int(0, 1));
    }
}
