using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PathFinding pathfinding; // Reference to your PathFinding script

    private List<PlayerNode> path;
    private  int currentWaypoint = 0;

    void Start()
    {
        pathfinding = GetComponent<PathFinding>();
    }

    void Update()
    {
        if (path != null && path.Count > 0)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        
        Vector3 targetPosition = path[currentWaypoint].worldPosition;
        Debug.Log(targetPosition);
        Debug.Log(currentWaypoint);
        // Move towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the player has reached the current waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Debug.Log("here");
            currentWaypoint+=1;
            Debug.Log(currentWaypoint);
            // Check if all waypoints have been reached
            if (currentWaypoint >= path.Count)
            {
                Debug.Log("here2");
                // Reset variables for the next pathfinding operation
                path = null;
                currentWaypoint = 0;
            }

        }
        
    }

    public void SetPath(List<PlayerNode> newPath)
    {
        path = newPath;
        //currentWaypoint = 0;
        Debug.Log("problem");
    }
}
