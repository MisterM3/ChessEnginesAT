using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightS : PiecesS { }

public class Knight : Pieces
{
    // Start is called before the first frame update
    public override void Start()
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

    public override Pieces CopyPiece()
    {
        Knight copy = new Knight();
        copy.colourPiece = this.colourPiece;
        copy.gridPosition = this.gridPosition;
        return copy;
    }

    public override int GetValuePiece() => 3;
}
