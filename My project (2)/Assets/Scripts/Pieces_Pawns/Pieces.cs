using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pieces : MonoBehaviour
{


    //Directions to move (usefull for all except knight/horse)
    protected List<Vector2Int> moveDirections;

    //Max Amount of spaces move (8 except king, knight and pawn)
    protected int maxMoveAmount = 8;

    public Vector2Int gridPosition;

    public bool isWhite = false;



    public void Awake()
    {
        moveDirections = new List<Vector2Int>();
    }

    public virtual List<Vector2Int> GetPseudoLegalMoves(Vector2Int gridPoint)
    {
        List<Vector2Int> movePositions = new List<Vector2Int>();

        if (moveDirections == null)
        {
            Debug.LogError($"{name} has no Directions to move to");
            return null;
        }

        
        foreach(Vector2Int direction in moveDirections)
        {
            for(int i = 1; i < maxMoveAmount + 1; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + (i * direction.x), gridPoint.y + (i * direction.y));

                if (gridPoint.x + (i * direction.x) > 7) continue;
                if (gridPoint.y + (i * direction.y) > 7) continue;
                if (gridPoint.x + (i * direction.x) < 0) continue;
                if (gridPoint.y + (i * direction.y) < 0) continue;

                try
                {


                    //Test if a piece is on the grid (last point)
                    if (GameBoard.Instance.IsSameSidePieceAtLocation(nextGridPoint, isWhite))
                    {
                        break;
                    }


                }
                catch(System.Exception e)
                {

                }

                movePositions.Add(nextGridPoint);



                try
                {
                    //Test if a piece is on the grid (last point)
                    if (GameBoard.Instance.IsOtherSidePieceAtLocation(nextGridPoint, isWhite))
                    {
                        break;
                    }

                }
                catch (System.Exception e)
                {

                }
            }
        }

        return movePositions;
    }


    public virtual List<Vector2Int> ConvertPseudoToLegalMoves(List<Vector2Int> pseudoLegalMoves)
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();

        if (pseudoLegalMoves.Count == 0) return legalMoves;

        foreach(Vector2Int move in pseudoLegalMoves)
        {
         //   Pieces[,] newBoardState = GameBoard.Instance.chessBoardPositions.Clone() as Pieces[,];

            Vector2Int oldGridPosition = this.gridPosition;

            GameBoard.Instance.TryGetPieceAtLocation(move, out Pieces oldPiece);

            GameBoard.Instance.SetPieceAtLocation(oldGridPosition, null, false);

            GameBoard.Instance.SetPieceAtLocation(move, this, false);

          //  newBoardState[this.gridPosition.x, this.gridPosition.y] = null;
          //  newBoardState[move.x, move.y] = this;

            foreach(Pieces piece in GameBoard.Instance.chessBoardPositions)
            {
                if (piece is King)
                {
                    King king = (King)piece;

                    if ((king.isWhite == isWhite) && !king.InCheck()) legalMoves.Add(move);
                }
            }

            GameBoard.Instance.SetPieceAtLocation(oldGridPosition, this, false);
            GameBoard.Instance.SetPieceAtLocation(move, oldPiece, false);
        }

        return legalMoves;
    }

    public List<Vector2Int> GetLegalMoves(Vector2Int gridPoint)
    {
      //  List<Vector2Int> legalMoves = ConvertPseudoToLegalMoves(GetPseudoLegalMoves(gridPoint));

      //  if (legalMoves.Count == 0) Debug.LogError("wot");
        return ConvertPseudoToLegalMoves(GetPseudoLegalMoves(gridPoint));
     //   return legalMoves;
    }


    public virtual void SetGridPosition(Vector2Int newPosition)
    {
        gridPosition = newPosition;
        this.gameObject.transform.position = new Vector3(newPosition.x, 0, newPosition.y);
    }


}
