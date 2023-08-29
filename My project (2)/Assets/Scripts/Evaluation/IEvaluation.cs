using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IEvaluation
{
    int Evaluate(ChessBoard boardToEvaluate);
}
