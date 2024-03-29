using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAIPlayer : AbstractPlayer
{
    public IEvaluation evaluationMethodStragety;

    [SerializeField] protected ColourChessSide side = ColourChessSide.Unassigned;

    //both connected (look into dictionary)
    protected List<Vector2Int> bestGridPosition;

    protected List<Pieces> bestPieceToMove;

    protected Pieces bestPieceToPromoteTo;

    [SerializeField] protected int depth = 1;
    public void Awake()
    {
        bestGridPosition = new List<Vector2Int>();
        bestPieceToMove = new List<Pieces>();

        evaluationMethodStragety = GetComponent<IEvaluation>();
    }



    //Looks horrendious remake if possible
    public abstract int SearchingMethod(ChessBoard boardState, int depth, ColourChessSide side, out ChessBoard bestBoard);


    public abstract int EvaluateBoard(ChessBoard boardState);

}
