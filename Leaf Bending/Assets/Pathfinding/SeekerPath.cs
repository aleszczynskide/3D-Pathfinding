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
            pathfinding.FindPath(seeker.position, target.position);
            FollowPath();
        }
    }

    private void FollowPath()
    {
        if (pathfinding.grid != null && pathfinding.grid.path != null && pathfinding.grid.path.Count > 0)
        {
            if (pathIndex < pathfinding.grid.path.Count)
            {
                Node targetNode = pathfinding.grid.path[pathIndex];
                seeker.GetComponent<Pathfinding>().Target.transform.position = targetNode.WorldPosition;
                Vector2 targetPosition = targetNode.WorldPosition;

                Vector2 moveDir = (targetPosition - (Vector2)seeker.position).normalized;
                Vector2 moveAmount = moveDir * Time.deltaTime * moveSpeed; 

                seeker.position += (Vector3)moveAmount;

                float distanceToTarget = Vector2.Distance(seeker.position, targetPosition);
                if (distanceToTarget < pathfinding.grid.NodeRadius)
                {
                    pathIndex++;
                }
            }
        }
    }
}