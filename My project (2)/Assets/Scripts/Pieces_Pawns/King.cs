using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Pieces
{

    public bool hasMoved = false;

    // Start is called before the first frame update
    void Start()
    {

        GameBoard.Instance.SetPieceAtLocation(gridPosition, this);

        maxMoveAmount = 1;
        moveDirections.Add(new Vector2Int(-1, -1));
        moveDirections.Add(new Vector2Int(1, 1));
        moveDirections.Add(new Vector2Int(1, -1));
        moveDirections.Add(new Vector2Int(-1, 1));
        moveDirections.Add(new Vector2Int(-1, 0));
        moveDirections.Add(new Vector2Int(1, 0));
        moveDirections.Add(new Vector2Int(0, -1));
        moveDirections.Add(new Vector2Int(0, 1));
    }


    public override List<Vector2Int> ConvertPseudoToLegalMoves(List<Vector2Int> pseudoLegalMoves)
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();

        if (pseudoLegalMoves.Count == 0) return legalMoves;

        foreach (Vector2Int move in pseudoLegalMoves)
        {
            //   Pieces[,] newBoardState = GameBoard.Instance.chessBoardPositions.Clone() as Pieces[,];

            Vector2Int oldGridPosition = this.gridPosition;

            GameBoard.Instance.TryGetPieceAtLocation(move, out Pieces oldPiece);



            GameBoard.Instance.SetPieceAtLocation(move, this, false);
            GameBoard.Instance.SetPieceAtLocation(oldGridPosition, null, false);

            this.gridPosition = move;
            //  newBoardState[this.gridPosition.x, this.gridPosition.y] = null;
            //  newBoardState[move.x, move.y] = this;

            if (!InCheck()) legalMoves.Add(move);

            this.gridPosition = oldGridPosition;
            GameBoard.Instance.SetPieceAtLocation(oldGridPosition, this, false);
            GameBoard.Instance.SetPieceAtLocation(move, oldPiece, false);
        }

        return legalMoves;
    } 

    //Rewrite
    public bool TryShortCastling()
    {

        if (hasMoved) return false;

        if (!GameBoard.Instance.TryGetSamePieceAtLocation(new Vector2Int(7, gridPosition.y), isWhite, out Pieces piece)) return false;
        
        if (!(piece is Rook)) return false;
        Debug.Log("test");

        //Has a rook at position
        Rook rook = (Rook)piece;

        if (rook.hasMoved) return false;


        //if (KingInCheck(white, position);
        for (int i = 1; i < 8; i++)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * 1), gridPosition.y);


            //Also stops before the rooks
            if (nextGridPoint.x >= 7) continue;
            if (nextGridPoint.x <= 0) continue;

            try
            {
                //Test if a piece is on the grid (last point)
                if (GameBoard.Instance.IsPieceAtLocation(nextGridPoint))
                {
                    return false;
                }

              // if (InCheck())


            }
            catch (System.Exception e)
            {

            }
        }

        return true;
    }

    public bool TryLongCastling()
    {


        if (hasMoved) return false;

        if (!GameBoard.Instance.TryGetSamePieceAtLocation(new Vector2Int(0, gridPosition.y), isWhite, out Pieces piece)) return false;

        if (!(piece is Rook)) return false;

        //Has a rook at position
        Rook rook = (Rook)piece;

        if (rook.hasMoved) return false;


        //if (KingInCheck(white, position);
        for (int i = 1; i < 8; i++)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * -1), gridPosition.y);


            //Also stops before the rooks
            if (nextGridPoint.x >= 7) continue;
            if (nextGridPoint.x <= 0) continue;

            try
            {
                //Test if a piece is on the grid (last point)
                if (GameBoard.Instance.IsPieceAtLocation(nextGridPoint))
                {
                    return false;
                }


            }
            catch (System.Exception e)
            {

            }
        }

        return true;
    }

    public bool InCheck()
    {
        try
        {
            //Checks for Queen, Rook and Bishop
            foreach (Vector2Int direction in moveDirections)
            {
                for (int i = 1; i < 9; i++)
                {
                    Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * direction.x), gridPosition.y + (i * direction.y));

                    if (nextGridPoint.x > 7) continue;
                    if (nextGridPoint.y > 7) continue;
                    if (nextGridPoint.x < 0) continue;
                    if (nextGridPoint.y < 0) continue;

                    Debug.Log(direction);
                    Debug.Log(i);
                    Debug.Log(nextGridPoint);

                    try
                    {
                        //Test if a piece is on the grid (last point)
                        if (GameBoard.Instance.IsSameSidePieceAtLocation(nextGridPoint, isWhite))
                        {
                               Debug.Log("test");
                            break;
                        }


                    }
                    catch (System.Exception e)
                    {

                    }

                    try
                    {
                        //Test if a piece is on the grid (last point)
                        if (GameBoard.Instance.TryGetOtherPieceAtLocation(nextGridPoint, isWhite, out Pieces piece))
                        {
                            Debug.Log("tst");
                            if (direction.x * direction.y == 0 && (piece is Rook || piece is Queen)) return true;
                            if (direction.x * direction.y != 0 && (piece is Bishop || piece is Queen)) return true;

                            //PawnCheck
                            if (((i * direction.x == -1 || i * direction.x == 1) && ((i * direction.y == 1 && isWhite) || i * direction.y == -1 && !isWhite)) && piece is Pawns) return true;
                            break;
                        }

                    }
                    catch (System.Exception e)
                    {

                    }
                }
            }

            //HorseChecks
            {
                List<Vector2Int> horseCheck = new List<Vector2Int>();

                horseCheck.Add(new Vector2Int(-1, -2));
                horseCheck.Add(new Vector2Int(-2, -1));
                horseCheck.Add(new Vector2Int(-2, 1));
                horseCheck.Add(new Vector2Int(2, 1));
                horseCheck.Add(new Vector2Int(2, -1));
                horseCheck.Add(new Vector2Int(-1, 2));
                horseCheck.Add(new Vector2Int(1, 2));
                horseCheck.Add(new Vector2Int(1, -2));

                foreach (Vector2Int horseMovements in horseCheck)
                {
                    Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + horseMovements.x, gridPosition.y + horseMovements.y);

                    if (nextGridPoint.x > 7) continue;
                    if (nextGridPoint.y > 7) continue;
                    if (nextGridPoint.x < 0) continue;
                    if (nextGridPoint.y < 0) continue;

                    if (GameBoard.Instance.TryGetOtherPieceAtLocation(nextGridPoint, isWhite, out Pieces piece))
                    {
                        if (piece is Knight) return true;
                    }
                }
            }

        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }


        return false;

    }

    public bool InCheck(Pieces[,] board)
    {
        try
        {
            //Checks for Queen, Rook and Bishop
            foreach (Vector2Int direction in moveDirections)
            {
                for (int i = 1; i < 9; i++)
                {
                    Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + (i * direction.x), gridPosition.y + (i * direction.y));

                    if (nextGridPoint.x > 7) continue;
                    if (nextGridPoint.y > 7) continue;
                    if (nextGridPoint.x < 0) continue;
                    if (nextGridPoint.y < 0) continue;

                    Debug.Log(direction);
                    Debug.Log(i);
                    Debug.Log(nextGridPoint);

                    try
                    {
                        //Test if a piece is on the grid (last point)
                        if (GameBoard.Instance.IsSameSidePieceAtLocation(nextGridPoint, isWhite))
                        {
                            Debug.Log("test");
                            break;
                        }


                    }
                    catch (System.Exception e)
                    {

                    }

                    try
                    {
                        //Test if a piece is on the grid (last point)
                        if (GameBoard.Instance.TryGetOtherPieceAtLocation(nextGridPoint, isWhite, out Pieces piece))
                        {
                            Debug.Log("tst");
                            if (direction.x * direction.y == 0 && (piece is Rook || piece is Queen)) return true;
                            if (direction.x * direction.y != 0 && (piece is Bishop || piece is Queen)) return true;

                            //PawnCheck
                            if (((i * direction.x == -1 || i * direction.x == 1) && ((i * direction.y == 1 && isWhite) || i * direction.y == -1 && !isWhite)) && piece is Pawns) return true;
                            break;
                        }

                    }
                    catch (System.Exception e)
                    {

                    }
                }
            }

            //HorseChecks
            {
                List<Vector2Int> horseCheck = new List<Vector2Int>();

                horseCheck.Add(new Vector2Int(-1, -2));
                horseCheck.Add(new Vector2Int(-2, -1));
                horseCheck.Add(new Vector2Int(-2, 1));
                horseCheck.Add(new Vector2Int(2, 1));
                horseCheck.Add(new Vector2Int(2, -1));
                horseCheck.Add(new Vector2Int(-1, 2));
                horseCheck.Add(new Vector2Int(1, 2));
                horseCheck.Add(new Vector2Int(1, -2));

                foreach (Vector2Int horseMovements in horseCheck)
                {
                    Vector2Int nextGridPoint = new Vector2Int(gridPosition.x + horseMovements.x, gridPosition.y + horseMovements.y);

                    if (nextGridPoint.x > 7) continue;
                    if (nextGridPoint.y > 7) continue;
                    if (nextGridPoint.x < 0) continue;
                    if (nextGridPoint.y < 0) continue;

                    if (GameBoard.Instance.TryGetOtherPieceAtLocation(nextGridPoint, isWhite, out Pieces piece))
                    {
                        if (piece is Knight) return true;
                    }
                }
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }


        return false;

    }

    public override void SetGridPosition(Vector2Int newPosition)
    {
        base.SetGridPosition(newPosition);
        hasMoved = true;
    }
}
