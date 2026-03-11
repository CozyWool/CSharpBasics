using System;

namespace func.brainfuck;

public class VmBuilder : IVmBuilder
{
    private Func<string, IVirtualMachine> _buildVirtualMachine;

    public VmBuilder(int memorySize)
    {
        _buildVirtualMachine = program => new VirtualMachine(program, memorySize);
    }

    public VmBuilder AddBasicCommands(Func<int> read, Action<char> write)
    {
        var previousBuild = _buildVirtualMachine;
        _buildVirtualMachine = program =>
                               {
                                   var vm = previousBuild(program);
                                   BrainfuckBasicCommands.RegisterTo(vm, read, write);

                                   return vm;
                               };
        return this;
    }

    public VmBuilder AddLoopCommands()
    {
        var previousBuild = _buildVirtualMachine;
        _buildVirtualMachine = program =>
                               {
                                   var vm = previousBuild(program);
                                   BrainfuckLoopCommands.RegisterTo(vm);

                                   return vm;
                               };
        return this;
    }

    public IVirtualMachine Build(string program)
    {
        return _buildVirtualMachine(program);
    }
}