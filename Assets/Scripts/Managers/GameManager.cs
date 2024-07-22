using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GamePhase currentGamePhase;

    public GameEvent Event;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ChangeGamePhase);
    }
    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(ChangeGamePhase);
    }
    private void Awake()
    {
        Event.OnChangeGameState.Invoke(GamePhase.Phase1);
    }


    private void ChangeGamePhase(GamePhase nextPhase)
    {
      switch (nextPhase)
      {
          case GamePhase.Phase1:
              currentGamePhase = GamePhase.Phase1;
              break;
          case GamePhase.Phase2:
              currentGamePhase = GamePhase.Phase2;
              break;
          case GamePhase.Phase3:
              currentGamePhase = GamePhase.Phase3;
              break;
          case GamePhase.Trick:
              currentGamePhase = GamePhase.Trick;
              break;
      }
    }

}

public enum GamePhase
{
    Phase1,
    Phase2,
    Phase3,
    Trick
}
