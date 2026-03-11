namespace func.brainfuck;

public interface IVmBuilder
{
    IVirtualMachine Build(string program);
}