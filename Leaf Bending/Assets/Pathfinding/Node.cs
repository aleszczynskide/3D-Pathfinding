using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GCost;
    public int HCost;
    public int GridX;
    public int GridY;
    public int GridZ;
    public Node Parent;

    public Node(bool _walkable, Vector3 _worldPos,int _gridx,int _gridy,int _gridz)
    {
        Walkable = _walkable;
        WorldPosition = _worldPos;
        GridX = _gridx;
        GridY = _gridy;
        GridZ = _gridz;
    }
    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }
}
