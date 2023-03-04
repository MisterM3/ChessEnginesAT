using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawns : Pieces
{
    public bool notMoved = true;

    public bool enPassantAble = false;

    // Start is called before the first frame update
    void Start()
    {
        GameBoard.Instance.SetPieceAtLocation(gridPosition, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {

        if (isWhite) moveDirections.Add(new Vector2Int(0, 1));
        else moveDirections.Add(new Vector2Int(0, -1));




        List<Vector2Int> movePositions = new List<Vector2Int>();

        try
        {

            //FirstMoveDouble
            if (notMoved && !GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0] * 2))
            {
                
                movePositions.Add(gridPosition + moveDirections[0] * 2);
            }

            //Top
            if (!GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0]))
            {
            
                movePositions.Add(gridPosition + moveDirections[0]);
            }

            Debug.Log(gridPosition + moveDirections[0] + new Vector2Int(1, 0));

            //Left Capture
            if (GameBoard.Instance.IsOtherSidePieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(-1, 0), isWhite))
            {
                movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(-1, 0));
            }

            //Right Capture
            if (GameBoard.Instance.IsOtherSidePieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(1, 0), isWhite))
            {
                movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(1, 0));
            }

            //Enpassant
            if (GameBoard.Instance.TryGetPieceAtLocation(gridPosition + new Vector2Int(1, 0), out Pieces piece) && !GameBoard.Instance.IsOtherSidePieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(1, 0), isWhite))
            {

                //Check if enpassantable
                if (piece is Pawns)
                {
                    Pawns pawn = (Pawns)piece;

                    if (pawn.enPassantAble) movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(1, 0));
                }
            }



            if (GameBoard.Instance.TryGetPieceAtLocation(gridPosition + new Vector2Int(-1, 0), out Pieces piece2) && !GameBoard.Instance.IsOtherSidePieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(-1, 0), isWhite))
            {

                //Check if enpassantable
                if (piece2 is Pawns)
                {
                    Pawns pawn = (Pawns)piece2;

                    if (pawn.enPassantAble) movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(-1, 0));
                }
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }


        return movePositions;
    }


    public override void SetGridPosition(Vector2Int newPosition)
    {
        base.SetGridPosition(newPosition);

        if (enPassantAble) enPassantAble = false;

        if (!notMoved) return;

        notMoved = false;
        if ((newPosition.y == 3 && isWhite) || (newPosition.y == 4 && !isWhite)) enPassantAble = true;
    }
}
