using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask WalkableMask;
    public Vector3 GridWorldSize;
    public float NodeRadius;
    Node[,,] grid;
    float NodeSize;
    int GridSizeX;
    int GridSizeY;
    int GridSizeZ;
    private void Start()
    {
        NodeSize = NodeRadius * 2;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x/NodeSize);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y/ NodeSize);
        GridSizeZ = Mathf.RoundToInt(GridWorldSize.z/ NodeSize);
        CreateGrid();
    }
    void CreateGrid()
    {
        grid = new Node[GridSizeX, GridSizeY, GridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.up * GridWorldSize.y / 2 - Vector3.forward * GridWorldSize.z / 2;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int z = 0; z < GridSizeZ; z++)
                {
                    Vector3 worldpoint = worldBottomLeft + Vector3.right * (x * NodeSize + NodeRadius) + Vector3.up * (y * NodeSize + NodeRadius) + Vector3.forward * (z * NodeSize + NodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldpoint, NodeRadius));
                    grid[x, y, z] = new Node(walkable, worldpoint);
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, GridWorldSize.z));
    foreach (Node n in grid) 
        {
            Gizmos.color = (n.Walkable) ? Color.white : Color.red;
            Gizmos.DrawCube(n.WorldPosition, Vector3.one * (NodeSize - .1f));
        }
    }
}
