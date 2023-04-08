using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IEvaluation
{
    int Evaluate(Pieces[,] boardToEvaluate);
}
