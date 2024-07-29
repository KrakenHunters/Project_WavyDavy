using System.Collections.Generic;
using Tricks;
using UnityEngine;
[CreateAssetMenu(fileName = "trick", menuName = "New Trick")]
public class TrickSO : ScriptableObject
{
    public string trickName;
    public Sprite icon;
    public int points;
    public List<TrickCombo> trickCombo;
    private TrickEvalStrategy _strategy;

    public TrickResult Evaluate(List<TrickCombo> combos)
    {
        return _strategy.EvaluateTrick(combos);
    }
    private void OnEnable()
    {
        _strategy = new TrickEvalStrategy(trickCombo);
    }
}
