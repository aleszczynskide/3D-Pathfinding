using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMovement : MonoBehaviour
{
    public Transform seeker;
    public Transform target;
    public float moveSpeed = 5f;

    private Pathfinding pathfinding;

    private int pathIndex = 0;

    private void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    private void Update()
    {
        if (pathfinding != null)
        {
            if (seeker.transform.position != target.position) 
            {
                FollowPath();
            }
           
        }
    }

    private void FollowPath()
    {
        if (pathfinding.grid != null && pathfinding.grid.path != null && pathfinding.grid.path.Count > 0)
        {
            if (pathIndex < pathfinding.grid.path.Count)
            {
                Node targetNode = pathfinding.grid.path[pathIndex];
                Vector3 targetPosition = targetNode.WorldPosition;
                Vector3 moveDir = (targetPosition - seeker.position).normalized;
                Vector3 moveAmount = moveDir * Time.deltaTime * moveSpeed;
                seeker.position = Vector3.Lerp(seeker.position, targetPosition, Time.deltaTime * moveSpeed);
                if (Vector3.Distance(seeker.position, targetPosition) < 0.1f)
                {
                    pathIndex++;
                }
            }
        }
    }


}