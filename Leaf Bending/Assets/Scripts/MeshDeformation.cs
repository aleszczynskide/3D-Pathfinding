using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
    Mesh DeformationMesh;
    Vector3[] StartingMesh, GoalMesh;
    void Start()
    {
        DeformationMesh = GetComponent<MeshFilter>().mesh;
        StartingMesh = DeformationMesh.vertices;
        GoalMesh = new Vector3[StartingMesh.Length];
        for (int i = 0; i < StartingMesh.Length; i++)
        {
           GoalMesh[i] = StartingMesh[i];
        }
    }

    void Update()
    {
        
    }
}
