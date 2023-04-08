using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public static GameBoard Instance { get; private set; }

    public Pieces[,] chessBoardPositions;

    //A chessBoard is 8 by 8
    public const int SIZE = 8;

    public GameObject QueenB;
    public GameObject QueenW;
    public GameObject KnightB;
    public GameObject KnightW;
    public GameObject BishopB;
    public GameObject BishopW;
    public GameObject RookB;
    public GameObject RookW;





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

    public void RemovePieceFromLocation(Vector2Int gridPosition)
    {
        Pieces oldPiece = chessBoardPositions[gridPosition.x, gridPosition.y];

        Destroy(oldPiece.gameObject);
        chessBoardPositions[gridPosition.x, gridPosition.y] = null;

    }


    public void SetPieceAtLocation(Vector2Int gridPosition, Pieces piece, bool destroy = true)
    {
        if (piece != null)
        {
            Pieces oldPiece = chessBoardPositions[gridPosition.x, gridPosition.y];

            if (oldPiece != null && destroy) Destroy(oldPiece.gameObject);
        }


        chessBoardPositions[gridPosition.x, gridPosition.y] = piece;
    }

    //I HATE PROMOTIONS
    public Pieces SetPieceAtLocation(Vector2Int gridPosition, int pieceNumber)
    {
        switch(pieceNumber)
        {
            case 1:
                GameObject queenPrefab = Instantiate(QueenW, new Vector3(-100, 0, 0), Quaternion.identity);
                Queen queen = queenPrefab.GetComponent<Queen>();
                queen.gridPosition = gridPosition;
                queen.isWhite = true;
                queen.SetGridPosition(gridPosition);

                return queen;
                break;
            case 2:
                GameObject knightPrefab = Instantiate(KnightW, new Vector3(-100, 0, 0), Quaternion.identity);
                Knight knight = knightPrefab.GetComponent<Knight>();
                knight.gridPosition = gridPosition;
                knight.isWhite = true;
                knight.SetGridPosition(gridPosition);

                return knight;
                break;
            case 3:
                GameObject bishopPrefab = Instantiate(BishopW, new Vector3(-100, 0, 0), Quaternion.identity);
                Bishop bishop = bishopPrefab.GetComponent<Bishop>();
                bishop.gridPosition = gridPosition;
                bishop.isWhite = true;
                bishop.SetGridPosition(gridPosition);
                return bishop;
                break;
            case 4:
                GameObject rookPrefab = Instantiate(RookW, new Vector3(-100, 0, 0), Quaternion.identity);
                Rook rook = rookPrefab.GetComponent<Rook>();
                rook.gridPosition = gridPosition;
                rook.isWhite = true;
                rook.SetGridPosition(gridPosition);
                return rook;
                break;

            case -1:
                GameObject queenPrefabB = Instantiate(QueenB, new Vector3(-100, 0, 0), Quaternion.identity);
                Queen queenB = queenPrefabB.GetComponent<Queen>();
                queenB.gridPosition = gridPosition;
                queenB.isWhite = false;
                queenB.SetGridPosition(gridPosition);
                return queenB;
                break;
            case -2:
                GameObject knightPrefabB = Instantiate(KnightB, new Vector3(-100, 0, 0), Quaternion.identity);
                Knight knightB = knightPrefabB.GetComponent<Knight>();
                knightB.gridPosition = gridPosition;
                knightB.isWhite = false;
                knightB.SetGridPosition(gridPosition);
                return knightB;
                break;
            case -3:
                GameObject bishopPrefabB = Instantiate(BishopB, new Vector3(-100, 0, 0), Quaternion.identity);
                Bishop bishopB = bishopPrefabB.GetComponent<Bishop>();
                bishopB.gridPosition = gridPosition;
                bishopB.isWhite = false;
                bishopB.SetGridPosition(gridPosition);
                return bishopB;
                break;
            case -4:
                GameObject rookPrefabB = Instantiate(RookB, new Vector3(-100, 0, 0), Quaternion.identity);
                Rook rookB = rookPrefabB.GetComponent<Rook>();
                rookB.gridPosition = gridPosition;
                rookB.isWhite = false;
                rookB.SetGridPosition(gridPosition);
                return rookB;
                break;
        }

        return null;
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

    public bool TryGetSamePieceAtLocation(Vector2Int gridPosition, bool isWhite, out Pieces piece)
    {
        try
        {
            if (IsSameSidePieceAtLocation(gridPosition, isWhite))
            {
                piece = chessBoardPositions[gridPosition.x, gridPosition.y];
                return true;
            }
        }
        catch(System.Exception e)
        {
            piece = null;
            return false;
        }

        piece = null;
        return false;
    }

    public bool TryGetOtherPieceAtLocation(Vector2Int gridPosition, bool isWhite, out Pieces piece)
    {
        if (IsOtherSidePieceAtLocation(gridPosition, isWhite))
        {
            piece = chessBoardPositions[gridPosition.x, gridPosition.y];
            return true;
        }

        piece = null;
        return false;
    }

}
