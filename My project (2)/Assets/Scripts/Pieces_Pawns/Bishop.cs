using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Pieces
{

    ChessBoardVisual chess;
    // Start is called before the first frame update
    void Start()
    {

        chess = GameObject.FindObjectOfType<ChessBoardVisual>();

        moveDirections.Add(new Vector2Int(-1, -1));
        moveDirections.Add(new Vector2Int(1, 1));
        moveDirections.Add(new Vector2Int(1, -1));
        moveDirections.Add(new Vector2Int(-1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        chess.ActivateMoveVisuals(MoveLocations(new Vector2Int(4, 4)));
    }
}
