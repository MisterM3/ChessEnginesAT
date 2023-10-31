using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IEvaluation))]
public class MiniMaxAI : AbstractAIPlayer
{


   // [SerializeField] protected ColourChessSide side = ColourChessSide.Unassigned;

    int score = 0;

    ChessBoard bestBoard;

    ChessBoard testBoard;

    int finalScore = 0;

    IEvaluation evaluation;


    public bool pruning = false;

    public void Start()
    {
        if (side == ColourChessSide.White) finalScore = int.MinValue;
        if (side == ColourChessSide.Black) finalScore = int.MaxValue;

        evaluation = GetComponent<IEvaluation>();

    }


    //Evaluates for which side it's on
    public override int EvaluateBoard(ChessBoard boardState)
    {
        int score = evaluation.Evaluate(boardState);



        return score;
    }

    


    int amount = 0;





    public override void MovePiece()
    {
        score = 0;

       // bestBoard = null;
        amount = 0;

        float timeBefore = Time.realtimeSinceStartup;
        
        score = SearchingMethod(GameBoard.Instance.chessBoardPositions, depth, side, out bestBoard);

        float timeAfter = Time.realtimeSinceStartup;

        float timeTotal = timeAfter - timeBefore;

        if (bestBoard == null && testBoard == null)
        {
            Debug.LogError("Error no board found");
            return; 
        }

        Debug.Log($"Amount of moves: {amount}");
        Debug.Log($"Best score: {score}");

        //  GameBoard.Instance.ChangeBoard(bestBoard);
        GameBoard.Instance.ChangeBoard(testBoard);


        SaveData.Instance.SaveDataToFile(GameStateManager.Instance.GetTurn(), amount, timeTotal, side);
    }


    public override int SearchingMethod(ChessBoard boardState, int pDepth, ColourChessSide side, out ChessBoard bestBoard)
    {
        return SearchingMethod(boardState, pDepth, side, int.MinValue, int.MaxValue, out bestBoard);
    }


    public int SearchingMethod(ChessBoard boardState, int pDepth, ColourChessSide pSide, int alpha, int beta, out ChessBoard bestBoard)
    {

       // Debug.Log(depth);

        bestBoard = null;
        ChessBoard newBestBoard = null;

        if (pDepth == 0)
        {
            bestBoard = boardState;


            if (pSide == ColourChessSide.White)
            {
                return EvaluateBoard(boardState);
            }
            else return -1 * EvaluateBoard(boardState);
            //else return -1 * EvaluateBoard(boardState);

        }




        List<ChessBoard> childBoardNodes = new();

        int value = int.MinValue;

        //Get ChildNodes
        foreach (Pieces piece in boardState.GetChessBoard())
        {
            if (piece == null) continue;
            
            //Pieces allowed to move in current depth (ReWrite)
            if (piece.colourPiece != pSide) continue;

            List<Vector2Int> moves = piece.GetLegalMoves();

            if (moves == null || moves.Count == 0) continue;

            foreach (Vector2Int move in moves)
            {
                ChessBoard newBoard = GetNewBoard(piece, boardState, move);

                
                //Promotions
                if (piece is Pawns)
                {

                    if ((move.y == 7 && piece.colourPiece == ColourChessSide.White) || (move.y == 0 && piece.colourPiece == ColourChessSide.Black))
                    {
                        ChessBoard QueenPromotion = newBoard.CopyBoard();
                        Queen queen = new Queen();
                        queen.colourPiece = piece.colourPiece;
                        QueenPromotion.SetPieceAtPosition(move, queen);

                        ChessBoard BishopPromotion = newBoard.CopyBoard();
                        Bishop bishop = new Bishop();
                        bishop.colourPiece = piece.colourPiece;
                        BishopPromotion.SetPieceAtPosition(move, bishop);

                        ChessBoard RookPromotion = newBoard.CopyBoard();
                        Rook rook = new Rook();
                        rook.colourPiece = piece.colourPiece;
                        RookPromotion.SetPieceAtPosition(move, bishop);

                        ChessBoard KnightPromotion = newBoard.CopyBoard();
                        Knight knight = new Knight();
                        knight.colourPiece = piece.colourPiece;
                        KnightPromotion.SetPieceAtPosition(move, bishop);

                        childBoardNodes.Add(QueenPromotion);
                        childBoardNodes.Add(BishopPromotion);
                        childBoardNodes.Add(RookPromotion);
                        childBoardNodes.Add(KnightPromotion);

                        continue;
                    }

                }
                
                childBoardNodes.Add(newBoard);

            }
            

            

            List<ChessBoard> castling = boardState.GetCastlingMoves(pSide);

            if (castling != null && castling.Count != 0)
            {
                foreach (ChessBoard board in castling)
                {
                    childBoardNodes.Add(board);
                }
            }

            

        }


        
        //Terminal node (maybe change to have full other side)
        if (childBoardNodes == null || childBoardNodes.Count == 0)
        {
            newBestBoard = boardState;


            if (pSide == ColourChessSide.White)
            {
                return EvaluateBoard(boardState);
            }
            else return -1 *  EvaluateBoard(boardState);
            
        }


        //  Debug.Log(amount);
        // Debug.Log(childBoardNodes.Count);
        // Debug.Log(depth);

        //Use childnodes for new nodes


        List<ChessBoard> bestChessBoards = new();

        foreach (ChessBoard board in childBoardNodes)
        {

            ChessBoard newBoard = board.CopyBoard();

            ColourChessSide sideNewBoard;

            amount++;

            if (pSide == ColourChessSide.White) sideNewBoard = ColourChessSide.Black;
            else if (pSide == ColourChessSide.Black) sideNewBoard = ColourChessSide.White;
            else sideNewBoard = ColourChessSide.Unassigned;

           // Debug.Log(sideNewBoard);

            int newValue = -SearchingMethod(newBoard, pDepth - 1, sideNewBoard, -beta, -alpha, out newBestBoard);
            

            //value = Mathf.Max(value, -SearchingMethod(newBoard, depth - 1, sideNewBoard, -beta, -alpha, out bestBoard));

            if (newValue > value)
            {
                newBestBoard = newBoard;
                value = newValue;
                bestChessBoards.Clear();

                if (pDepth == depth) testBoard = newBestBoard;
            }

            if (newValue == value)
            {
                bestChessBoards.Add(newBoard);
            }

            


            if (pruning)
            {
                alpha = Mathf.Max(alpha, value);

                if (alpha >= beta) break;
            }
        }

        if (pDepth == depth)
        {
            if (bestChessBoards.Count != 0)
            {
                testBoard = bestChessBoards[Random.Range(0, bestChessBoards.Count)];
            }
        }

        return value;
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
   

}
