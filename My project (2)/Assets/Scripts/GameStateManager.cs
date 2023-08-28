using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public King blackKing;
    public King whiteKing;

    public bool isWhiteTurn;

    [SerializeField] int turn;

    [SerializeField] AbstractPlayer whitePlayer;
    [SerializeField] AbstractPlayer blackPlayer;



    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        
    }


    public float timer = .2f;
    public float cooldown = 0.0f;

    private void Update()
    {
      //  if (cooldown < 0)
        {
              if (Input.GetKeyDown(KeyCode.T))
              {  
            MakeTurn();
            //     MakeTurn();
            }
         //   cooldown = timer;
        }
        //cooldown -= Time.deltaTime;
    }

    /*
    public bool InCheck(Pieces[,] board, Vector2Int GridPosition, bool isWhite)
    {

        foreach (Pieces isKing in board)
        {
            if (isKing is King && isKing.colourPiece == isWhite)
            {
                King king = (King)isKing;

                return king.InCheck();
            }
        }

        Debug.LogError("I hate this");
       return false;

    }

    */

    public King getKing(bool isWhite)
    {
        if (isWhite) return whiteKing;
        else return blackKing;
    }


    public bool IsWhiteTurn()
    {
        return isWhiteTurn;
    }

    public void NextTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        turn++;
     //   MakeTurn();

    }

    public void MakeTurn()
    {

        if (isWhiteTurn) whitePlayer.MovePiece();
        else blackPlayer.MovePiece();


        NextTurn();
    }


}
