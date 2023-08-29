using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookS : PiecesS { }

public class Rook : Pieces, ISpecialFirstMove
{

    public bool hasMoved = false;

    // Start is called before the first frame update
    public override void Start()
    {
        moveDirections.Add(new Vector2Int(-1, 0));
        moveDirections.Add(new Vector2Int(1, 0));
        moveDirections.Add(new Vector2Int(0, -1));
        moveDirections.Add(new Vector2Int(0, 1));
    }

    public override Pieces CopyPiece()
    {

        Rook copy = new Rook();
        copy.colourPiece = this.colourPiece;

        copy.gridPosition = this.gridPosition;
        copy.hasMoved = this.hasMoved;
        return copy;
    }

    public void FirstMove()
    {
        hasMoved = true;
    }

    public override int GetValuePiece() => 5;
}
