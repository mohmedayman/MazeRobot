using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    PlayerGrid grid;
    //public PlayerMovement playerMovement;
    void Awake() {
        grid = GetComponent<PlayerGrid>();

    }
    private void Start()
    {
        //FindPathAStar(seeker.position, target.position);
        //FindPathDFS(seeker.position, target.position);
        //FindPathBFS(seeker.position, target.position);
    }
    void Update() {
        //FindPathAStar(seeker.position, target.position);
        //FindPathDFS(seeker.position, target.position);
        //FindPathBFS(seeker.position, target.position);


    }
    public void ApplyDFS()
    {
        FindPathDFS(seeker.position, target.position);
    }public void ApplyBFS()
    {
        FindPathBFS(seeker.position, target.position);
    }public void ApplyAStar()
    {
        FindPathAStar(seeker.position, target.position);
    }


    void FindPathDFS(Vector3 startPos, Vector3 targetPos)
    {
        PlayerNode startNode = grid.NodeFromWorldPoint(startPos);
        PlayerNode targetNode = grid.NodeFromWorldPoint(targetPos);

        Stack<PlayerNode> stack = new Stack<PlayerNode>();
        HashSet<PlayerNode> visited = new HashSet<PlayerNode>();

        stack.Push(startNode);

        while (stack.Count > 0)
        {
            PlayerNode currentNode = stack.Pop();

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            if (!visited.Contains(currentNode))
            {
                visited.Add(currentNode);
                foreach (PlayerNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (neighbour.walkable && !visited.Contains(neighbour))
                    {
                        stack.Push(neighbour);
                        neighbour.parent = currentNode;
                    }
                }
            }
        }
    }

    void FindPathBFS(Vector3 startPos, Vector3 targetPos)
    {
        PlayerNode startNode = grid.NodeFromWorldPoint(startPos);
        PlayerNode targetNode = grid.NodeFromWorldPoint(targetPos);

        Queue<PlayerNode> queue = new Queue<PlayerNode>();
        HashSet<PlayerNode> visited = new HashSet<PlayerNode>();

        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            PlayerNode currentNode = queue.Dequeue();

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            if (!visited.Contains(currentNode))
            {
                visited.Add(currentNode);
                foreach (PlayerNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (neighbour.walkable && !visited.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        neighbour.parent = currentNode;
                    }
                }
            }
        }
    }
    void FindPathAStar(Vector3 startPos, Vector3 targetPos){
        PlayerNode startNode = grid.NodeFromWorldPoint(startPos);
        PlayerNode targetNode = grid.NodeFromWorldPoint(targetPos);

        //create open and closed set
        List<PlayerNode> openSet = new List<PlayerNode>();
        HashSet<PlayerNode> closedSet = new HashSet<PlayerNode> ();

        openSet.Add(startNode);

        while (openSet.Count > 0){
        // while openset is not empty
            PlayerNode currentNode = openSet[0];
            //search for next move
            for (int i =1; i < openSet.Count; i++){
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost ==currentNode.fCost && openSet[i].hCost < currentNode.hCost){
                    currentNode = openSet[i];
                }
            }
            // mark visited node from openset and add it to closed (visited) set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            // if i arrive at destination or not
            if (currentNode == targetNode){
                RetracePath(startNode,  targetNode);
                return;
            }
            foreach (PlayerNode neighbour in grid.GetNeighbours(currentNode)){
                if (!neighbour.walkable || closedSet.Contains(neighbour)){
                    continue;
                }
                //distance to the new neighbor is the distance walked + new c->n distance
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                //if node is not visited or 
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if (!openSet.Contains(neighbour)){
                        openSet.Add(neighbour);
                    }
                }
            }
        }


    }
    void RetracePath(PlayerNode startNode, PlayerNode endNode){
        List <PlayerNode> path = new List<PlayerNode>();
        PlayerNode currentNode = endNode;

        while (currentNode != startNode){
            path.Add(currentNode);
            currentNode= currentNode.parent;
        }
        path.Reverse();
        grid.path=path;
        // Assuming the PlayerMovement script is not attached to the same GameObject
        PlayerMovement playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetPath(path);
        }
        else
        {
            Debug.LogError("PlayerMovement script not found.");
        }
    }
    int GetDistance (PlayerNode nodeA, PlayerNode nodeB){
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (dstX >dstY){
            return  14* dstY + 10*(dstX-dstY);
        }
        return  14* dstX + 10*(dstY-dstX);
    }
}
