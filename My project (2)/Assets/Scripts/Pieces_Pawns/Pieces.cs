using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColourChessSide { White, Black}


public abstract class Pieces : MonoBehaviour
{


    //Directions to move (usefull for all except knight/horse)
    protected List<Vector2Int> moveDirections;

    //Max Amount of spaces move (8 except king, knight and pawn)
    protected int maxMoveAmount = 8;


    public ChessBoard board;

    public Vector2Int gridPosition;

    

    public ColourChessSide colourPiece;


    public void Awake()
    {
        moveDirections = new List<Vector2Int>();
    }

    public virtual List<Vector2Int> GetPseudoLegalMoves()
    {
        List<Vector2Int> movePositions = new List<Vector2Int>();

        if (moveDirections == null)
        {
            Debug.LogError($"{name} has no Directions to move to");
            return null;
        }


        foreach (Vector2Int direction in moveDirections)
        {
            for (int i = 1; i <= maxMoveAmount; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * direction.x), gridPosition.y + (i * direction.y));


                //Piece is outside board, so any spot further than this one will also be outside of the board
                if (!IsInsideBoard(nextGridPoint)) break;

                try
                {

                    //Gets the piece at the position to check later on what to do with it
                    Pieces pieceAtCheckingPosition = board.GetPieceFromPosition(nextGridPoint);

                    //No piece already at position, so it's free to move to
                    if (pieceAtCheckingPosition == null)
                    {
                        movePositions.Add(nextGridPoint);
                        continue;
                    }

                    //Piece is of opposite colour and can be taken, therefore the piece can move there
                    if (pieceAtCheckingPosition.colourPiece != this.colourPiece)
                    {
                        movePositions.Add(nextGridPoint);
                        continue;
                    }

                    //Piece is the same colour as this piece so we can't go any further and can check in different direction
                    break;


                }
                catch (System.Exception e) { Debug.LogError(e); }
            }
        }

        //Returns all pseudoPositions it can move to
        return movePositions;

    }

    protected bool IsInsideBoard(Vector2Int gridPosition)
    {
        if (gridPosition.x >= ChessBoard.SIZE) return false;
        if (gridPosition.y >= ChessBoard.SIZE) return false;
        if (gridPosition.x < 0) return false;
        if (gridPosition.y < 0) return false;


        return true;
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
        return ConvertPseudoToLegalMoves(GetPseudoLegalMoves());
    }


    public virtual void SetGridPosition(Vector2Int newPosition)
    {
        gridPosition = newPosition;
        this.gameObject.transform.position = new Vector3(newPosition.x, 0, newPosition.y);
    }


}
