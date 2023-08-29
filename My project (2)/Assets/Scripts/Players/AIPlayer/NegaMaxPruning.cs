using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IEvaluation))]
public class NegaMaxPruning : AbstractAIPlayer
{


    [SerializeField] protected ColourChessSide side = ColourChessSide.Unassigned;

    int score = 0;

    ChessBoard bestBoard;

    int finalScore = 0;

    IEvaluation evaluation;

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

     //   if (side == ColourChessSide.White)
       //     score *= -1;

        return score;
    }

    


    int amount = 0;



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

        if (pDepth == 0)
        {
            if (side == ColourChessSide.White)
            {
                return EvaluateBoard(boardState);
            }
            else return -1 * EvaluateBoard(boardState);

        }

        int max = int.MinValue;

        foreach (Pieces piece in boardState.GetChessBoard())
        {
            if (piece == null) continue;
            
            //Pieces allowed to move in current depth (ReWrite)
            if (piece.colourPiece != side) continue;




            //List<Vector2Int> moves = piece.GetPseudoLegalMoves();

            List<Vector2Int> moves = piece.GetLegalMoves();

            //Castling+Promotions
            List<ChessBoard> specialBoards = new List<ChessBoard>();

            if (moves == null || moves.Count == 0) continue;
            foreach (Vector2Int move in moves)
            {

                ChessBoard newBoard = GetNewBoard(piece, boardState, move);

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

                        specialBoards.Add(QueenPromotion);
                        specialBoards.Add(BishopPromotion);
                        specialBoards.Add(RookPromotion);
                        specialBoards.Add(KnightPromotion);


                    }
                }


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
                    if (randomRange >= 50)
                    {
                        bestBoard = newBoard.CopyBoard();
                    }
                }
            }


            List<ChessBoard> castling = boardState.GetCastlingMoves(side);

            if (castling != null && castling.Count != 0)
            {
                foreach (ChessBoard board in castling)
                {
                    specialBoards.Add(board);
                }
            }

            if (specialBoards == null || specialBoards.Count == 0) continue;

            foreach(ChessBoard board in specialBoards)
            {
                ChessBoard newBoard = board.CopyBoard();

                amount++;

                if (side == ColourChessSide.White) score = -SearchingMethod(newBoard, pDepth - 1, ColourChessSide.Black);
                else if (side == ColourChessSide.Black) score = -SearchingMethod(newBoard, pDepth - 1, ColourChessSide.White);


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
                    if (randomRange >= 50)
                    {
                        bestBoard = newBoard.CopyBoard();
                    }
                }
            }
        }

        return score;
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
