using System.Collections.Generic;

namespace Tricks
{
    public class TrickEvalStrategy
    {
        public TrickEvalStrategy(List<TrickCombo> trickCombos) 
        {
            Combos = trickCombos;
        }

        public List<TrickCombo> Combos { get; set; }

        public virtual TrickResult EvaluateTrick(List<TrickCombo> combos)
        {
            if (combos.Count > Combos.Count)
                return TrickResult.Failed;
            bool hasMatch = true;
            for (int i = 0; i < combos.Count; i++)
            {
                hasMatch &= combos[i] == Combos[i];
            }
            if (!hasMatch)
                return TrickResult.Failed;
            return combos.Count == Combos.Count ? TrickResult.Complete : TrickResult.Possible;
        }

/*        public virtual TrickResult EvaluateTrick(List<TrickCommand> trickCommands) // change if you want time held and stuff
        {
            if (trickCommands.Count > Combos.Count)
                return TrickResult.Failed;
            bool hasMatch = true;
            for (int i = 0; i < trickCommands.Count; i++)
            {
                hasMatch &= trickCommands[trickCommands.Count - Combos.Count + i].TrickButton == Combos[i];
            }
            if (!hasMatch)
                return TrickResult.Failed;
            return trickCommands.Count == Combos.Count ? TrickResult.Complete : TrickResult.Possible;
        }*/
    }

    public enum TrickResult
    {
        Possible, Failed, Complete
    }
}