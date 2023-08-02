

public class AstarNode
{
    public bool IsObstacle;
    public int CoordinateX;
    public int CoordinateY;

    // GCost: cost of the path from the start node to this node
    public int GCost;
    // HCost:  heuristic estimating the cost of the cheapest path from this node to the goal
    public int HCost;

    public int FCost 
    {
        get 
        {
            return GCost + HCost;
        }
    }

    public AstarNode Parent;

    public AstarNode(bool isObstacle, int x, int y)
    {
        IsObstacle = isObstacle;
        CoordinateX = x;
        CoordinateY = y;
    }

}