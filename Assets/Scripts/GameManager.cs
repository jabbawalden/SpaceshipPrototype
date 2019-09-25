using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {Start, Playing, Restart, Completed}
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        gameState = GameState.Start;
    }

    public void GameStatePlaying()
    {
        uiManager.PlayGame();
        gameState = GameState.Playing;
    }

    public void GameStateDead()
    {
        uiManager.SetEndMissionPanel(false);
        gameState = GameState.Restart;
    }

    public void GameStateComplete() 
    {
        uiManager.SetEndMissionPanel(true);
        gameState = GameState.Restart;
    }
}
