using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameBoard))]
public class ChessBoardVisual : MonoBehaviour
{


    [SerializeField] GameObject WhiteSpacePrefab;
    [SerializeField] GameObject BlackSpacePrefab;
    [SerializeField] GameObject parentBoardVisual;

    [SerializeField] GameObject moveAblePrefab;


    [SerializeField] GameObject parentPieces;

    [Header("Pieces")]
    public GameObject QueenB;
    public GameObject QueenW;
    public GameObject KnightB;
    public GameObject KnightW;
    public GameObject BishopB;
    public GameObject BishopW;
    public GameObject RookB;
    public GameObject RookW;
    public GameObject PawnB;
    public GameObject PawnW;
    public GameObject KingB;
    public GameObject KingW;

    GameObject[,] piecesVisuals;

    GameObject[,] MoveAbleSpacesVisuals;



    // Start is called before the first frame update
    void Start()
    {

        MoveAbleSpacesVisuals = new GameObject[GameBoard.SIZE, GameBoard.SIZE];

        for (int x = 0; x < GameBoard.SIZE; x++)
        {
            for (int y = 0; y < GameBoard.SIZE; y++)
            {
                Vector3Int position = new Vector3Int(x, 0, y);

                if ((x - y)%2 == 0)
                {
                    Instantiate(BlackSpacePrefab, position, Quaternion.identity, parentBoardVisual.transform);
                }
                else Instantiate(WhiteSpacePrefab,position, Quaternion.identity, parentBoardVisual.transform);

               

                GameObject moveAbleVisual = Instantiate(moveAblePrefab, position, Quaternion.identity, parentBoardVisual.transform);

                MoveAbleSpacesVisuals[x, y] = moveAbleVisual;


            }
        }


        SetPieces();

        DeActivateAllMoveVisual();
    }

    public void SetPieces()
    {

        for (int i = parentPieces.transform.childCount; i > 0; i++)
        {
            Destroy(parentPieces.transform.GetChild(i));
        }

        for (int x = 0; x < GameBoard.SIZE; x++)
        {
            for (int y = 0; y < GameBoard.SIZE; y++)
            {
                Vector3Int position = new Vector3Int(x, 0, y);

                Vector2Int gridPosition = new Vector2Int(x, y);

                Pieces piece = GameBoard.Instance.chessBoardPositions.GetPieceFromPosition(gridPosition);


                if (piece is Rook)
                {
                    if (piece.colourPiece == ColourChessSide.White) Instantiate(RookW, position, Quaternion.identity, parentPieces.transform);
                    else if (piece.colourPiece == ColourChessSide.Black) Instantiate(RookB, position, Quaternion.identity, parentPieces.transform);
                }

                if (piece is Knight)
                {
                    if (piece.colourPiece == ColourChessSide.White) Instantiate(KnightW, position, Quaternion.identity, parentPieces.transform);
                    else if (piece.colourPiece == ColourChessSide.Black) Instantiate(KnightB, position, Quaternion.identity, parentPieces.transform);
                }

                if (piece is Bishop)
                {
                    if (piece.colourPiece == ColourChessSide.White) Instantiate(BishopW, position, Quaternion.identity, parentPieces.transform);
                    else if (piece.colourPiece == ColourChessSide.Black) Instantiate(BishopB, position, Quaternion.identity, parentPieces.transform);
                }

                if (piece is Queen)
                {
                    if (piece.colourPiece == ColourChessSide.White) Instantiate(QueenW, position, Quaternion.identity, parentPieces.transform);
                    else if (piece.colourPiece == ColourChessSide.Black) Instantiate(QueenB, position, Quaternion.identity, parentPieces.transform);
                }

                if (piece is King)
                {
                    if (piece.colourPiece == ColourChessSide.White) Instantiate(KingW, position, Quaternion.identity, parentPieces.transform);
                    else if (piece.colourPiece == ColourChessSide.Black) Instantiate(KingB, position, Quaternion.identity, parentPieces.transform);
                }

                if (piece is Pawns)
                {
                    if (piece.colourPiece == ColourChessSide.White) Instantiate(PawnW, position, Quaternion.identity, parentPieces.transform);
                    else if (piece.colourPiece == ColourChessSide.Black) Instantiate(PawnB, position, Quaternion.identity, parentPieces.transform);
                }

               
            }
        }
    }


    /*
    public void SetPieces()
    {

        for (int i = parentPieces.transform.childCount; i > 0; i++)
        {
            Destroy(parentPieces.transform.GetChild(i));
        }




        for (int x = 0; x < GameBoard.SIZE; x++)
        {
            for (int y = 0; y < GameBoard.SIZE; y++)
            {
                Vector3Int position = new Vector3Int(x, 0, y);


                if (position.z == 0)
                {
                    if (position.x == 0 || position.x == 7)
                    {
                        Instantiate(RookW, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 1 || position.x == 6)
                    {
                        Instantiate(KnightW, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 2 || position.x == 5)
                    {
                        Instantiate(BishopW, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 3)
                    {
                        Instantiate(QueenW, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 4)
                    {
                        Instantiate(KingW, position, Quaternion.identity, parentPieces.transform);
                    }
                }

                if (position.z == 7)
                {
                    if (position.x == 0 || position.x == 7)
                    {
                        Instantiate(RookB, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 1 || position.x == 6)
                    {
                        Instantiate(KnightB, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 2 || position.x == 5)
                    {
                        Instantiate(BishopB, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 3)
                    {
                        Instantiate(QueenB, position, Quaternion.identity, parentPieces.transform);
                    }
                    else if (position.x == 4)
                    {
                        Instantiate(KingB, position, Quaternion.identity, parentPieces.transform);
                    }
                }


                if (position.z == 1)
                {
                    Instantiate(PawnW, position, Quaternion.identity, parentPieces.transform);
                }

                else if (position.z == 6)
                {
                    Instantiate(PawnB, position, Quaternion.identity, parentPieces.transform);
                }
            }
        }
    }
    */


    public void InstantiatePiece(GameObject Piece, Vector3Int position)
    {
        Instantiate(Piece, position, Quaternion.identity, parentPieces.transform);
    }

    public void MovePieceVisual()
    {
        for (int i = parentPieces.transform.childCount - 1; i >= 0; i--)
        {
             Destroy(parentPieces.transform.GetChild(i).gameObject);
        }
       
        ChessBoard board = GameBoard.Instance.chessBoardPositions;

        foreach (Pieces piece in board.GetChessBoard())
        {
            if (piece == null) continue;


            Vector3Int position = new Vector3Int(piece.gridPosition.x, 0, piece.gridPosition.y);
            if (piece is Rook)
            {
                if (piece.colourPiece == ColourChessSide.White)
                {
                    InstantiatePiece(RookW, position);
                }
                else
                {
                    InstantiatePiece(RookB, position);
                }
            }

            if (piece is Knight)
            {
                if (piece.colourPiece == ColourChessSide.White)
                {
                    InstantiatePiece(KnightW, position);
                }
                else
                {
                    InstantiatePiece(KnightB, position);
                }
            }

            if (piece is Bishop)
            {
                if (piece.colourPiece == ColourChessSide.White)
                {
                    InstantiatePiece(BishopW, position);
                }
                else
                {
                    InstantiatePiece(BishopB, position);
                }
            }

            if (piece is King)
            {
                if (piece.colourPiece == ColourChessSide.White)
                {
                    InstantiatePiece(KingW, position);
                }
                else
                {
                    InstantiatePiece(KingB, position);
                }
            }
            if (piece is Queen)
            {
                if (piece.colourPiece == ColourChessSide.White)
                {
                    InstantiatePiece(QueenW, position);
                }
                else
                {
                    InstantiatePiece(QueenB, position);
                }
            }
            if (piece is Pawns)
            {
                if (piece.colourPiece == ColourChessSide.White)
                {
                    InstantiatePiece(PawnW, position);
                }
                else
                {
                    InstantiatePiece(PawnB, position);
                }
            }
        }
    }

    public void DeActivateAllMoveVisual()
    {
        foreach (GameObject visual in MoveAbleSpacesVisuals) visual.SetActive(false);
    }

    public void ActivateMoveVisuals(List<Vector2Int> positions)
    {

        DeActivateAllMoveVisual();

       

        foreach (Vector2Int position in positions)
        {
            try
            {
                MoveAbleSpacesVisuals[position.x, position.y].SetActive(true);

            }
            catch (System.Exception e)
            {

            }
        }
    }

    /*

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


    */



}
