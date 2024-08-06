using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Surfer", menuName = "New Surfer")]
public class SurferInfoSO : ScriptableObject
{
    public Sprite sprite;
    public int pointsToBeat;
    public string surferName;
    public string surferCatchPhrase;
}
