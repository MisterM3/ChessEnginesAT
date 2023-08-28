using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColourChessSide { White, Black, Unassigned}


public class PiecesS : MonoBehaviour { }

public abstract class Pieces
{


    //Directions to move (usefull for all except knight/horse)
    protected List<Vector2Int> moveDirections;

    //Max Amount of spaces move (8 except king, knight and pawn)
    protected int maxMoveAmount = 8;


    public ChessBoard board;

    public Vector2Int gridPosition;

    

    public ColourChessSide colourPiece;


    public Pieces()
    {


        Awake();
        Start();

    }


    public void Awake()
    {
        moveDirections = new List<Vector2Int>();
    }

    public virtual void Start() { }

    public virtual int GetValuePiece()
    {
        return 0;
    }


    public virtual List<Vector2Int> GetPseudoLegalMoves()
    {
        List<Vector2Int> movePositions = new List<Vector2Int>();

        if (moveDirections == null)
        {
            //Debug.LogError($"{name} has no Directions to move to");
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
                        //Was continue, have to break, as it's the last position to move to
                        break;
                    }

                    //Piece is the same colour as this piece so we can't go any further and can check in different direction
                    break;


                }
                catch (System.Exception e) { Debug.LogError(e); }
            }
        }


        //Debug.Log(movePositions[0]);
        //Returns all pseudoPositions it can move to
        return movePositions;

    }

    public virtual Pieces CopyPiece()
    {

        Debug.LogWarning("USED VIRTUAL METHOD");
        return null;
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

        for (int i = pseudoLegalMoves.Count - 1; i >= 0; i--)
        {
            Vector2Int move = pseudoLegalMoves[i];

            //Put on board
            ChessBoard newBoard = GetNewBoard(this, board, move);

            King king = newBoard.getKing(this.colourPiece);

            //Check if king is in check
            if (king.InCheck())
            {
                pseudoLegalMoves.RemoveAt(i);
            }

        }


        return pseudoLegalMoves;
    }


    private ChessBoard GetNewBoard(Pieces piece, ChessBoard oldBoard, Vector2Int move)
    {
        Vector2Int oldGridPosition = piece.gridPosition;



        ChessBoard newBoard = oldBoard.CopyBoard();

        Pieces copiedPiece = newBoard.GetPieceFromPosition(oldGridPosition);


        newBoard.SetPieceAtPosition(move, copiedPiece);
        newBoard.SetPieceAtPosition(oldGridPosition, null);


        return newBoard;
    }

    public List<Vector2Int> GetLegalMoves()
    {
       
        return ConvertPseudoToLegalMoves(GetPseudoLegalMoves());
    }


    public virtual void SetGridPosition(Vector2Int newPosition)
    {
        gridPosition = newPosition;
    }


}
