using System.Collections.Generic;

namespace LimitedSizeStack;

public interface IUndoableCommand
{
    public void Execute();
    public void Undo();
}

public class AddCommand<TItem> : IUndoableCommand
{
    private readonly ListModel<TItem> _listModel;
    private readonly TItem _item;
    private int _index;

    public AddCommand(ListModel<TItem> listModel, TItem item)
    {
        _listModel = listModel;
        _item = item;
    }

    public void Execute()
    {
        _index = _listModel.Items.Count;
        _listModel.Items.Add(_item);
    }

    public void Undo()
    {
        _listModel.Items.RemoveAt(_index);
    }
}

public class RemoveCommand<TItem> : IUndoableCommand
{
    private readonly ListModel<TItem> _listModel;
    private readonly int _index;
    private TItem _item;

    public RemoveCommand(ListModel<TItem> listModel, int index)
    {
        _listModel = listModel;
        _index = index;
    }

    public void Execute()
    {
        _item = _listModel.Items[_index];
        _listModel.Items.RemoveAt(_index);
    }

    public void Undo()
    {
        _listModel.Items.Insert(_index, _item);
    }
}

public class ListModel<TItem>
{
    private readonly LimitedSizeStack<IUndoableCommand> _commandHistory;
    public List<TItem> Items { get; }

    public ListModel(int undoLimit) : this([], undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        _commandHistory = new LimitedSizeStack<IUndoableCommand>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        var addCommand = new AddCommand<TItem>(this, item);
        addCommand.Execute();
        _commandHistory.Push(addCommand);
    }

    public void RemoveItem(int index)
    {
        var removeCommand = new RemoveCommand<TItem>(this, index);
        removeCommand.Execute();
        _commandHistory.Push(removeCommand);
    }

    public bool CanUndo()
    {
        return _commandHistory.Count > 0;
    }

    public void Undo()
    {
        var command = _commandHistory.Pop();
        command.Undo();
    }
}