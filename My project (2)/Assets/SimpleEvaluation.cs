using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEvaluation : MonoBehaviour, IEvaluation
{
    public int Evaluate(Pieces[,] boardToEvaluate)
    {
        int score = 0;


        foreach(Pieces piece in boardToEvaluate)
        {
            #region CHESSPIECEVALUES

            if (piece is Queen)
            {
                if (piece.isWhite)
                {
                    score += 9;
                    continue;
                }

                score -= 9;
                continue;
            }

            if (piece is Rook)
            {
                if (piece.isWhite)
                {
                    score += 5;
                    continue;
                }

                score -= 5;
                continue;
            }

            if (piece is Bishop)
            {
                if (piece.isWhite)
                {
                    score += 3;
                    continue;
                }

                score -= 3;
                continue;
            }

            if (piece is Knight)
            {
                if (piece.isWhite)
                {
                    score += 3;
                    continue;
                }

                score -= 3;
                continue;
            }

            if (piece is Pawns)
            {
                if (piece.isWhite)
                {
                    score += 1;
                    continue;
                }

                score -= 1;
                continue;
            }

            if (piece is King)
            {
                if (piece.isWhite)
                {
                  //  score += int.MaxValue;
                    continue;
                }

               // score -= int.MinValue;
                continue;
            }

            #endregion
        }

        Debug.LogWarning(score);
        return score;
    }
}
