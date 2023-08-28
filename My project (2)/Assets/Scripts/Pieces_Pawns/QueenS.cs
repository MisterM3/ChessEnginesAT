using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenS : PiecesS { }

public class Queen : Pieces
{
    // Start is called before the first frame update
    public override void Start()
    {
        moveDirections.Add(new Vector2Int(-1, -1));
        moveDirections.Add(new Vector2Int(1, 1));
        moveDirections.Add(new Vector2Int(1, -1));
        moveDirections.Add(new Vector2Int(-1, 1));
        moveDirections.Add(new Vector2Int(-1, 0));
        moveDirections.Add(new Vector2Int(1, 0));
        moveDirections.Add(new Vector2Int(0, -1));
        moveDirections.Add(new Vector2Int(0, 1));
    }

    public override Pieces CopyPiece()
    {
        Queen copy = new Queen();
        copy.colourPiece = this.colourPiece;
        copy.gridPosition = this.gridPosition;
        return copy;
    }

    public override int GetValuePiece() => 9;

}
