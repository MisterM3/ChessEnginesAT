using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Board with only scripts and functionallity
/// </summary>
public class ChessBoard : MonoBehaviour
{
    private Pieces[,] chessBoard;

    private King blackKing;

    private King whiteKing;

    //A chessBoard is 8 by 8
    public const int SIZE = 8;

    public ChessBoard()
    {


        chessBoard = new Pieces[SIZE, SIZE];
    }


    public void Awake()
    {
        if (chessBoard != null) return;
        chessBoard = new Pieces[SIZE, SIZE];
    }

    public bool IsSameSidePieceAtLocation(Vector2Int gridPosition, bool isWhite)
    {
        return chessBoard[gridPosition.x, gridPosition.y].isWhite == isWhite;
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
    }


    public ChessBoard CopyBoard()
    {
        ChessBoard copiedBoard = new ChessBoard();

        for(int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {

                Vector2Int gridPosition = new Vector2Int(x, y);

                Pieces pieceToCopy = this.chessBoard[x, y];

                copiedBoard.SetPieceAtPosition(gridPosition, pieceToCopy);
            }
        }

        return copiedBoard;
    }



}
