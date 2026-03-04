using System;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', OutputByte(write));
            vm.RegisterCommand('+', IncByte);
            vm.RegisterCommand('-', DecByte);
            vm.RegisterCommand(',', InputByte(read));
            vm.RegisterCommand('>', ShiftRight);
            vm.RegisterCommand('<', ShiftLeft);
            RegisterAsciiCommands(vm);
        }

        private static void RegisterAsciiCommands(IVirtualMachine vm)
        {
            const int alphabetLength = 26;
            const int digitsCount = 10;

            var allAscii = Enumerable.Range('a', alphabetLength)
                                     .Concat(Enumerable.Range('A', alphabetLength))
                                     .Concat(Enumerable.Range('0', digitsCount))
                                     .Select(x => (char) x);
            foreach (var symbol in allAscii)
            {
                var capturedSymbol = symbol;
                vm.RegisterCommand(capturedSymbol, b => b.Memory[b.MemoryPointer] = (byte) capturedSymbol);
            }
        }

        private static Action<IVirtualMachine> OutputByte(Action<char> write)
        {
            return b => write((char) b.Memory[b.MemoryPointer]);
        }

        private static void IncByte(IVirtualMachine b)
        {
            unchecked
            {
                b.Memory[b.MemoryPointer]++;
            }
        }

        private static void DecByte(IVirtualMachine b)
        {
            unchecked
            {
                b.Memory[b.MemoryPointer]--;
            }
        }

        private static Action<IVirtualMachine> InputByte(Func<int> read)
        {
            return b => b.Memory[b.MemoryPointer] = (byte) read();
        }

        private static void ShiftRight(IVirtualMachine b)
        {
            var memoryLength = b.Memory.Length;
            b.MemoryPointer = (b.MemoryPointer + 1) % memoryLength;
        }

        private static void ShiftLeft(IVirtualMachine b)
        {
            var memoryLength = b.Memory.Length;
            b.MemoryPointer = (b.MemoryPointer - 1 + memoryLength) % memoryLength;
        }
    }
}