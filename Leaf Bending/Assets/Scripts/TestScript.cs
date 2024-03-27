using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TestScript : MonoBehaviour
{
    public PlayerSO Leaf;
    private SkinnedMeshRenderer SMR;
    private MeshCollider MeshCollider;
    public GameObject Pointer, Player;
    public GameObject Bone1, Bone2, Bone3, Bone4, Bone5, Bone6;
    private float Distance;
    private float MaxValue;
    private float MinValue;
    public bool IsRotating;
    private int TimesOfBending;
    private bool IsNegative;
    private float RotationSpeed = 0.5f;
    private float RotateComeBack;
    private float Landed;
    private int Bending;

    void Start()
    {
        CreateLeaf();
        SMR = GetComponent<SkinnedMeshRenderer>();
        MeshCollider = GetComponent<MeshCollider>();

        if (SMR != null && MeshCollider != null)
        {
            MeshCollider.sharedMesh = SMR.sharedMesh;
        }
        else
        {
            Debug.Log("Pad³o");
        }
    }

    void Update()
    {
        if (Player != null)
        {
            Distance = Mathf.Abs(Pointer.transform.position.x - Player.transform.position.x) + Mathf.Abs(Pointer.transform.position.z - Player.transform.position.z);
            Distance = (Distance / Bending);
            Distance = -Distance;
            Distance = Distance * Player.GetComponent<Player>().PlayerWeight;
            RotateComeBack = Player.GetComponent<Player>().PlayerWeight;
        }
        UpdateMeshCollider();
        if (Player == null && IsRotating == false)
        {
            LeafBendingReturn();
        }
        else if (Player != null && IsRotating == false)
        {
            LeafBendingByWalking();
        }
        else if (IsRotating == true)
        {
            LeafBendingRotating();
        }
    }
    void UpdateMeshCollider()
    {
        if (SMR != null && MeshCollider != null)
        {
            Mesh mesh = new Mesh();
            SMR.BakeMesh(mesh);
            MeshCollider.sharedMesh = mesh;
        }
    }
    private void LeafBendingByWalking()
    {
        RotateBones(Bone1);
        RotateBones(Bone2);
        RotateBones(Bone3);
        RotateBones(Bone4);
        RotateBones(Bone5);
        RotateBones(Bone6);
    }
    private void RotateBones(GameObject Bone)
    {
        if (Distance < -2)
        {
            RotationSpeed = 0.5f;
            float TargetRotationAngle = Mathf.Atan2(Distance, 1) * 1.2f * Mathf.Rad2Deg;
            float CurrentRotation = Bone.transform.rotation.eulerAngles.z;
            float NewRotation = Mathf.LerpAngle(CurrentRotation, TargetRotationAngle, RotationSpeed * Time.deltaTime);
            Bone.transform.rotation = Quaternion.Euler(Bone.transform.rotation.eulerAngles.x, Bone.transform.rotation.eulerAngles.y, NewRotation);
        }
        else
        {
            LeafBendingReturn();
        }
    }
    private void RotateBonesReturn(GameObject Bone)
    {
        RotationSpeed = Landed / 6;
        float TargetRotationAngle = Mathf.Atan2(-2.28f, 1) * 1.2f * Mathf.Rad2Deg;
        float CurrentRotation = Bone.transform.rotation.eulerAngles.z;
        float NewRotation = Mathf.LerpAngle(CurrentRotation, TargetRotationAngle, RotationSpeed * Time.deltaTime);
        Bone.transform.rotation = Quaternion.Euler(Bone.transform.rotation.eulerAngles.x, Bone.transform.rotation.eulerAngles.y, NewRotation);
    }
    private void LeafBendingReturn()
    {
        RotateBonesReturn(Bone1);
        RotateBonesReturn(Bone2);
        RotateBonesReturn(Bone3);
        RotateBonesReturn(Bone4);
        RotateBonesReturn(Bone5);
        RotateBonesReturn(Bone6);
    }
    private void LeafBendingRotating()
    {
        RotateBonesRotation(Bone1);
        RotateBonesRotation(Bone2);
        RotateBonesRotation(Bone3);
        RotateBonesRotation(Bone4);
        RotateBonesRotation(Bone5);
        RotateBonesRotation(Bone6);
    }
    private void RotateBonesRotation(GameObject Bone)
    {
        if (TimesOfBending > 0)
        {
            if (MaxValue < -3)
            {
                RotationSpeed = Landed / 4;
                float TargetRotationAngle = Mathf.Atan2(MinValue, 1) * 1.2f * Mathf.Rad2Deg;
                float CurrentRotation = Bone.transform.rotation.eulerAngles.z;
                float NewRotation = Mathf.LerpAngle(CurrentRotation, TargetRotationAngle, RotationSpeed * Time.deltaTime);
                Bone.transform.rotation = Quaternion.Euler(Bone.transform.rotation.eulerAngles.x, Bone.transform.rotation.eulerAngles.y, NewRotation);
                if (IsNegative)
                {
                    MinValue -= Time.deltaTime;
                    if (MinValue < (MaxValue * 1.4f) && TimesOfBending < 3)
                    {
                        IsRotating = false;
                    }
                    if (MinValue < (MaxValue * 1.6f) && TimesOfBending < 3)
                    {
                        if (Player == null)
                        {
                            IsRotating = false;
                        }
                    }
                    else if (MinValue < (MaxValue * 1.8f))
                    {
                        IsNegative = false;
                        TimesOfBending--;
                        RotationSpeed = Landed / 6;
                        return;
                    }
                }
                else if (!IsNegative)
                {
                    MinValue += Time.deltaTime;
                    if (MinValue > (MaxValue * 0.6f))
                    {
                        if (Player == null)
                        {
                            IsRotating = false;
                        }
                        IsNegative = true;
                        TimesOfBending--;
                        RotationSpeed = 0.6f / RotateComeBack;
                    }
                }
            }
            else
            {
                IsRotating = false;
            }
        }
        else
        {
            IsRotating = false;
        }
    }
    public void SetMaxValue()
    {
        Distance = Mathf.Abs(Pointer.transform.position.x - Player.transform.position.x) + Mathf.Abs(Pointer.transform.position.z - Player.transform.position.z);
        Distance = (Distance / Bending);
        Distance = -Distance;
        Distance = Distance - Player.GetComponent<Player>().PlayerWeight;
        MaxValue = Distance;
        MinValue = MaxValue;
        TimesOfBending = 3;
        IsNegative = true;
        Landed = Player.GetComponent<Player>().PlayerWeight;
    }
    void CreateLeaf()
    {
        Bending = Leaf.Bending;
    }
}