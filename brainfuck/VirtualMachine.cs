using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine(string program, int memorySize) : IVirtualMachine
    {
        public string Instructions { get; } = program;
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; } = new byte[memorySize];
        public int MemoryPointer { get; set; }
        private Dictionary<char, Action<IVirtualMachine>> Commands { get; } = new();

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            Commands.TryAdd(symbol, execute);
        }

        public void Run()
        {
            while (0 <= InstructionPointer && InstructionPointer < Instructions.Length)
            {
                var command = Instructions[InstructionPointer];
                if (Commands.TryGetValue(command, out var execute))
                {
                    execute(this);
                }

                InstructionPointer++;
            }
        }
    }
}