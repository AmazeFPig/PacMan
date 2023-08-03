using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public bool IsDrawPath;

    private List<AstarNode> drawPathNodes = new List<AstarNode>();

    public List<AstarNode> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        AstarNode startNode = AStarGrid.GetInstance().WorldToAStarNode(startPosition);
        AstarNode endNode = AStarGrid.GetInstance().WorldToAStarNode(endPosition);

        // neighbors remaining
        List<AstarNode> openList = new List<AstarNode>();
        // nodes visited
        HashSet<AstarNode> closedList = new HashSet<AstarNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            AstarNode currentNode = openList[openList.Count - 1];

            openList.RemoveAt(openList.Count - 1);
            closedList.Add(currentNode);

            // reach goal
            if (currentNode == endNode)
            {
                return MakePath(startNode, endNode);
            }

            // list to hold neighbors
            List<AstarNode> toMerge = new List<AstarNode>();
            foreach (AstarNode neighbor in AStarGrid.GetInstance().GetNeighborNodes(currentNode))
            {
                if (neighbor.IsObstacle || closedList.Contains(neighbor))
                {
                    continue;
                }
                if (!openList.Contains(neighbor))
                {
                    neighbor.Parent = currentNode;
                    neighbor.GCost = currentNode.GCost + getManhattanDistance(neighbor, currentNode);
                    neighbor.HCost = getManhattanDistance(neighbor, endNode);
                    toMerge.Add(neighbor);
                }
            }

            toMerge.Sort((x, y) => y.FCost - x.FCost);

            openList = mergeLists(openList, toMerge);
        }

        return new List<AstarNode>();
    }

    public List<AstarNode> MakePath(AstarNode start, AstarNode end)
    {
        List<AstarNode> path = new List<AstarNode>();
        AstarNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();

        if (IsDrawPath)
        {
            drawPathNodes = new List<AstarNode>(path);
        }

        return path;
    }

    public int getManhattanDistance(AstarNode start, AstarNode end)
    {
        return Mathf.Abs(start.CoordinateX - end.CoordinateX) + Mathf.Abs(start.CoordinateY - end.CoordinateY);
    }

    public List<AstarNode> mergeLists(List<AstarNode> list1, List<AstarNode> list2)
    {
        //performs a merge from mergesort on two node lists, preferring list2
        List<AstarNode> result = new List<AstarNode>();
        int i = 0, j = 0;

        while (i < list1.Count || j < list2.Count)
        {
            if (i >= list1.Count)
            {
                result.Add(list2[j]);
                j++;
                continue;
            }
            if (j >= list2.Count)
            {
                result.Add(list1[i]);
                i++;
                continue;
            }

            if (list1[i].FCost > list2[j].FCost)
            {
                result.Add(list1[i]);
                i++;
            }
            else
            {
                result.Add(list2[j]);
                j++;
            }
        }

        return result;
    }

    public Vector3 WorldPointFromNode(AstarNode node)
    {
        //for use by entities using the pathfinding
        return AStarGrid.GetInstance().AStarNodeToWorld(node);
    }

    private void OnDrawGizmos()
    {
        //Draw the path in red if debugging
        if (IsDrawPath)
        {
            Gizmos.color = Color.red;
            if (drawPathNodes.Count > 0)
            {
                foreach (AstarNode node in drawPathNodes)
                {
                    Gizmos.DrawWireCube(AStarGrid.GetInstance().AStarNodeToWorld(node), new Vector3(1, 1, 1) * AStarGrid.GetInstance().NodeSideLength);
                }
            }
        }
    }
}