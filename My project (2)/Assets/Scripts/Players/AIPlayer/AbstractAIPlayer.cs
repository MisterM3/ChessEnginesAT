using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAIPlayer : AbstractPlayer
{
    public int evaluationMethodStragety;


    //both connected (look into dictionary)
    protected List<Vector2Int> bestGridPosition;

    protected List<Pieces> bestPieceToMove;

    [SerializeField] protected int depth = 1;
    public void Awake()
    {
        bestGridPosition = new List<Vector2Int>();
        bestPieceToMove = new List<Pieces>();
    }



    //Looks horrendious remake if possible
    public abstract int SearchingMethod(Pieces[,] boardState, int depth, bool WhiteMove);

    public abstract int EvaluateBoard(Pieces[,] boardState);

}
