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

        for(int x = 0; x < SIZE; x++)
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



}
