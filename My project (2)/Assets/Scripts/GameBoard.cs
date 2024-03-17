using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Shown board on map
/// </summary>

public class GameBoard : MonoBehaviour
{

    public static GameBoard Instance { get; private set; }

    public ChessBoard chessBoardPositions;

    //A chessBoard is 8 by 8
    public const int SIZE = 8;

    public UnityEvent boardChange;


    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
        ResetGame();
    }

    public void ResetGame()
    {
        chessBoardPositions = new ChessBoard();

        SetupBoard();
    }

    public void SetupBoard()
    {
        for (int x = 0; x < GameBoard.SIZE; x++)
        {
            for (int y = 0; y < GameBoard.SIZE; y++)
            {

                Vector2Int gridPosition = new Vector2Int(x, y);
                ColourChessSide side = ColourChessSide.Unassigned;
                Pieces pieceToPut = null;

                


                switch(y)
                {
                    case 0:
                        side = ColourChessSide.White;
                        break;
                    case 7:
                        side = ColourChessSide.Black;
                        break;

                    case 1:
                        side = ColourChessSide.White;
                        Pawns pawns = new();
                        pawns.notMoved = true;
                        pieceToPut = pawns;
                        break;
                    case 6:
                        side = ColourChessSide.Black;
                        Pawns pawn = new Pawns();
                        pawn.notMoved = true;
                        pieceToPut = pawn;
                        break;
                }

                if (y == 0 || y == 7)
                {

                    switch (x)
                    {
                        case 0:
                        case 7:
                            pieceToPut = new Rook();
                            break;

                        case 1:
                        case 6:
                            pieceToPut = new Knight();
                            break;

                        case 2:
                        case 5:
                            pieceToPut = new Bishop();
                            break;

                        case 3:
                            pieceToPut = new Queen();
                            break;

                        case 4:
                            pieceToPut = new King();
                            break;

                        default:
                            pieceToPut = null;
                            break;
                    }
                }

                //Debug.Log($"{pieceToPut} + { side} ");

                if (pieceToPut == null || side == ColourChessSide.Unassigned) continue;

                pieceToPut.Awake();
                pieceToPut.Start();
                PutPieceOnBoard(pieceToPut, gridPosition, side);
            }
        }
    }



    public void PutPieceOnBoard(Pieces piece, Vector2Int gridPosition, ColourChessSide side = ColourChessSide.White)
    {
        piece.colourPiece = side;
        piece.SetGridPosition(gridPosition);
        piece.board = chessBoardPositions;
        chessBoardPositions.SetPieceAtPosition(gridPosition, piece);

        if (piece is King) chessBoardPositions.setKing((King)piece);
    }



    public void ChangeBoard(ChessBoard newBoard)
    {
        chessBoardPositions = newBoard;

        //KEEP DOWN, FIRST LOGIC THAN CHANGE BOARD VISUALS!!!!
        boardChange?.Invoke();

    }


    

}
