using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleEvaluation))]
public class SaveData : MonoBehaviour
{

    public static SaveData Instance;

    [SerializeField] public string folderNameBase;

    [SerializeField] public string folderNameWhite;

    [SerializeField] public string folderNameBlack;

    private SimpleEvaluation eval;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            Debug.LogWarning("Already a SaveData in scene, destroying " + this.name);
            return;
        }

        Instance = this;

        eval = GetComponent<SimpleEvaluation>();
    }

    
    public void SaveDataToFile(int turn, int amountBoards, float time, ColourChessSide side)
    {

        string folderName = folderNameBase;

        if (side == ColourChessSide.White)
        {
            folderName += $"/{folderNameWhite}";
        }

        if (side == ColourChessSide.Black)
        {
            folderName += $"/{folderNameBlack}";
        }

        WriteDataToText.WriteText("AmountBoardsChecked", folderName, turn, amountBoards.ToString());
        WriteDataToText.WriteText("Time", folderName, turn, time.ToString());
       // WriteDataToText.WriteText("AmountBoardsChecked", folderName, turn, score.ToString());

    }

    public void SaveScoreToFile(int turn, ChessBoard currentState)
    {
        string folderName = folderNameBase;

        int score = eval.Evaluate(currentState);

        WriteDataToText.WriteText("Score", folderName, turn, score.ToString());
    }

    public void SaveWhoWonToFile(ChessBoard currentState)
    {
        string folderName = folderNameBase;

        int score = eval.Evaluate(currentState);

        string winner = "";

        if (score > 1) winner += "White";
        else if (score < -1) winner += "Black";
        else winner += "Draw";

        WriteDataToText.WriteText("Winner", folderName, 1, winner);
    }
}
