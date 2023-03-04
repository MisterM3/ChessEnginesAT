using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAI : AbstractAIPlayer
{

    [SerializeField] protected bool isWhite = false;

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

        Debug.Log(amount);
        GameBoard.Instance.SetPieceAtLocation(bestPieceToMove[random].gridPosition, null);
        bestPieceToMove[random].SetGridPosition(bestGridPosition[random]);
        GameBoard.Instance.SetPieceAtLocation(bestGridPosition[random], bestPieceToMove[random]);

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
                    }
                }
            }
        }


        return finalScore;



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
