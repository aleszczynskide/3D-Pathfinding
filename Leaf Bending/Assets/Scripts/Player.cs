using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public PlayerSO PlayerSO;
    [SerializeField] private int PlayerSpeed, JumpForce;  //Player Speed Int and jumppower
    Rigidbody Rb;
    private GameObject Leaf;
    RaycastHit Hit;
    public float Distance;
    public float PlayerWeight;
    public float Falling;
    private bool IsFalling;
    private bool IsGrounded;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }
    void Update()

    {

        //Player Movement Script
        Vector3 MoveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) MoveDirection.z = -1;
        if (Input.GetKey(KeyCode.S)) MoveDirection.z = +1;
        if (Input.GetKey(KeyCode.A)) MoveDirection.x = +1;
        if (Input.GetKey(KeyCode.D)) MoveDirection.x = -1;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            StartCoroutine(WaitForComeBack());
            Rb.velocity = new Vector3(0, +JumpForce, 0);
        }
        transform.position += MoveDirection * PlayerSpeed * Time.deltaTime;
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, 1.2f))
        {
            if (Hit.collider.CompareTag("Leaf") || (Hit.collider.CompareTag("Ground")))
            {
                IsGrounded = true;
            }
        }
        else
        {
            IsGrounded = false;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, Distance))
        {
            if (Hit.collider.CompareTag("Leaf"))
            {
                if (Rb.velocity.y < -0.5f)
                {
                    Leaf = Hit.collider.gameObject;
                    Leaf.GetComponent<TestScript>().Player = this.gameObject;
                    this.transform.SetParent(Leaf.GetComponent<TestScript>().Bone6.transform);
                    if (IsFalling == true)
                    {
                        Leaf.GetComponent<TestScript>().SetMaxValue();
                        Leaf.GetComponent<TestScript>().IsRotating = true;
                        IsFalling = false;
                    }
                    Falling = 0;
                    Distance = 8;
                }
            }
            else
            {
                Falling = 0;
                if (Leaf != null)
                {
                    Leaf.GetComponent<TestScript>().Player = null;
                    Leaf = null;
                    StartCoroutine(WaitForComeBack());
                }
            }
        }
        else
        {
            if (Leaf != null)
            {
                Leaf.GetComponent<TestScript>().Player = null;
                Leaf = null;
            }
            Falling += Time.deltaTime;
        }
        if (Falling > 1f) IsFalling = true;
    }
    IEnumerator WaitForComeBack()
    {
        if (PlayerWeight < 10)
        {
            yield return new WaitForSeconds(0.1f);
            Distance = 1.2f;
            if (Leaf != null)
            {
                Leaf.GetComponent<TestScript>().IsRotating = false;
            }
        }
        else if (PlayerWeight > 7)
        {
            yield return new WaitForSeconds(0.2f);
            Distance = 1.2f;
            if (Leaf != null)
            {
                Leaf.GetComponent<TestScript>().IsRotating = false;
            }
        }
    }
    private void CreatePlayer()
    {
        PlayerWeight = PlayerSO.PlayerWeight;
    }
}
