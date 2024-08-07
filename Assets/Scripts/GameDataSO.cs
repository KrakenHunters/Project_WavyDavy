using System.Collections.Generic;
using Tricks;
using UnityEngine;
[CreateAssetMenu(fileName = "score", menuName = "New Score")]
public class GameDataSO : ScriptableObject
{
    public int Score;
    public GameMode gameMode;
}
