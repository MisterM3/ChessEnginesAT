using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawns : Pieces
{
    public bool notMoved = true;

    public bool enPassantAble = false;

    public bool dieTroughEnPassent = false;

    // Start is called before the first frame update
    void Start()
    {
        GameBoard.Instance.SetPieceAtLocation(gridPosition, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<Vector2Int> GetPseudoLegalMoves(Vector2Int gridPoint)
    {

        if (isWhite) moveDirections.Add(new Vector2Int(0, 1));
        else moveDirections.Add(new Vector2Int(0, -1));




        List<Vector2Int> movePositions = new List<Vector2Int>();

        try
        {

            //FirstMoveDouble
            if (notMoved && !GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0] * 2) && !GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0]))
            {
                
                movePositions.Add(gridPosition + moveDirections[0] * 2);
            }

            //Top
            if (!GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0]))
            {
                movePositions.Add(gridPosition + moveDirections[0]);
            }


            //Left Capture
            if (GameBoard.Instance.IsOtherSidePieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(-1, 0), isWhite))
            {
                movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(-1, 0));
            }

            //Right Capture
            if (GameBoard.Instance.IsOtherSidePieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(1, 0), isWhite))
            {
                movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(1, 0));

            }

            //Enpassant
            if (GameBoard.Instance.TryGetPieceAtLocation(gridPosition + new Vector2Int(1, 0), out Pieces piece) && !GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(1,0)))
            {

                //Check if enpassantable
                if (piece is Pawns)
                {
                    Pawns pawn = (Pawns)piece;

                    if (pawn.enPassantAble)
                    {
                        Debug.Log("en");
                        dieTroughEnPassent = true;
                        movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(1, 0));
                    }
                }
            }



            if (GameBoard.Instance.TryGetPieceAtLocation(gridPosition + new Vector2Int(-1, 0), out Pieces piece2) && !GameBoard.Instance.IsPieceAtLocation(gridPosition + moveDirections[0] + new Vector2Int(-1, 0)))
            {

                //Check if enpassantable
                if (piece2 is Pawns)
                {
                    Pawns pawn = (Pawns)piece2;

                    if (pawn.enPassantAble)
                    {
                        Debug.Log("en");
                        dieTroughEnPassent = true;
                        movePositions.Add(gridPosition + moveDirections[0] + new Vector2Int(-1, 0));
                    }
                }
            }
        }
        catch(System.Exception e)
        {
          //  Debug.Log(e);
        }


        return movePositions;
    }

    public bool TryPromotion(Vector2Int movePosition)
    {

        if ((movePosition.y == 7 && isWhite) || (movePosition.y == 0 && !isWhite)) return true;

        return false;
    }


    public override void SetGridPosition(Vector2Int newPosition)
    {
        base.SetGridPosition(newPosition);

        if (enPassantAble) enPassantAble = false;

        if (!notMoved) return;

        notMoved = false;
        if ((newPosition.y == 3 && isWhite) || (newPosition.y == 4 && !isWhite)) enPassantAble = true;
    }

    public override List<Vector2Int> ConvertPseudoToLegalMoves(List<Vector2Int> pseudoLegalMoves)
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();

        if (pseudoLegalMoves.Count == 0) return legalMoves;

        bool passent = false;

        foreach (Vector2Int move in pseudoLegalMoves)
        {

            //   Pieces[,] newBoardState = GameBoard.Instance.chessBoardPositions.Clone() as Pieces[,];

            Vector2Int oldGridPosition = this.gridPosition;

            GameBoard.Instance.TryGetPieceAtLocation(move, out Pieces oldPiece);

            //Has enpassent
            if (oldPiece == null && move.x != oldGridPosition.x)
            {
                Vector2Int enpassentGridPosition = new Vector2Int(move.x, oldGridPosition.y);
                GameBoard.Instance.TryGetPieceAtLocation(enpassentGridPosition, out Pieces enPassentPieced);
                oldPiece = enPassentPieced;
                Debug.Log("efaen");
                passent = true;
            }


            GameBoard.Instance.SetPieceAtLocation(oldGridPosition, null, false);

            GameBoard.Instance.SetPieceAtLocation(move, this, false);


            //  newBoardState[this.gridPosition.x, this.gridPosition.y] = null;
            //  newBoardState[move.x, move.y] = this;

            foreach (Pieces piece in GameBoard.Instance.chessBoardPositions)
            {
                if (piece is King)
                {
                    King king = (King)piece;

                    if ((king.isWhite == isWhite) && !king.InCheck()) legalMoves.Add(move);
                }
            }

            GameBoard.Instance.SetPieceAtLocation(oldGridPosition, this, false);

            if (passent == true)
            {
                Vector2Int enpassentGridPosition = new Vector2Int(move.x, oldGridPosition.y);
                GameBoard.Instance.SetPieceAtLocation(enpassentGridPosition, oldPiece, false);
                Debug.Log("efaen");
            }
            else GameBoard.Instance.SetPieceAtLocation(move, oldPiece, false);

            //GameBoard.Instance.SetPieceAtLocation(move, oldPiece, false);
        }

        return legalMoves;
    }
}
