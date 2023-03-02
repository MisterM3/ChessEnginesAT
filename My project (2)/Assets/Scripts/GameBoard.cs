using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    Pieces[,] chessBoardPositions;

    //A chessBoard is 8 by 8
    public const int SIZE = 8;



    private void Awake()
    {
        chessBoardPositions = new Pieces[SIZE, SIZE];
    }


    public bool IsPieceAtLocation(Vector2Int gridPosition)
    {
        return chessBoardPositions[gridPosition.x, gridPosition.y] != null;
    }
}
