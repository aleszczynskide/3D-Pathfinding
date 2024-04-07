using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask WalkableMask;
    public Vector3 GridWorldSize;
    public float NodeRadius;
    Node[,,] grid;
    Node[,,] pathgrid;
    float NodeSize;
    int GridSizeX;
    int GridSizeY;
    int GridSizeZ;
    public float A; 
    public float B;
    public float C;
    public List<Node> path;
    private void Start()
    {
        NodeSize = NodeRadius * 4;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / NodeSize);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / NodeSize);
        GridSizeZ = Mathf.RoundToInt(GridWorldSize.z / NodeSize);
        CreateGrid();
    }
    private void Update()
    {
        CreateGrid();
       
    }
    void CreateGrid()
    {
        grid = new Node[GridSizeX, GridSizeY, GridSizeZ];
        pathgrid = new Node[GridSizeX, GridSizeY, GridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.up * GridWorldSize.y / 2 - Vector3.forward * GridWorldSize.z / 2;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int z = 0; z < GridSizeZ; z++)
                {
                 Vector3 worldpoint = worldBottomLeft + Vector3.right * (x * NodeSize + NodeRadius) + Vector3.up * (y * NodeSize + NodeRadius) + Vector3.forward * (z * NodeSize + NodeRadius);
                 bool walkable = !Physics.BoxCast(worldpoint, Vector3.one * NodeRadius, Vector3.up, Quaternion.identity, WalkableMask);

                    grid[x, y, z] = new Node(walkable, worldpoint, x, y, z);
                }
            }
        }
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int z = 0; z < GridSizeZ; z++)
                {
                    if (y + 1 < GridSizeY && grid[x, y + 1, z].Walkable && !grid[x, y, z].Walkable)
                    {
                        Vector3 worldpoint = worldBottomLeft + Vector3.right * (x * NodeSize + NodeRadius) + Vector3.up * (y * NodeSize + NodeRadius) + Vector3.forward * (z * NodeSize + NodeRadius);
                        pathgrid[x, y, z] = new Node(true, worldpoint, x, y, z);
                    }
                }
            }
        }
    }
    public Node NodeFromWorldPoint(Vector3 WorldPosition)
    {
        float PercentX = Mathf.Clamp01((WorldPosition.x - A) / GridWorldSize.x);
        float PercentY = Mathf.Clamp01((WorldPosition.y - B) / GridWorldSize.y);
        float PercentZ = Mathf.Clamp01((WorldPosition.z  - C) / GridWorldSize.z);
        int x = Mathf.RoundToInt((GridSizeX - 1) * PercentX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * PercentY);
        int z = Mathf.RoundToInt((GridSizeZ - 1) * PercentZ);
        return grid[x, y, z];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, GridWorldSize.z));
        foreach (Node n in grid)
        {
            Gizmos.color = (n.Walkable)? Color.clear : Color.clear;
            if (path != null)
            {
                if (path.Contains(n))
                {
                    Gizmos.color = Color.cyan;
                }
            }
            if (n != null)
            {
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (NodeSize - 0.1f));
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> Neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;
                    int checkZ = node.GridZ + z;

                    if (checkX >= 0 && checkX < GridSizeX && checkY >= 0 && checkY < GridSizeY && checkZ >= 0 && checkZ < GridSizeZ)
                    {
                        Neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }
        return Neighbours;
    }
}
