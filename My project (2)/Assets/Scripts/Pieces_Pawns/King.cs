using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Pieces
{

    public bool hasMoved = false;

    // Start is called before the first frame update
    void Start()
    {

        GameBoard.Instance.SetPieceAtLocation(gridPosition, this);

        maxMoveAmount = 1;
        moveDirections.Add(new Vector2Int(-1, -1));
        moveDirections.Add(new Vector2Int(1, 1));
        moveDirections.Add(new Vector2Int(1, -1));
        moveDirections.Add(new Vector2Int(-1, 1));
        moveDirections.Add(new Vector2Int(-1, 0));
        moveDirections.Add(new Vector2Int(1, 0));
        moveDirections.Add(new Vector2Int(0, -1));
        moveDirections.Add(new Vector2Int(0, 1));
    }

    //Rewrite
    public bool TryShortCastling()
    {

        if (hasMoved) return false;

        if (!GameBoard.Instance.TryGetSamePieceAtLocation(new Vector2Int(7, gridPosition.y), isWhite, out Pieces piece)) return false;
        
        if (!(piece is Rook)) return false;
        Debug.Log("test");

        //Has a rook at position
        Rook rook = (Rook)piece;

        if (rook.hasMoved) return false;


        //if (KingInCheck(white, position);
        for (int i = 1; i < 8; i++)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * 1), gridPosition.y);


            //Also stops before the rooks
            if (nextGridPoint.x >= 7) continue;
            if (nextGridPoint.x <= 0) continue;

            try
            {
                //Test if a piece is on the grid (last point)
                if (GameBoard.Instance.IsPieceAtLocation(nextGridPoint))
                {
                    return false;
                }

              // if (InCheck())


            }
            catch (System.Exception e)
            {

            }
        }

        return true;
    }

    public bool TryLongCastling()
    {
        if (hasMoved) return false;

        if (!GameBoard.Instance.TryGetSamePieceAtLocation(new Vector2Int(0, gridPosition.y), isWhite, out Pieces piece)) return false;

        if (!(piece is Rook)) return false;

        //Has a rook at position
        Rook rook = (Rook)piece;

        if (rook.hasMoved) return false;


        //if (KingInCheck(white, position);
        for (int i = 1; i < 8; i++)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * -1), gridPosition.y);


            //Also stops before the rooks
            if (nextGridPoint.x >= 7) continue;
            if (nextGridPoint.x <= 0) continue;

            try
            {
                //Test if a piece is on the grid (last point)
                if (GameBoard.Instance.IsPieceAtLocation(nextGridPoint))
                {
                    return false;
                }


            }
            catch (System.Exception e)
            {

            }
        }

        return true;
    }

    public override void SetGridPosition(Vector2Int newPosition)
    {
        base.SetGridPosition(newPosition);
        hasMoved = true;
    }
}
