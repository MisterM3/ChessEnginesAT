using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexEvaluation : MonoBehaviour, IEvaluation
{
    public int Evaluate(ChessBoard boardToEvaluate)
    {

        int score = 0;
        int scoreMoves = 0;


        foreach (Pieces piece in boardToEvaluate.GetChessBoard())
        {
            if (piece == null) continue;

            List<Vector2Int> pseudoMoves = piece.GetPseudoLegalMoves();

            if (piece.colourPiece == ColourChessSide.White)
            {
                score += piece.GetValuePiece();

                //Maybe also put in seperate amount and tweak division
                if (pseudoMoves.Contains(new Vector2Int(3, 3))) scoreMoves++;
                if (pseudoMoves.Contains(new Vector2Int(3, 4))) scoreMoves++;
                if (pseudoMoves.Contains(new Vector2Int(4, 3))) scoreMoves++;
                if (pseudoMoves.Contains(new Vector2Int(4, 4))) scoreMoves++;

                scoreMoves += pseudoMoves.Count;
            }
            if (piece.colourPiece == ColourChessSide.Black)
            {
                score -= piece.GetValuePiece();
                scoreMoves -= piece.GetPseudoLegalMoves().Count;

                if (pseudoMoves.Contains(new Vector2Int(3, 3))) scoreMoves--;
                if (pseudoMoves.Contains(new Vector2Int(3, 4))) scoreMoves--;
                if (pseudoMoves.Contains(new Vector2Int(4, 3))) scoreMoves--;
                if (pseudoMoves.Contains(new Vector2Int(4, 4))) scoreMoves--;
            }


            


        }

        scoreMoves /= 4;

        score += scoreMoves;

        return score;
    }

}
