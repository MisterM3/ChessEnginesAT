using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEvaluation : MonoBehaviour, IEvaluation
{
    public int Evaluate(ChessBoard boardToEvaluate)
    {

        return 1;
    }
}
