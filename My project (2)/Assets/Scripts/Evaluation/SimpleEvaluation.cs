using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEvaluation : MonoBehaviour, IEvaluation
{
    public int Evaluate(ChessBoard boardToEvaluate)
    {

        int score = 0;
        int scoreMoves = 0;


        foreach (Pieces piece in boardToEvaluate.GetChessBoard())
        {
            if (piece == null) continue;

            if (piece.colourPiece == ColourChessSide.White)
            {
                score += piece.GetValuePiece();
                scoreMoves += piece.GetPseudoLegalMoves().Count;
            }
            if (piece.colourPiece == ColourChessSide.Black)
            {
                score -= piece.GetValuePiece();
                scoreMoves -= piece.GetPseudoLegalMoves().Count;
            }


        }

        scoreMoves /= 4;

        score += scoreMoves;

        return score;
    }

}
