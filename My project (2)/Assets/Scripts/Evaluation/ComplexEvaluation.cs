using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexEvaluation : MonoBehaviour, IEvaluation
{




    public int Evaluate(ChessBoard boardToEvaluate)
    {

        int score = 0;
        int scoreMoves = 0;
        int scoreMiddleAttack = 0;


        foreach (Pieces piece in boardToEvaluate.GetChessBoard())
        {
            if (piece == null) continue;

            List<Vector2Int> pseudoMoves = piece.GetPseudoLegalMoves();

            if (piece.colourPiece == ColourChessSide.White)
            {
                score += piece.GetValuePiece();



                //Maybe also put in seperate amount and tweak division
                if (pseudoMoves.Contains(new Vector2Int(3, 3))) scoreMiddleAttack++;
                if (pseudoMoves.Contains(new Vector2Int(3, 4))) scoreMiddleAttack++;
                if (pseudoMoves.Contains(new Vector2Int(4, 3))) scoreMiddleAttack++;
                if (pseudoMoves.Contains(new Vector2Int(4, 4))) scoreMiddleAttack++;

                scoreMoves += pseudoMoves.Count;
            }
            if (piece.colourPiece == ColourChessSide.Black)
            {
                score -= piece.GetValuePiece();
                scoreMoves -= piece.GetPseudoLegalMoves().Count;

                if (pseudoMoves.Contains(new Vector2Int(3, 3))) scoreMiddleAttack--;
                if (pseudoMoves.Contains(new Vector2Int(3, 4))) scoreMiddleAttack--;
                if (pseudoMoves.Contains(new Vector2Int(4, 3))) scoreMiddleAttack--;
                if (pseudoMoves.Contains(new Vector2Int(4, 4))) scoreMiddleAttack--;
            }


            


        }

        scoreMoves /= 4;

        scoreMiddleAttack /= 6;

        score += scoreMoves;
        score += scoreMiddleAttack;

        return score;
    }

}
