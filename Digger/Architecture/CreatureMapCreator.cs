using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Digger.Architecture;

public static class CreatureMapCreator
{
    private static readonly Dictionary<string, Func<ICreature>> factory = new();

    public static ICreature[,] CreateMap(string map, string separator = "\r\n")
    {
        var rows = map.Split([separator, "\n"], StringSplitOptions.RemoveEmptyEntries);
        if (rows.Select(z => z.Length).Distinct().Count() != 1)
        {
            throw new Exception($"Wrong test map '{map}'");
        }

        var result = new ICreature[rows[0].Length, rows.Length];
        for (var x = 0; x < rows[0].Length; x++)
        for (var y = 0; y < rows.Length; y++)
            result[x, y] = CreateCreatureBySymbol(rows[y][x])!;
        return result;
    }

    private static ICreature CreateCreatureByTypeName(string name)
    {
        // Это использование механизма рефлексии.
        // Ему посвящена одна из последних лекций второй части курса Основы программирования
        // В обычном коде можно было обойтись без нее, но нам нужно было написать такой код,
        // который работал бы, даже если вы ещё не создали класс Monster или Gold.
        // Просто написать new Gold() мы не могли, потому что это не скомпилировалось бы до создания класса Gold.
        if (!factory.ContainsKey(name))
        {
            var type = Assembly
                       .GetExecutingAssembly()
                       .GetTypes()
                       .FirstOrDefault(z => z.Name == name);
            if (type == null)
            {
                throw new Exception($"Can't find type '{name}'");
            }

            if (type == typeof(MonsterBase))
            {
                ;
            }

            factory[name] = () => (ICreature) Activator.CreateInstance(type);
        }

        return factory[name]();
    }


    private static ICreature? CreateCreatureBySymbol(char c)
    {
        return c switch
               {
                   'P' => CreateCreatureByTypeName("Player"),
                   'T' => CreateCreatureByTypeName("Terrain"),
                   'G' => CreateCreatureByTypeName("Gold"),
                   'S' => CreateCreatureByTypeName("Sack"),
                   'M' => CreateCreatureByTypeName("Monster"),
                   'R' => CreateCreatureByTypeName("MonsterRandom"),
                   ' ' => null,
                   _   => throw new Exception($"wrong character for ICreature {c}")
               };
    }
}

public static class RandomMapCreator
{
    public static string GenerateRandomMap(int width, int height)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                if (i == 0 && j == 0)
                {
                    sb.Append('P');
                    continue;
                }

                if ((i == 0 && j == 1) || (i == 1 && j == 1) || (i == 1 && j == 0))
                {
                    sb.Append('T');
                    continue;
                }

                var randomSymbol = GetRandomSymbolCreature();
                sb.Append(randomSymbol);
            }

            sb.Append("\r\n");
        }

        return sb.ToString();
    }

    private static char GetRandomSymbolCreature()
    {
        var random = new Random();
        char[] arr = [' ', 'T', 'G', 'S', 'M', 'R'];
        return arr[random.Next(0, arr.Length)];
    }
}