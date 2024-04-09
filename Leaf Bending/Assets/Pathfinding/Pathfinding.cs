using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform Seeker, Target;
    public Grid grid;
    private void Start()
    {
        if (grid == null)
        {
            grid = GetComponent<Grid>();
        }

    }
    private void Update()
    {
        FindPath(Seeker.transform.position, Target.transform.position);
    }
    public void FindPath(Vector3 StartPosition, Vector3 TargetPosition)
    {
        Node StartNode = grid.NodeFromWorldPoint(StartPosition);
        Node TargetNode = grid.NodeFromWorldPoint(TargetPosition);
        List<Node> OpenSet = new List<Node>();
        HashSet<Node> ClosedSet = new HashSet<Node>();
        OpenSet.Add(StartNode);

        while (OpenSet.Count > 0)
        {
            Node CurrentNode = OpenSet[0];
            for (int i = 1; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].FCost < CurrentNode.FCost || OpenSet[i].FCost == CurrentNode.FCost)
                {
                    CurrentNode = OpenSet[i];
                }
            }
            OpenSet.Remove(CurrentNode);
            ClosedSet.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                RetracePath(StartNode, TargetNode);
                return;
            }

            foreach (Node Neighbour in grid.GetNeighbours(CurrentNode))
            {
                if (Neighbour.Walkable || Neighbour.Obstacle || ClosedSet.Contains(Neighbour))
                {
                    continue;
                }
                int NewMovementCostNeighbour = CurrentNode.GCost + GetDistance(CurrentNode, Neighbour);
                if (NewMovementCostNeighbour < Neighbour.GCost || !OpenSet.Contains(Neighbour))
                {
                    Neighbour.GCost = NewMovementCostNeighbour;
                    Neighbour.HCost = GetDistance(Neighbour, TargetNode);
                    Neighbour.Parent = CurrentNode;
                    if (!OpenSet.Contains(Neighbour))
                    {
                        OpenSet.Add(Neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Add(startNode);
        path.Reverse();
        grid.path = path;
    }

    int GetDistance(Node nodea, Node nodeb)
    {
        int dstx = Mathf.Abs(nodea.GridX - nodeb.GridX);
        int dsty = Mathf.Abs(nodea.GridY - nodeb.GridY);
        int dstz = Mathf.Abs(nodea.GridZ - nodeb.GridZ);
        int upwardCost = 10;
        int totalCost = 14 * (dstx + dstz) + (dsty/10);
        if (nodea.GridY < nodeb.GridY)
        {
            totalCost += upwardCost;
        }
        return totalCost;
    }
}
