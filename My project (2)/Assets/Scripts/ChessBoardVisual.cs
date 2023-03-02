using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameBoard))]
public class ChessBoardVisual : MonoBehaviour
{


    [SerializeField] GameObject WhiteSpacePrefab;
    [SerializeField] GameObject BlackSpacePrefab;
    [SerializeField] GameObject parentBoardVisual;

    GameObject[,] piecesVisuals;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < GameBoard.SIZE; x++)
        {
            for (int y = 0; y < GameBoard.SIZE; y++)
            {
                Vector3Int position = new Vector3Int(x, 0, y);

                if ((x - y)%2 == 0)
                {
                    Instantiate(BlackSpacePrefab, position, Quaternion.identity, parentBoardVisual.transform);
                }
                else Instantiate(WhiteSpacePrefab,position, Quaternion.identity, parentBoardVisual.transform);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
