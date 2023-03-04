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




    public override void MovePiece()
    {
        bestPieceToMove.Clear();
        bestGridPosition.Clear();

        int number = SearchingMethod(GameBoard.Instance.chessBoardPositions, 1);

        int random = Random.Range(0, bestPieceToMove.Count);

        Debug.Log(bestPieceToMove.Count);
        GameBoard.Instance.SetPieceAtLocation(bestPieceToMove[random].gridPosition, null);
        bestPieceToMove[random].SetGridPosition(bestGridPosition[random]);
        GameBoard.Instance.SetPieceAtLocation(bestGridPosition[random], bestPieceToMove[random]);

        GameStateManager.Instance.NextTurn();
    }

    public override int SearchingMethod(Pieces[,] boardState, int depth)
    {

        int finalScore = 0;

        foreach (Pieces piece in boardState)
        {

            if (piece == null) continue;
            //Evaluate when depth has been reached
            if (depth <= 0) return 0;

            List<Vector2Int> moves = new();

            int i = 0;
            if (piece.isWhite == isWhite)
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

                    int score = SearchingMethod(newBoardState, depth - 1);

                    if (score > i)
                    {
                        score = i;
                        bestGridPosition.Clear();
                        bestPieceToMove.Clear();

                        bestGridPosition.Add(move);
                        bestPieceToMove.Add(piece);
                    }

                    if (score == 0)
                    {
                        bestGridPosition.Add(move);
                        bestPieceToMove.Add(piece);
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
