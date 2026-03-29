// описание ребер разделены пробелами
// дефисом разделены номера вершин ребраCheckHasCycle("0-1", false);

CheckHasCycle("0-1 0-2", false);
CheckHasCycle("0-1 0-2 1-2", true);
CheckHasCycle("0-1 0-2 0-3", false);
CheckHasCycle("0-1 0-2 0-3 1-3", true);
Console.WriteLine("OK");

void CheckHasCycle(string p0, bool p1)
{
    throw new NotImplementedException();
}

bool HasCycle(List<Node> graph)
{
    var visited = new HashSet<Node>();  // Серые вершины
    var finished = new HashSet<Node>(); // Черные вершины
    var stack = new Stack<Node>();
    visited.Add(graph.First());
    stack.Push(graph.First());
    while (stack.Count != 0)
    {
        var node = stack.Pop();
        foreach (var nextNode in node.IncidentNodes.Where(n => !finished.Contains(n)))
        {
            if (!visited.Add(nextNode))
            {
                return true;
            }

            stack.Push(nextNode);
        }

        finished.Add(node); // красим в черный, когда рассмотрели все пути из node
    }

    return false;
}

public class Node
{
    public int NodeNumber;
    public List<Node> IncidentNodes = new List<Node>();
}