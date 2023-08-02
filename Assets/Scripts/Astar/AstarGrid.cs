using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    public Tilemap ObstacleTileMap;
    public Vector2 GridWorldSize;
    public float NodeSideLength = 1f;

    public bool IsShowGrid;

    private AstarNode[,] nodeGrid;

    private int gridWidth, gridHeight;
    private float offset;

    private void Start()
    {
        gridWidth = Mathf.RoundToInt(GridWorldSize.x / NodeSideLength);
        gridHeight = Mathf.RoundToInt(GridWorldSize.y / NodeSideLength);

        offset = NodeSideLength / 2;

        BuildGrid();
    }

    private void BuildGrid()
    {
        nodeGrid = new AstarNode[gridWidth, gridHeight];

        Vector3 startPosition = transform.position - new Vector3(GridWorldSize.x / 2, GridWorldSize.y / 2, 0);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 nodeCenter = startPosition + new Vector3(x * NodeSideLength + offset, y * NodeSideLength + offset, 0);

                nodeGrid[x, y] = new AstarNode(isObstacle: ObstacleTileMap.HasTile(ObstacleTileMap.WorldToCell(nodeCenter)), x, y);
            }
        }
    }

    public AstarNode WorldToAStarNode(Vector3 worldPosition)
    {
        Vector3 centered = worldPosition - transform.position;

        int x = Mathf.RoundToInt(Mathf.Clamp01(centered.x / GridWorldSize.x + 0.5f) * (gridWidth - 1));
        int y = Mathf.RoundToInt(Mathf.Clamp01(centered.y / GridWorldSize.y + 0.5f) * (gridHeight - 1));

        return nodeGrid[x, y];
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
        bool right = x != gridWidth - 1;
        bool down = y != 0;
        bool up = y != gridHeight - 1;

        if (left)
        {
            neighborList.Add(nodeGrid[x - 1, y]);
        }
        if (right)
        {
            neighborList.Add(nodeGrid[x + 1, y]);
        }
        if (down)
        {
            neighborList.Add(nodeGrid[x, y - 1]);
        }
        if (up)
        {
            neighborList.Add(nodeGrid[x, y + 1]);
        }

        return neighborList;
    }

    private void OnDrawGizmos()
    {

        if (IsShowGrid && nodeGrid != null)
        {
            foreach (AstarNode node in nodeGrid)
            {
                if (node.IsObstacle)
                {
                    Gizmos.color = Color.yellow;
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