using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAI : AbstractAIPlayer
{

    [SerializeField] protected bool isWhite = false;

    public bool canCastle = false;



    public void Start()
    {
        if (isWhite) finalScore = int.MinValue;
        if (!isWhite) finalScore = int.MaxValue;
    }


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

        Debug.LogWarning(number);

        int random = Random.Range(0, bestPieceToMove.Count);



        foreach (Pieces piece in GameBoard.Instance.chessBoardPositions)
        {
         //   if ((isWhite == piece.isWhite) && piece is Pawns)
         //   {
          //      Pawns pawn = (Pawns)piece;
           //     pawn.enPassantAble = false;
           // }
        }

        //Castling (rework if possible)


        //Debug.Log(canCastle);

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


        Debug.Log(bestPieceToMove.Count);

        if (bestPieceToMove[random] is Pawns && (bestGridPosition[random].y == 7 || bestGridPosition[random].y == 0))
        {

         //   GameBoard.Instance.SetPieceAtLocation(bestPieceToMove[random].gridPosition, null);

            Pieces queen;


            if (isWhite)
            {
                queen = GameBoard.Instance.SetPieceAtLocation(bestGridPosition[random], 1);
            }
            else queen = GameBoard.Instance.SetPieceAtLocation(bestGridPosition[random], -1);


        }
        

        
        
        //Enpassent, remove pawn
        if (bestPieceToMove[random] is Pawns && (!GameBoard.Instance.IsPieceAtLocation(bestGridPosition[random])) && bestPieceToMove[random].gridPosition.x != bestGridPosition[random].x)
        {
            Debug.Log(bestGridPosition[random]);
            if (!GameBoard.Instance.IsPieceAtLocation(bestGridPosition[random])) Debug.LogError("true");
            Vector2Int test = new Vector2Int(bestGridPosition[random].x, bestPieceToMove[random].gridPosition.y);
            Debug.Log(test);
            GameBoard.Instance.RemovePieceFromLocation(test);
            Debug.Log("tefeafst");
        }

            Debug.LogError("Nodes: " + amount);
            GameBoard.Instance.SetPieceAtLocation(bestPieceToMove[random].gridPosition, null);
            bestPieceToMove[random].SetGridPosition(bestGridPosition[random]);
            GameBoard.Instance.SetPieceAtLocation(bestGridPosition[random], bestPieceToMove[random]);
        


        canCastle = false;
        GameStateManager.Instance.NextTurn();
    }

  
    int finalScore = 0;

    /*
    public override int SearchingMethod(Pieces[,] boardState, int pDepth, bool whiteMove)
    {


        int i = 0;

        Debug.LogError(whiteMove);



        if (pDepth <= 0)
        {
            amount++;
            if (evaluationMethodStragety != null)
            {
                Debug.LogError(boardState);
                Debug.Log(boardState[2, 2]);
                return evaluationMethodStragety.Evaluate(boardState);
            }
        }
            foreach (Pieces piece in boardState)
        {
          //  if ((isWhite == piece.isWhite) && piece is Pawns)
        //    {
          //      Pawns pawn = (Pawns)piece;
          //      pawn.enPassantAble = false;
          //  }
        }

        foreach (Pieces piece in boardState)
        {
            
            if (piece == null) continue;
            //Evaluate when depth has been reached

            List<Vector2Int> moves = new();

           
            if (piece.isWhite == whiteMove)
            {
                moves = piece.GetLegalMoves(piece.gridPosition);

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
                  //      if (pDepth == depth)
                        {
                            //Change so only top part can add to best move
                            if ((score > i && isWhite) || (score < i && !isWhite))
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
                        //    if (pDepth == depth)
                        {
                            //Change so only top part can add to best move
                            if ((score > i && isWhite) || (score < i && !isWhite))
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
                    
                    if (piece is Pawns)
                    {
                        Pawns pawn = (Pawns)piece;

                        if (pawn.TryPromotion(move))
                        {

                            Pieces[,] queenPromotion = boardState.Clone() as Pieces[,];
                            //Puts the old position to null;
                            queenPromotion[piece.gridPosition.x, piece.gridPosition.y] = null;

                            Pieces newQueen;

                            if (isWhite)
                            {
                                newQueen = GameBoard.Instance.SetPieceAtLocation(move, 1);
                            }
                            else newQueen = GameBoard.Instance.SetPieceAtLocation(move, -1);

                            newQueen.isWhite = isWhite;
                            newQueen.gridPosition = move;
                            queenPromotion[move.x, move.y] = newQueen;

                            int queenScore = SearchingMethod(queenPromotion, pDepth - 1, !whiteMove);

                            Destroy(newQueen.gameObject);

                            finalScore = queenScore;
                            if (pDepth == depth)
                            {

                                //Change so only top part can add to best move
                                if ((queenScore > i && isWhite) || (queenScore < i && !isWhite))
                                {
                                    i = queenScore;
                                    bestGridPosition.Clear();
                                    bestPieceToMove.Clear();

                                    bestGridPosition.Add(move);
                                    bestPieceToMove.Add(newQueen);

                                    bestPieceToPromoteTo = newQueen;
                                }

                                if (queenScore == i)
                                {
                                    bestGridPosition.Add(move);
                                    bestPieceToMove.Add(piece);

                                    bestPieceToPromoteTo = newQueen;
                                }

                            }

                            
                            continue;

                        }
                        

                        //Has enpassented something
                        if (move.x != pawn.gridPosition.x && pawn.dieTroughEnPassent)
                        {
                            Pieces[,] enpassent = boardState.Clone() as Pieces[,];
                            //Puts the old position to null;

                            //Puts the old position to null;
                            enpassent[piece.gridPosition.x, piece.gridPosition.y] = null;
                            enpassent[move.x, move.y] = piece;

                            enpassent[move.x, move.y - 1] = null;

                            int scoreEnpassent = SearchingMethod(enpassent, pDepth - 1, !whiteMove);
                            finalScore = scoreEnpassent;
                            if (pDepth == depth)
                            {
                                //Change so only top part can add to best move
                                if ((scoreEnpassent > i && isWhite) || (scoreEnpassent < i && !isWhite))
                                {
                                    i = scoreEnpassent;
                                    bestGridPosition.Clear();
                                    bestPieceToMove.Clear();


                                    bestGridPosition.Add(move);
                                    bestPieceToMove.Add(piece);
                                }

                                if (scoreEnpassent == i)
                                {
                                    bestGridPosition.Add(move);
                                    bestPieceToMove.Add(piece);
                                }

                            }

                            continue;

                        }

                    }
                    

                    

                    Pieces[,] newBoardState = boardState.Clone() as Pieces[,];

                   
                    //Puts the old position to null;
                    newBoardState[piece.gridPosition.x, piece.gridPosition.y] = null;
                    newBoardState[move.x, move.y] = piece;


                    bool pawnNotMovedYet = false ;
                    
                    if (piece is Pawns)
                    {
                        Pawns pawn = (Pawns)piece;
                        if (pawn.notMoved)
                        {
                            pawnNotMovedYet = true;
                            pawn.notMoved = false;
                            Debug.Log("fea");
                        }
                    }

                    int score = SearchingMethod(newBoardState, pDepth - 1, !whiteMove);

                    if (piece is Pawns)
                    {
                        Pawns pawn = (Pawns)piece;
                        if (pawnNotMovedYet)
                        {
                            pawn.notMoved = true;
                        }
                    }

                    //    if (pDepth == depth)
                    {
                        //Change so only top part can add to best move
                        if ((score > finalScore && isWhite) || (score < finalScore && !isWhite))
                        {
                          //  i = score;
                            finalScore = score;
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
                        
                    }
                }

                



            }
        }


        return finalScore;



    }
    */


    public override int SearchingMethod(Pieces[,] boardState, int pDepth, bool whiteMove)
    {

        foreach(Pieces piece in boardState)
        {
            piece.GetPseudoLegalMoves();



        }
        return 0;

    }




    private void DoMove()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
