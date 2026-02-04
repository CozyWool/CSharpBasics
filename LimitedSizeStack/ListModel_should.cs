using System.Collections.Generic;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace LimitedSizeStack;

[TestFixture]
public class ListModel_Should
{
	[Test]
	public void AddItems()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		Assert.AreEqual(new List<string>{"a", "bb", "ccc"}, model.Items);
	}

	[Test]
	public void RemoveFromTheEnd()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(2);
		Assert.AreEqual(new List<string> { "a", "bb" }, model.Items);
	}

	[Test]
	public void RemoveFromTheBeginning()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(0);
		Assert.AreEqual(new List<string> { "bb", "ccc" }, model.Items);
	}

	[Test]
	public void RemoveFromTheMiddle()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(1);
		Assert.AreEqual(new List<string> { "a", "ccc" }, model.Items);
		model.Undo();
		Assert.AreEqual(new List<string> { "a", "bb", "ccc" }, model.Items);
	}

	[Test]
	public void RemoveAndUndoAllItems()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(0);
		model.RemoveItem(0);
		model.RemoveItem(0);
		Assert.AreEqual(new List<string>(), model.Items);
		model.Undo();
		model.Undo();
		model.Undo();
		Assert.AreEqual(new List<string> { "a", "bb", "ccc" }, model.Items);
	}

	[Test]
	public void UndoAddOperations()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		Assert.AreEqual(true, model.CanUndo());
		model.Undo();
		Assert.AreEqual(0, model.Items.Count);
	}

	[Test]
	public void NotUndo_WhenEverythingIsUndone()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
	}

	[Test]
	public void Add_AfterUndo()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.Undo();
		model.AddItem("qq");
		Assert.AreEqual(new List<string> { "qq" }, model.Items);
	}

	[Test]
	public void Undo_AfterRemove()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.RemoveItem(1);
		model.Undo();
		Assert.AreEqual(new List<string> { "a", "bb" }, model.Items);
	}

	[Test]
	public void Remove_AfterUndo()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.RemoveItem(0);
		Assert.AreEqual(0, model.Items.Count);
	}

	[Test]
	public void NotUndo_WhenUndoLimitIsReached()
	{
		var model = new ListModel<string>(2);
		model.AddItem("a");
		model.AddItem("bb");
		model.RemoveItem(1);
		model.Undo();
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
		Assert.AreEqual(new List<string> {"a"}, model.Items);
	}

	[Test]
	public void CanUndo_ReturnsFalse_WhenUndoLimitIsReached()
	{
		var model = new ListModel<string>(1);
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("ccc");
		Assert.AreEqual(true, model.CanUndo());
	}

	[Test]
	public void CanUndo_ReturnsFalse_WhenUndoLimitIsZero()
	{
		var model = new ListModel<string>(0);
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("a");
		model.AddItem("bb");
		Assert.AreEqual(false, model.CanUndo());
	}
}

[TestFixture]
public class ListModel_DuplicateItems_Should
{
    [Test]
    public void UndoAdd_WhenItemsAreSame()
    {
        var model = new ListModel<string>(20);
        model.AddItem("a");
        model.AddItem("a"); // Добавляем дубликат
        model.AddItem("b");

        // Отменяем последнее добавление
        model.Undo();
        Assert.AreEqual(new List<string> { "a", "a" }, model.Items);

        // Отменяем второе добавление
        model.Undo();
        Assert.AreEqual(new List<string> { "a" }, model.Items);

        // Отменяем первое добавление
        model.Undo();
        Assert.AreEqual(new List<string>(), model.Items);
    }

    [Test]
    public void UndoAddAndRemove_WithDuplicateItems()
    {
        var model = new ListModel<string>(20);
        model.AddItem("a");
        model.AddItem("b");
        model.AddItem("a"); // Дубликат
        model.AddItem("c");

        // Удаляем элемент с индексом 2 (вторую "a")
        model.RemoveItem(2);
        Assert.AreEqual(new List<string> { "a", "b", "c" }, model.Items);

        // Отменяем удаление - должна восстановиться "a" на позицию 2
        model.Undo();
        Assert.AreEqual(new List<string> { "a", "b", "a", "c" }, model.Items);

        // Отменяем добавление "c"
        model.Undo();
        Assert.AreEqual(new List<string> { "a", "b", "a" }, model.Items);

        // Отменяем добавление второй "a"
        model.Undo();
        Assert.AreEqual(new List<string> { "a", "b" }, model.Items);
    }

    [Test]
    public void UndoMultipleAdds_WithSameItems()
    {
        var model = new ListModel<int>(20);
        model.AddItem(1);
        model.AddItem(2);
        model.AddItem(1); // Дубликат значения 1
        model.AddItem(2); // Дубликат значения 2

        // Последовательная отмена
        model.Undo();
        Assert.AreEqual(new List<int> { 1, 2, 1 }, model.Items);

        model.Undo();
        Assert.AreEqual(new List<int> { 1, 2 }, model.Items);

        model.Undo();
        Assert.AreEqual(new List<int> { 1 }, model.Items);

        model.Undo();
        Assert.AreEqual(new List<int>(), model.Items);
    }

    [Test]
    public void ComplexScenario_WithDuplicates()
    {
        var model = new ListModel<string>(10);
        // Добавляем элементы
        model.AddItem("x");
        model.AddItem("x");
        model.AddItem("y");
        model.AddItem("x");

        // Удаляем элемент с индексом 1 (второй "x")
        model.RemoveItem(1);
        Assert.AreEqual(new List<string> { "x", "y", "x" }, model.Items);

        // Удаляем элемент с индексом 2 (последний "x")
        model.RemoveItem(2);
        Assert.AreEqual(new List<string> { "x", "y" }, model.Items);

        // Отменяем последнее удаление (должна восстановиться "x" на позицию 2)
        model.Undo();
        Assert.AreEqual(new List<string> { "x", "y", "x" }, model.Items);

        // Отменяем первое удаление (должна восстановиться "x" на позицию 1)
        model.Undo();
        Assert.AreEqual(new List<string> { "x", "x", "y", "x" }, model.Items);

        // Отменяем добавление последнего "x"
        model.Undo();
        Assert.AreEqual(new List<string> { "x", "x", "y" }, model.Items);
    }

    [Test]
    public void UndoAdd_WhenItemIsNull()
    {
        var model = new ListModel<string>(20);
        model.AddItem(null);
        model.AddItem("a");
        model.AddItem(null); // Второй null

        // Отменяем добавление второго null
        model.Undo();
        Assert.AreEqual(new List<string> { null, "a" }, model.Items);

        // Отменяем добавление "a"
        model.Undo();
        Assert.AreEqual(new List<string> { null }, model.Items);

        // Отменяем добавление первого null
        model.Undo();
        Assert.AreEqual(new List<string>(), model.Items);
    }
}