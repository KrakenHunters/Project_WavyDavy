using Utilities;

namespace Tricks
{
    public class TrickCommand : ICommand
    {
        public TrickCombo TrickButton { get; set; }
        public float TimeHeld { get; set; }
        public float TimeStarted { get; set; }
        
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }
    }
    
}