using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopS : PiecesS { }

public class Bishop : Pieces
{



    // Start is called before the first frame update
    public override void Start()
    {
        moveDirections.Add(new Vector2Int(-1, -1));
        moveDirections.Add(new Vector2Int(1, 1));
        moveDirections.Add(new Vector2Int(1, -1));
        moveDirections.Add(new Vector2Int(-1, 1));
    }

    public override Pieces CopyPiece()
    {
        Bishop copy = new Bishop();
        copy.colourPiece = this.colourPiece;
        copy.gridPosition = this.gridPosition;
        return copy;
    }

    public override int GetValuePiece() => 3;


}
