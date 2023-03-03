using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public static GameBoard Instance { get; private set; }

    Pieces[,] chessBoardPositions;

    //A chessBoard is 8 by 8
    public const int SIZE = 8;



    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        chessBoardPositions = new Pieces[SIZE, SIZE];
    }


    public bool IsPieceAtLocation(Vector2Int gridPosition)
    {
        return chessBoardPositions[gridPosition.x, gridPosition.y] != null;
    }

    public bool IsSameSidePieceAtLocation(Vector2Int gridPosition, bool isWhite)
    {
        return chessBoardPositions[gridPosition.x, gridPosition.y].isWhite == isWhite;
    }

    public bool IsOtherSidePieceAtLocation(Vector2Int gridPosition, bool isWhite)
    {
        if (chessBoardPositions[gridPosition.x, gridPosition.y] == null) return false;

        return chessBoardPositions[gridPosition.x, gridPosition.y].isWhite != isWhite;
    }

    public bool IsPieceAtLocation(Vector2Int gridPosition, out bool isWhite)
    {
        Pieces piece = chessBoardPositions[gridPosition.x, gridPosition.y];

        if (piece == null)
        {
            isWhite = false;
            return false;
        }

        isWhite = piece.isWhite;
        return true;
    }


    public void SetPieceAtLocation(Vector2Int gridPosition, Pieces piece)
    {
        if (piece != null)
        {
            Pieces oldPiece = chessBoardPositions[gridPosition.x, gridPosition.y];

            if (oldPiece != null) Destroy(oldPiece.gameObject);
        }

        chessBoardPositions[gridPosition.x, gridPosition.y] = piece;
    }

    public bool TryGetPieceAtLocation(Vector2Int gridPosition, out Pieces piece )
    {
        if (IsPieceAtLocation(gridPosition))
        {
            piece = chessBoardPositions[gridPosition.x, gridPosition.y];
            return true;
        }

        piece = null;
        return false;
    }
    
}
