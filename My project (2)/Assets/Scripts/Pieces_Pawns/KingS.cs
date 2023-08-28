using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingS : PiecesS { }

public class King : Pieces, ISpecialFirstMove
{

    public bool hasMoved = false;

    // Start is called before the first frame update
    public override void Start()
    {
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

    public override Pieces CopyPiece()
    {
        King copy = new King();
        copy.colourPiece = this.colourPiece;
        copy.gridPosition = this.gridPosition;
        copy.hasMoved = this.hasMoved;
        return copy;
    }

    public override int GetValuePiece() => 1000;

    public void FirstMove()
    {
        hasMoved = true;
    }


    #region KingCheck

    public bool InCheck()
    {
        //Check Horizontal/Vertical
        if (HorVerCheck()) return true;
        //Check Diagonal
        if (DiagonalCheck()) return true;
        //Check Horse
        if (HorseCheck()) return true;
        //Check Pawn
        if (PawnCheck()) return true;

        return false;
    }

    #region CheckCheckingPieces

    private bool HorVerCheck()
    {
        List<Vector2Int> horVerCheck = new List<Vector2Int>();

        horVerCheck.Add(new Vector2Int(0, -1));
        horVerCheck.Add(new Vector2Int(0, 1));
        horVerCheck.Add(new Vector2Int(-1, 0));
        horVerCheck.Add(new Vector2Int(1, 0));

        List<Pieces> pieces = CheckChecking(horVerCheck, 8);

        if (pieces == null || pieces.Count == 0) return false;

        foreach (Pieces piece in pieces)
        {
            if (piece is Queen || piece is Rook) return true;
        }

        return false;

    }

    private bool DiagonalCheck()
    {
        List<Vector2Int> diagonalCheck = new List<Vector2Int>();

        diagonalCheck.Add(new Vector2Int(-1, -1));
        diagonalCheck.Add(new Vector2Int(1, 1));
        diagonalCheck.Add(new Vector2Int(-1, 1));
        diagonalCheck.Add(new Vector2Int(1, -1));

        List<Pieces> pieces = CheckChecking(diagonalCheck, 8);

        if (pieces == null || pieces.Count == 0) return false;

        foreach (Pieces piece in pieces)
        {
            if (piece is Queen || piece is Bishop) return true;
        }

        return false;
    }

    private bool HorseCheck()
    {
        List<Vector2Int> horseCheck = new List<Vector2Int>();

        horseCheck.Add(new Vector2Int(-1, -2));
        horseCheck.Add(new Vector2Int(-2, -1));
        horseCheck.Add(new Vector2Int(-2, 1));
        horseCheck.Add(new Vector2Int(2, 1));
        horseCheck.Add(new Vector2Int(2, -1));
        horseCheck.Add(new Vector2Int(-1, 2));
        horseCheck.Add(new Vector2Int(1, 2));
        horseCheck.Add(new Vector2Int(1, -2));

        List<Pieces> pieces = CheckChecking(horseCheck, 1);

        if (pieces == null || pieces.Count == 0) return false;

        foreach (Pieces piece in pieces)
        {
            if (piece is Knight) return true;
        }

        return false;
    }

    private bool PawnCheck()
    {
        List<Vector2Int> pawnCheck = new List<Vector2Int>();


        if (this.colourPiece == ColourChessSide.White)
        {
            pawnCheck.Add(new Vector2Int(-1, -1));
            pawnCheck.Add(new Vector2Int(1, -1));
        }

        if (this.colourPiece == ColourChessSide.Black)
        {
            pawnCheck.Add(new Vector2Int(-1, 1));
            pawnCheck.Add(new Vector2Int(1, 1));
        }
        List<Pieces> pieces = CheckChecking(pawnCheck, 1);

        if (pieces == null || pieces.Count == 0) return false;

        foreach (Pieces piece in pieces)
        {
            if (piece is Pawns) return true;
        }

        return false;
    }

    #endregion CheckCheckingPieces
    //Returns the pieces the king "sees" in the directions
    private List<Pieces> CheckChecking(List<Vector2Int> directions, int amountToMove)
    {

        List<Pieces> seePieces = new List<Pieces>();

        foreach (Vector2Int direction in directions)
        {

            for (int i = 1; i <= amountToMove; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * direction.x), gridPosition.y + (i * direction.y));

                if (nextGridPoint.x > 7) continue;
                if (nextGridPoint.y > 7) continue;
                if (nextGridPoint.x < 0) continue;
                if (nextGridPoint.y < 0) continue;

                if (board.IsSquareEmpty(nextGridPoint)) continue;

                Pieces piece = board.GetPieceFromPosition(nextGridPoint);

                if (piece.colourPiece == this.colourPiece) continue;

                seePieces.Add(piece);
                
            }
        }

        return seePieces;
    }

    #endregion KingCheck




    /*


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

        if (InCheck()) return false;

        if (InCheck(new Vector2Int(3, gridPosition.y))) return false;
        if (InCheck(new Vector2Int(2, gridPosition.y))) return false;


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

        if (InCheck()) return false;

        if (InCheck(new Vector2Int(5, gridPosition.y))) return false;
        if (InCheck(new Vector2Int(6, gridPosition.y))) return false;


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

   

    */
    public override void SetGridPosition(Vector2Int newPosition)
    {
        base.SetGridPosition(newPosition);
        hasMoved = true;
    }
}
