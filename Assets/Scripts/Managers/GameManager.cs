using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GamePhase currentGamePhase;

    public GameEvent Event;


}

public enum GamePhase
{
    Phase1,
    Phase2,
    Phase3,
    Trick
}
