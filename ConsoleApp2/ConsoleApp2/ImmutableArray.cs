/*
Задача "Целостность данных". Напишите ImmutableArray - неизменяемый массив intов. Его должно быть возможно создать на основе переданного обычного массива, он
должен иметь метод Get, отдающий элемент по индексу, и свойство Length, отдающее
длину массива. Позаботьтесь о целостности данных, чтобы массив и правда был
неизменяемым
*/

namespace ConsoleApp2;

public class ImmutableArray
{
    private readonly int[] _array;

    public int Length { get; }

    public ImmutableArray(List<int> array)
    {
        _array = new int[array.Count];
        array.CopyTo(_array, 0);

        Length = array.Count;
    }

    public ImmutableArray(int[] array)
    {
        _array = new int[array.Length];
        array.CopyTo(_array, 0);

        Length = array.Length;
    }

    public int Get(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new IndexOutOfRangeException();
        }

        return _array[index];
    }
}