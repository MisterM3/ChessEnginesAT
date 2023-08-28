using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessBoardS : MonoBehaviour { }

/// <summary>
/// Board with only scripts and functionallity
/// </summary>
/// 


public class ChessBoard
{

    private Pieces[,] chessBoard;

    private King blackKing;

    private King whiteKing;

    //A chessBoard is 8 by 8
    public const int SIZE = 8;

    public ChessBoard()
    {


        Awake();

    }


    public void Awake()
    {
        if (chessBoard != null) return;
        chessBoard = new Pieces[SIZE, SIZE];
    }


    public bool IsSquareEmpty(Vector2Int gridPosition)
    {
        return chessBoard[gridPosition.x, gridPosition.y] == null;
    }

    public Pieces GetPieceFromPosition(Vector2Int gridPosition)
    {
        return chessBoard[gridPosition.x, gridPosition.y];
    }

    public void SetPieceAtPosition(Vector2Int gridPosition, Pieces pieceToPutInPosition)
    {
        chessBoard[gridPosition.x, gridPosition.y] = pieceToPutInPosition;

        if (pieceToPutInPosition == null) return;
        pieceToPutInPosition.gridPosition = gridPosition;
        pieceToPutInPosition.board = this;
    }

    public Pieces[,] GetChessBoard()
    {
        return chessBoard;
    }



    //Rewrite to make a new piece
    public ChessBoard CopyBoard()
    {
        ChessBoard copiedBoard = new ChessBoard();

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {

                Vector2Int gridPosition = new Vector2Int(x, y);




                Pieces piece = chessBoard[x, y];


                if (piece == null) continue;


                Pieces pieceToCopy = piece.CopyPiece();

                if (pieceToCopy is King) copiedBoard.setKing((King)pieceToCopy);


                copiedBoard.SetPieceAtPosition(gridPosition, pieceToCopy);
            }
        }

        return copiedBoard;
    }

    public override string ToString()
    {
        return "tea";
    }

    public King getKing(ColourChessSide side)
    {
        if (side == ColourChessSide.White) return whiteKing;
        if (side == ColourChessSide.Black) return blackKing;

        return null;
    }

    public void setKing(King king)
    {
        if (king.colourPiece == ColourChessSide.White) whiteKing = king;
        else if (king.colourPiece == ColourChessSide.Black) blackKing = king;

        else Debug.LogError("No king to set");
    }


    //Mess (Remember to put in checkcheck after catling)
    public List<ChessBoard> GetCastlingMoves(ColourChessSide side)
    {
        List<ChessBoard> boardsWithCastling = new List<ChessBoard>();

        ChessBoard copy = this.CopyBoard();

        int sideNumber = 4;

        if (side == ColourChessSide.White)
        {
            if (copy.whiteKing.hasMoved) return null;
            sideNumber = 0;
        }

        if (side == ColourChessSide.Black)
        {
            if (copy.blackKing.hasMoved) return null;
            sideNumber = 7;
        }



        if (copy.IsSquareEmpty(new Vector2Int(1, sideNumber)) &&
            copy.IsSquareEmpty(new Vector2Int(2, sideNumber)) &&
            copy.IsSquareEmpty(new Vector2Int(3, sideNumber)))
        {
            Pieces checkPiece = copy.GetPieceFromPosition(new Vector2Int(0, sideNumber));

            if (checkPiece != null && checkPiece is Rook)
            {
                Rook rook = (Rook)checkPiece;
                if (!rook.hasMoved)
                {
                    ChessBoard longCastle = this.CopyBoard();

                    longCastle.SetPieceAtPosition(new Vector2Int(2, sideNumber), whiteKing.CopyPiece());
                    longCastle.SetPieceAtPosition(new Vector2Int(3, sideNumber), rook.CopyPiece());
                    longCastle.SetPieceAtPosition(new Vector2Int(0, sideNumber), null);
                    longCastle.SetPieceAtPosition(new Vector2Int(4, sideNumber), null);

                    boardsWithCastling.Add(longCastle);
                }
            }
        }



        if (copy.IsSquareEmpty(new Vector2Int(6, sideNumber)) &&
            copy.IsSquareEmpty(new Vector2Int(5, sideNumber)))
        {
            Pieces checkPiece = copy.GetPieceFromPosition(new Vector2Int(7, sideNumber));

            if (checkPiece != null && checkPiece is Rook)
            {
                Rook rook = (Rook)checkPiece;
                if (!rook.hasMoved)
                {
                    ChessBoard shortCastle = this.CopyBoard();

                    shortCastle.SetPieceAtPosition(new Vector2Int(6, sideNumber), whiteKing.CopyPiece());
                    shortCastle.SetPieceAtPosition(new Vector2Int(5, sideNumber), rook.CopyPiece());
                    shortCastle.SetPieceAtPosition(new Vector2Int(7, sideNumber), null);
                    shortCastle.SetPieceAtPosition(new Vector2Int(4, sideNumber), null);

                    boardsWithCastling.Add(shortCastle);
                }
            }
        }
        return boardsWithCastling;
    }

}




