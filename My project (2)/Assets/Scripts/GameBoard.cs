using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    Pieces[,] chessBoardPositions;

    //A chessBoard is 8 by 8
    const int size = 8;



    private void Awake()
    {
        chessBoardPositions = new Pieces[size, size];
    }


    public bool IsPieceAtLocation(Vector2Int gridPosition)
    {
        return chessBoardPositions[gridPosition.x, gridPosition.y] != null;
    }
}
