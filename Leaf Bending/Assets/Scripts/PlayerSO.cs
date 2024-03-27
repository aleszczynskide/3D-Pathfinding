using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeafBendingSO", menuName = "LeafBendingSO")]
public class PlayerSO : ScriptableObject
{
    [Header("The lower the value is,the more leaf can bend")]
    [SerializeField, Range(10, 1)]
    public int Bending;
    public int PlayerWeight;
}
