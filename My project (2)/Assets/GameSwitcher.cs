using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSwitcher : MonoBehaviour
{
    [SerializeField] List<GameSwitch> games;
    Queue<GameSwitch> stackGames;

    public void Awake()
    {
        stackGames = new Queue<GameSwitch>(games);
    }

    public GameSwitch NextGame()
    {
        return stackGames.Dequeue();
    }
}

[Serializable]
public struct GameSwitch
{
    public int amountGames;

    public string folderName;
    public string whiteName;
    public string blackName;

    public AbstractPlayer whitePlayer;
    public AbstractPlayer blackPlayer;
}
