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

    [SerializeField] int iteration;

    [SerializeField] AbstractPlayer whitePlayer;
    [SerializeField] AbstractPlayer blackPlayer;



    [SerializeField] int maxTurn;
    [SerializeField] int maxIterations;



    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        turn = 1;
        iteration = 1;
        
    }


    public float timer = .2f;
    public float cooldown = 0.0f;

    private void Update()
    {
        if (iteration > maxIterations)
        {
            Debug.Log("WE DONE ERE!!!");
            return;
        }
        if (turn > maxTurn)
        {
            ResetGame();
            return;
        }

       MakeTurn();

    }

    private void ResetGame()
    {
        SaveData.Instance.SaveWhoWonToFile(GameBoard.Instance.chessBoardPositions);
        iteration++;
        isWhiteTurn = true;
        GameBoard.Instance.ResetGame();
        turn = 1;
    }



    public King getKing(bool isWhite)
    {
        if (isWhite) return whiteKing;
        else return blackKing;
    }

    public int GetTurn()
    {
        return turn;
    }


    public bool GetIsWhiteTurn()
    {
        return isWhiteTurn;
    }

    public void NextTurn()
    {
        isWhiteTurn = !isWhiteTurn;

        if (isWhiteTurn)
        {
            SaveData.Instance.SaveScoreToFile(turn, GameBoard.Instance.chessBoardPositions);
            turn++;
        }
    }

    public void MakeTurn()
    {

        if (isWhiteTurn) whitePlayer.MovePiece();
        else blackPlayer.MovePiece();


        NextTurn();
    }


}
