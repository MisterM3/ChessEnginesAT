using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAI : AbstractAIPlayer
{

    [SerializeField] protected bool isWhite = false;

    public bool canCastle = false;



    public override int EvaluateBoard(Pieces[,] boardState)
    {
        return 1;
    }

    int amount = 0;


    public override void MovePiece()
    {
        amount = 0;
 
        bestPieceToMove.Clear();
        bestGridPosition.Clear();

        int number = SearchingMethod(GameBoard.Instance.chessBoardPositions, depth, isWhite);

        int random = Random.Range(0, bestPieceToMove.Count);



        //Castling (rework if possible)


        Debug.Log(canCastle);

        if (canCastle)
        {
            if (bestPieceToMove[random] is King && (bestGridPosition[random].x == 2 || bestGridPosition[random].x == 6))
            {

                if (bestGridPosition[random].x == 2)
                {
                    if (GameBoard.Instance.TryGetPieceAtLocation(new Vector2Int(0, bestGridPosition[random].y), out Pieces rook))
                    {
                        GameBoard.Instance.SetPieceAtLocation(rook.gridPosition, null);
                        rook.SetGridPosition(new Vector2Int(3, bestGridPosition[random].y));
                        GameBoard.Instance.SetPieceAtLocation(new Vector2Int(3, bestGridPosition[random].y), rook);
                    }
                }

                if (bestGridPosition[random].x == 6)
                {
                    if (GameBoard.Instance.TryGetPieceAtLocation(new Vector2Int(7, bestGridPosition[random].y), out Pieces rook))
                    {
                        GameBoard.Instance.SetPieceAtLocation(rook.gridPosition, null);
                        rook.SetGridPosition(new Vector2Int(5, bestGridPosition[random].y));
                        GameBoard.Instance.SetPieceAtLocation(new Vector2Int(5, bestGridPosition[random].y), rook);
                    }
                }
            }
        }
        
            Debug.Log(amount);
            GameBoard.Instance.SetPieceAtLocation(bestPieceToMove[random].gridPosition, null);
            bestPieceToMove[random].SetGridPosition(bestGridPosition[random]);
            GameBoard.Instance.SetPieceAtLocation(bestGridPosition[random], bestPieceToMove[random]);
        


        canCastle = false;
        GameStateManager.Instance.NextTurn();
    }

    public override int SearchingMethod(Pieces[,] boardState, int pDepth, bool whiteMove)
    {


        int finalScore = 0;

        foreach (Pieces piece in boardState)
        {
            
            if (piece == null) continue;
            //Evaluate when depth has been reached
            if (pDepth <= 0)
            {
                amount++;
                return 1;
            }

            List<Vector2Int> moves = new();

            int i = 1;
            if (piece.isWhite == whiteMove)
            {
                moves = piece.MoveLocations(piece.gridPosition);

                //No moves possible for piece
                if (moves.Count == 0) continue;

                if (piece is King)
                {
                    King king = (King)piece;

                    if (king.TryLongCastling())
                    {
                        canCastle = true;
                        Pieces[,] newBoardState = boardState.Clone() as Pieces[,];

                        Pieces rook = newBoardState[0, piece.gridPosition.y];

                        //Castles long
                        newBoardState[piece.gridPosition.x, piece.gridPosition.y] = null;
                        newBoardState[2, piece.gridPosition.y] = king;

                        newBoardState[0, piece.gridPosition.y] = null;
                        newBoardState[3, piece.gridPosition.y] = rook;

                        int score = SearchingMethod(newBoardState, pDepth - 1, !whiteMove);
                        finalScore = score;
                        if (pDepth == depth)
                        {
                            //Change so only top part can add to best move
                            if (score > i)
                            {
                                i = score;
                                bestGridPosition.Clear();
                                bestPieceToMove.Clear();

                                bestGridPosition.Add(new Vector2Int(2, piece.gridPosition.y));
                                bestPieceToMove.Add(piece);
                                canCastle = true;
                            }

                            if (score == i)
                            {
                                bestGridPosition.Add(new Vector2Int(2, piece.gridPosition.y));
                                bestPieceToMove.Add(piece);
                                canCastle = true;
                            }
                        }

                    }

                    if (king.TryShortCastling())
                    {
                        Pieces[,] newBoardState = boardState.Clone() as Pieces[,];

                        canCastle = true;
                        Pieces rook = newBoardState[0, piece.gridPosition.y];

                        //Castles long
                        newBoardState[piece.gridPosition.x, piece.gridPosition.y] = null;
                        newBoardState[6, piece.gridPosition.y] = king;

                        newBoardState[7, piece.gridPosition.y] = null;
                        newBoardState[5, piece.gridPosition.y] = rook;

                        int score = SearchingMethod(newBoardState, pDepth - 1, !whiteMove);
                        finalScore = score;
                        if (pDepth == depth)
                        {
                            //Change so only top part can add to best move
                            if (score > i)
                            {
                                i = score;
                                bestGridPosition.Clear();
                                bestPieceToMove.Clear();

                                bestGridPosition.Add(new Vector2Int(6, piece.gridPosition.y));
                                bestPieceToMove.Add(piece);

                                canCastle = true;
                            }

                            if (score == i)
                            {
                                bestGridPosition.Add(new Vector2Int(6, piece.gridPosition.y));
                                bestPieceToMove.Add(piece);

                                canCastle = true;
                            }
                        }
                    }

                }

                //Recursivly go further in moves till max depth is reached
                foreach(Vector2Int move in moves)
                {
                    Pieces[,] newBoardState = boardState.Clone() as Pieces[,];

                   
                    //Puts the old position to null;
                    newBoardState[piece.gridPosition.x, piece.gridPosition.y] = null;
                    newBoardState[move.x, move.y] = piece;


                   
                    

                    int score = SearchingMethod(newBoardState, pDepth - 1, !whiteMove);
                    finalScore = score;
                    if (pDepth == depth)
                    {
                        /*
                        //Change so only top part can add to best move
                        if (score > i)
                        {
                            i = score;
                            bestGridPosition.Clear();
                            bestPieceToMove.Clear();

                            bestGridPosition.Add(move);
                            bestPieceToMove.Add(piece);
                        }

                        if (score == i)
                        {
                            bestGridPosition.Add(move);
                            bestPieceToMove.Add(piece);
                        }
                        */
                    }
                }

                



            }
        }


        return finalScore;



    }


    private void DoMove()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
