using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameBoard))]
public class ChessBoardVisual : MonoBehaviour
{


    [SerializeField] GameObject WhiteSpacePrefab;
    [SerializeField] GameObject BlackSpacePrefab;
    [SerializeField] GameObject parentBoardVisual;

    [SerializeField] GameObject moveAblePrefab;

    GameObject[,] piecesVisuals;

    GameObject[,] MoveAbleSpacesVisuals;

    // Start is called before the first frame update
    void Start()
    {

        MoveAbleSpacesVisuals = new GameObject[GameBoard.SIZE, GameBoard.SIZE];

        for (int x = 0; x < GameBoard.SIZE; x++)
        {
            for (int y = 0; y < GameBoard.SIZE; y++)
            {
                Vector3Int position = new Vector3Int(x, 0, y);

                if ((x - y)%2 == 0)
                {
                    Instantiate(BlackSpacePrefab, position, Quaternion.identity, parentBoardVisual.transform);
                }
                else Instantiate(WhiteSpacePrefab,position, Quaternion.identity, parentBoardVisual.transform);

                GameObject moveAbleVisual = Instantiate(moveAblePrefab, position, Quaternion.identity, parentBoardVisual.transform);

                MoveAbleSpacesVisuals[x, y] = moveAbleVisual;


            }
        }

        DeActivateAllMoveVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DeActivateAllMoveVisual()
    {
        foreach (GameObject visual in MoveAbleSpacesVisuals) visual.SetActive(false);
    }

    public void ActivateMoveVisuals(List<Vector2Int> positions)
    {

        DeActivateAllMoveVisual();

        foreach (Vector2Int position in positions)
        {
            try
            {
                MoveAbleSpacesVisuals[position.x, position.y].SetActive(true);

            }
            catch (System.Exception e)
            {

            }
        }
    }



}
