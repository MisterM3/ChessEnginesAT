using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : AbstractPlayer
{

    Pieces activePiece = null;

    public GameBoard board;

    public ChessBoardVisual visual;

    [SerializeField] protected bool isWhite;


    // Start is called before the first frame update
    void Start()
    {
        visual = GameObject.FindObjectOfType<ChessBoardVisual>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && GameStateManager.Instance.IsWhiteTurn() == isWhite)
        {
            MovePiece();
        }



    }


    public override void MovePiece()
    {
        Vector2Int mousePosition = MouseRay.GetMouseGridPosition();

        if (activePiece == null)
        {
            Pieces piece = null;

            if (board.TryGetSamePieceAtLocation(mousePosition, isWhite, out piece))
            {
                activePiece = piece;
                visual.ActivateMoveVisuals(activePiece.MoveLocations(activePiece.gridPosition));
                Debug.Log("true");
            }

            return;
        }

        //If a piece is selected

        List<Vector2Int> positions = activePiece.MoveLocations(activePiece.gridPosition);


        foreach (Vector2Int position in positions)
        {
            if (position == mousePosition)
            {
                board.SetPieceAtLocation(activePiece.gridPosition, null);
                activePiece.SetGridPosition(position);
                board.SetPieceAtLocation(position, activePiece);
                Debug.Log("true2");
                visual.DeActivateAllMoveVisual();

                activePiece = null;

                GameStateManager.Instance.NextTurn();
                return;
            }
        }
        visual.DeActivateAllMoveVisual();
        activePiece = null;
    }
}



