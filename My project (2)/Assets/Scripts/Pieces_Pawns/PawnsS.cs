using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnsS : PiecesS { }

public class Pawns : Pieces, ISpecialFirstMove
{
    public bool notMoved = true;

    public bool enPassantAble = false;

    public bool dieTroughEnPassent = false;


    

    public override void Start()
    {
        ChooseColour();  
    }

    public void FirstMove()
    {
        notMoved = false;
    }

    public override int GetValuePiece() => 1;

    public void ChooseColour()
    {
        switch (colourPiece)
        {
            case ColourChessSide.White:
                moveDirections.Clear();
                moveDirections.Add(new Vector2Int(0, 1));
                break;
            case ColourChessSide.Black:
                moveDirections.Clear();
                moveDirections.Add(new Vector2Int(0, -1));
                break;
        }
    }

    public override Pieces CopyPiece()
    {
        Pawns copy = new Pawns();
        copy.colourPiece = this.colourPiece;
        copy.gridPosition = this.gridPosition;
        copy.notMoved = this.notMoved;
        copy.enPassantAble = this.enPassantAble;
        copy.dieTroughEnPassent = this.dieTroughEnPassent;
        copy.ChooseColour();
        return copy;
    }


    #region CheckSpaces

    public bool IsNoPieceFront()
    {
        Vector2Int positionPieceToGet = gridPosition + moveDirections[0];

        if (!IsInsideBoard(positionPieceToGet)) return false;

        Pieces piece = board.GetPieceFromPosition(positionPieceToGet);


        return piece == null;
    }

    //Checks two spaces ahead as the pawn can move two spaces as the first move
    public bool IsNoPieceDoubleFront()
    {

        Vector2Int positionPieceToGet = gridPosition + moveDirections[0] * 2;

        if (!IsInsideBoard(positionPieceToGet)) return false;

        Pieces piece = board.GetPieceFromPosition(positionPieceToGet);

        return piece == null;
    }

    public bool CanCapturePieceLeft()
    {

        Vector2Int positionPieceToGet = gridPosition + moveDirections[0] + new Vector2Int(-1, 0);

        if (!IsInsideBoard(positionPieceToGet)) return false;

        Pieces piece = board.GetPieceFromPosition(positionPieceToGet);

        if (piece == null) return false;

        return piece.colourPiece != this.colourPiece;

    }

    public bool CanCapturePieceRight()
    {
        Vector2Int positionPieceToGet = gridPosition + moveDirections[0] + new Vector2Int(1, 0);

        if (!IsInsideBoard(positionPieceToGet)) return false;

        Pieces piece = board.GetPieceFromPosition(positionPieceToGet);

        if (piece == null) return false;

        return piece.colourPiece != this.colourPiece;
    }

    #endregion


    public override List<Vector2Int> GetPseudoLegalMoves()
    {
        List<Vector2Int> movePositions = new List<Vector2Int>();

        try
        {

            if (CanCapturePieceLeft())
            {
                movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(-1, 0));
            }


            if (CanCapturePieceRight())
            {
                movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(1, 0));
            }



            if (IsNoPieceFront())
            {
                movePositions.Add(gridPosition + moveDirections[0]);
             //   Debug.LogWarning(notMoved);
                if (IsNoPieceDoubleFront() && notMoved)
                {
                    movePositions.Add(gridPosition + moveDirections[0] * 2);
                }

            }


        }
        catch (System.Exception e)
        {
            Debug.LogWarning("feafafa");
              Debug.LogError(e);
        }


        return movePositions;
    }


    public bool TryPromotion(Vector2Int movePosition)
    {

        if ((movePosition.y == 7 && colourPiece == ColourChessSide.White) || (movePosition.y == 0 && colourPiece == ColourChessSide.Black)) return true;

        return false;
    }


    public override void SetGridPosition(Vector2Int newPosition)
    {
        base.SetGridPosition(newPosition);

        if (enPassantAble) enPassantAble = false;

        if (!notMoved) return;

        //notMoved = false;
        if ((newPosition.y == 3 && colourPiece == ColourChessSide.White) || (newPosition.y == 4 && colourPiece == ColourChessSide.Black)) enPassantAble = true;
    }


}
