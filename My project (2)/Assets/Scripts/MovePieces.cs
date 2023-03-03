using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{

    Pieces activePiece = null;

    public GameBoard board;

    public ChessBoardVisual visual;

    // Start is called before the first frame update
    void Start()
    {
        visual = GameObject.FindObjectOfType<ChessBoardVisual>();
    }

    // Update is called once per frame
    void Update()
    {


        
        if (Input.GetMouseButtonDown(0))
        {

            Vector2Int mousePosition = MouseRay.GetMouseGridPosition();

            Debug.Log(mousePosition);
            if (activePiece == null)
            {
                Pieces piece = null;

                if (board.TryGetPieceAtLocation(mousePosition, out piece))
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

                    return;
                }
            }
            visual.DeActivateAllMoveVisual();
            activePiece = null;
        }



        
        
    }


}
