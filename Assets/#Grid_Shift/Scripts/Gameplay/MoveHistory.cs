using System.Collections.Generic;

public class MoveHistory
{
    private readonly Stack<MoveResult> history = new();

    public int MoveCount => history.Count;

    public void Record(MoveResult move)
    {
        if (!move.Success)
            return;

        history.Push(move);
    }

    public bool TryUndo(out MoveResult move)
    {
        if (history.Count == 0)
        {
            move = default;
            return false;
        }

        move = history.Pop();
        return true;
    }

    public void Clear()
    {
        history.Clear();
    }
}