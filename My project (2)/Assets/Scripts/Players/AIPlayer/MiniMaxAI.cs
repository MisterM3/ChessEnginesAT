using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAI : AbstractAIPlayer
{


    [SerializeField] protected ColourChessSide side = ColourChessSide.Unassigned;

    int score = 0;

    ChessBoard bestBoard;

    int finalScore = 0;

    public void Start()
    {
        if (side == ColourChessSide.White) finalScore = int.MinValue;
        if (side == ColourChessSide.Black) finalScore = int.MaxValue;
    }


    //Evaluates for which side it's on
    public override int EvaluateBoard(ChessBoard boardState)
    {

        int score = 0;


        foreach (Pieces piece in boardState.GetChessBoard())
        {
            if (piece == null) continue;

            if (piece.colourPiece == ColourChessSide.White)
                score += piece.GetValuePiece();

            if (piece.colourPiece == ColourChessSide.Black)
                score -= piece.GetValuePiece();

            

        }

        if (side == ColourChessSide.White)
            score *= -1;

        return score;
    }

    int amount = 0;

    /*
    public override void MovePiece()
    {
        float depthAmount = depth;

        List<ChessBoard> checkedBoards = new List<ChessBoard>();

        List<ChessBoard> boardsToCheck = new List<ChessBoard>();

        checkedBoards.Add(GameBoard.Instance.chessBoardPositions);

        while (depthAmount > 0)
        {

            



            boardsToCheck = new List<ChessBoard>(checkedBoards);
            checkedBoards = new List<ChessBoard>();

            foreach (ChessBoard boardToCheck in boardsToCheck)
            {
                Debug.Log("test");
                foreach (Pieces piece in boardToCheck.GetChessBoard())
                {
                    if (piece == null) continue;
                    if (piece.colourPiece != side) continue;

                    List<Vector2Int> moves = piece.GetPseudoLegalMoves();

                    if (moves == null || moves.Count == 0) continue;
                    foreach (Vector2Int move in moves)
                    {
                        Vector2Int oldGridPosition = piece.gridPosition;

                        ChessBoard newBoard = boardToCheck.CopyBoard();

                        Pieces copiedPiece = newBoard.GetPieceFromPosition(oldGridPosition);
                        newBoard.SetPieceAtPosition(move, copiedPiece);
                        newBoard.SetPieceAtPosition(oldGridPosition, null);

                        checkedBoards.Add(newBoard);
                        Debug.Log("test");

                    }
                }

            }

            depthAmount--;
        }


        Debug.Log(checkedBoards.Count);

        
    }

    */



    public override void MovePiece()
    {
        score = 0;
        bestBoard = null;
        amount = 0;
        SearchingMethod(GameBoard.Instance.chessBoardPositions, depth, side);


        if (bestBoard == null)
        {
            Debug.LogError("Error no board found");
            return; ;
        }

        Debug.Log($"Amount of moves: {amount}");


        GameBoard.Instance.ChangeBoard(bestBoard);
    }

    

    public override int SearchingMethod(ChessBoard boardState, int pDepth, ColourChessSide side)
    {

        if (pDepth == 0) return EvaluateBoard(boardState);

        int max = int.MinValue;

        foreach (Pieces piece in boardState.GetChessBoard())
        {
            if (piece == null) continue;
            
            //Pieces allowed to move in current depth (ReWrite)
            if (piece.colourPiece != side) continue;




            List<Vector2Int> moves = piece.GetPseudoLegalMoves();

            if (moves == null || moves.Count == 0) continue;
            foreach (Vector2Int move in moves)
            {

                ChessBoard newBoard = GetNewBoard(piece, boardState, move);

                amount++;

                if (side == ColourChessSide.White) score = -SearchingMethod(newBoard, pDepth - 1, ColourChessSide.Black);
                else if (side == ColourChessSide.Black) score = -SearchingMethod(newBoard, pDepth - 1, ColourChessSide.White);

                //Debug.Log(score);

                if (score > max)
                {
                    max = score;
                    Debug.Log(score);

                    if (pDepth == depth)
                    {
                        bestBoard = newBoard.CopyBoard();
                    }
                }

                //Testing
                if (score == max && pDepth == depth)
                {
                     int randomRange = Random.Range(0, 101);

                  //  Debug.Log(randomRange);
                    if (randomRange <= 50)
                    {
                        bestBoard = newBoard.CopyBoard();
                    }
                }
            }
        }

        return max;
    }


    private ChessBoard GetNewBoard(Pieces piece, ChessBoard oldBoard, Vector2Int move)
    {
        Vector2Int oldGridPosition = piece.gridPosition;



        ChessBoard newBoard = oldBoard.CopyBoard();

        Pieces copiedPiece = newBoard.GetPieceFromPosition(oldGridPosition);


        //Sets moved to false
        if (copiedPiece is ISpecialFirstMove)
        {
            ISpecialFirstMove movePiece = (ISpecialFirstMove)copiedPiece;
            movePiece.FirstMove();

            copiedPiece = (Pieces)movePiece;
        }
        newBoard.SetPieceAtPosition(move, copiedPiece);
        newBoard.SetPieceAtPosition(oldGridPosition, null);

       
        return newBoard;
    }
    /*
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
    */


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







    private void DoMove()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
