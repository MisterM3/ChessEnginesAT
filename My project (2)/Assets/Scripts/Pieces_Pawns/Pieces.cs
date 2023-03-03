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

    public virtual List<Vector2Int> MoveLocations(Vector2Int gridPoint)
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


    public void SetGridPosition(Vector2Int newPosition)
    {
        gridPosition = newPosition;
        this.gameObject.transform.position = new Vector3(newPosition.x, 0, newPosition.y);
    }


}
