namespace Utilities
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}