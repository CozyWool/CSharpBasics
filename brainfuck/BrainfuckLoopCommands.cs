using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var loopSymbols = FindMatchingBrackets(vm);

            vm.RegisterCommand('[', StartLoop(loopSymbols));
            vm.RegisterCommand(']', EndLoop(loopSymbols));
        }

        private static int[] FindMatchingBrackets(IVirtualMachine vm)
        {
            var loopSymbols = new int[vm.Instructions.Length];
            var bracketStack = new Stack<int>();
            for (var i = 0; i < vm.Instructions.Length; i++)
            {
                if (vm.Instructions[i] == '[')
                {
                    bracketStack.Push(i);
                }

                if (vm.Instructions[i] == ']')
                {
                    var openBracket = bracketStack.Pop();
                    loopSymbols[openBracket] = i;
                    loopSymbols[i] = openBracket;
                }
            }

            return loopSymbols;
        }

        private static Action<IVirtualMachine> StartLoop(int[] loopSymbols) =>
            b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                {
                    b.InstructionPointer = loopSymbols[b.InstructionPointer];
                }
            };

        private static Action<IVirtualMachine> EndLoop(int[] loopSymbols) =>
            b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                {
                    b.InstructionPointer = loopSymbols[b.InstructionPointer];
                }
            };
    }
}