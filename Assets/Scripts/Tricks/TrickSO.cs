using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "trick", menuName = "New Trick")]
public class TrickSO : ScriptableObject
{
    public string trickName;
    public Sprite icon;
    public int points;
    public List<TrickCombo> trickCombo;
}
