using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : SingletonMono<AStarGrid>
{
    public Tilemap ObstacleTileMap;
    public Vector2 GridWorldSize;
    public float NodeSideLength = 1f;

    public bool IsShowGrid;

    public int GridWidth, GridHeight;

    public AstarNode[,] NodeGrid;

    private float offset;

    private void Start()
    {
        GridWidth = Mathf.RoundToInt(GridWorldSize.x / NodeSideLength);
        GridHeight = Mathf.RoundToInt(GridWorldSize.y / NodeSideLength);

        offset = NodeSideLength / 2;

        BuildGrid();

        CheckGrid();
    }

    private void BuildGrid()
    {
        NodeGrid = new AstarNode[GridWidth, GridHeight];

        Vector3 startPosition = transform.position - new Vector3(GridWorldSize.x / 2, GridWorldSize.y / 2, 0);

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector3 nodeCenter = startPosition + new Vector3(x * NodeSideLength + offset, y * NodeSideLength + offset, 0);

                NodeGrid[x, y] = 
                    new AstarNode(
                        isObstacle: ObstacleTileMap.HasTile(ObstacleTileMap.WorldToCell(nodeCenter)),
                        x, 
                        y);
            }
        }
    }

    private void CheckGrid()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                AstarNode node = NodeGrid[x, y];
                List<AstarNode> openNeighbors = GetOpenNeighborNodes(node);
                if (openNeighbors.Count > 2)
                {
                    node.IsCross = true;
                }
                else if (openNeighbors.Count == 2 
                    && openNeighbors[0].CoordinateX != openNeighbors[1].CoordinateX
                    && openNeighbors[0].CoordinateY != openNeighbors[1].CoordinateY)
                {
                    node.IsCorner = true;
                }
                foreach (AstarNode neighbor in openNeighbors)
                {
                    node.OpenDirections.Add(new Vector3(neighbor.CoordinateX-node.CoordinateX, neighbor.CoordinateY-node.CoordinateY, 0));
                }
                
            }
        }
    }

    public AstarNode WorldToAStarNode(Vector3 worldPosition)
    {
        Vector3 centered = worldPosition - transform.position;

        int x = Mathf.RoundToInt(Mathf.Clamp01(centered.x / GridWorldSize.x + 0.5f) * (GridWidth - 1));
        int y = Mathf.RoundToInt(Mathf.Clamp01(centered.y / GridWorldSize.y + 0.5f) * (GridHeight - 1));

        return NodeGrid[x, y];
    }

    public Vector3 AStarNodeToWorld(AstarNode node)
    {
        Vector3 position = transform.position;

        position.x = position.x - (GridWorldSize.x / 2) + ((node.CoordinateX + 0.5f) * NodeSideLength);
        position.y = position.y - (GridWorldSize.y / 2) + ((node.CoordinateY + 0.5f) * NodeSideLength);

        return position;
    }

    public List<AstarNode> GetNeighborNodes(AstarNode node)
    {
        List<AstarNode> neighborList = new List<AstarNode>();

        int x = node.CoordinateX;
        int y = node.CoordinateY;

        bool left = x != 0;
        bool right = x != GridWidth - 1;
        bool down = y != 0;
        bool up = y != GridHeight - 1;

        if (left)
        {
            neighborList.Add(NodeGrid[x - 1, y]);
        }
        if (right)
        {
            neighborList.Add(NodeGrid[x + 1, y]);
        }
        if (down)
        {
            neighborList.Add(NodeGrid[x, y - 1]);
        }
        if (up)
        {
            neighborList.Add(NodeGrid[x, y + 1]);
        }

        return neighborList;
    }

    public List<AstarNode> GetOpenNeighborNodes(AstarNode node)
    {
        List<AstarNode> openNeighborList = new List<AstarNode>();

        int x = node.CoordinateX;
        int y = node.CoordinateY;

        bool left = x != 0;
        bool right = x != GridWidth - 1;
        bool down = y != 0;
        bool up = y != GridHeight - 1;

        if (left && !NodeGrid[x - 1, y].IsObstacle)
        {
            openNeighborList.Add(NodeGrid[x - 1, y]);
        }
        if (right && !NodeGrid[x + 1, y].IsObstacle)
        {
            openNeighborList.Add(NodeGrid[x + 1, y]);
        }
        if (down && !NodeGrid[x, y - 1].IsObstacle)
        {
            openNeighborList.Add(NodeGrid[x, y - 1]);
        }
        if (up && !NodeGrid[x, y + 1].IsObstacle)
        {
            openNeighborList.Add(NodeGrid[x, y + 1]);
        }

        return openNeighborList;
    }

    public AstarNode GetNeighborNode(AstarNode node, Vector3 direction)
    {
        int x = node.CoordinateX;
        int y = node.CoordinateY;

        bool left = x != 0;
        bool right = x != GridWidth - 1;
        bool down = y != 0;
        bool up = y != GridHeight - 1;

        if (direction == Vector3.left && left)
        {
            return NodeGrid[x - 1, y];
        }
        if (direction == Vector3.right && right)
        {
            return NodeGrid[x + 1, y];
        }
        if (direction == Vector3.down && down)
        {
            return NodeGrid[x, y - 1];
        }
        if (direction == Vector3.up && up)
        {
            return NodeGrid[x, y + 1];
        }

        return null;
    }

    public int GetManhattanDistance(Vector3 startPos, Vector3 endPos)
    {
        AstarNode startNode = WorldToAStarNode(startPos);
        AstarNode endNode = WorldToAStarNode(endPos);

        return GetManhattanDistance(startNode, endNode);
    }

    public int GetManhattanDistance(AstarNode startNode, AstarNode endNode)
    {
        return Mathf.Abs(startNode.CoordinateX - endNode.CoordinateX) + Mathf.Abs(startNode.CoordinateY - endNode.CoordinateY);
    }

    private void OnDrawGizmos()
    {

        if (IsShowGrid && NodeGrid != null)
        {
            foreach (AstarNode node in NodeGrid)
            {
                if (node.IsObstacle)
                {
                    Gizmos.color = Color.yellow;
                }
                else if (node.IsCorner)
                {
                    Gizmos.color = Color.blue;
                }
                else if (node.IsCross)
                {
                    Gizmos.color = Color.green;

                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawWireCube(AStarNodeToWorld(node), new Vector3(1, 1, 1) * NodeSideLength);

            }
        }
    }
}