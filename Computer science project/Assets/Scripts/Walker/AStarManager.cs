using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        if (start == null || end == null)
        {
            Debug.LogError("Start or End node is null!");
            return null;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        // Reset all nodes
        Node[] allNodes = FindObjectsOfType<Node>();
        foreach (Node node in allNodes)
        {
            node.gScore = float.MaxValue;  // Reset gScore
            node.hScore = 0;               // Reset hScore
            node.cameFrom = null;          // Reset cameFrom
        }

        // Initialize start node
        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            // Find node with lowest F-score
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < currentNode.FScore())
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // If we reached the end, reconstruct the path
            if (currentNode == end)
            {
                return ReconstructPath(end);
            }

            // Process neighbors
            foreach (Node neighbor in currentNode.connections)
            {
                if (closedSet.Contains(neighbor)) continue; // Ignore visited nodes

                float tentativeGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, neighbor.transform.position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= neighbor.gScore)
                {
                    continue; // Skip if the new path is worse
                }

                // Update the neighbor's properties
                neighbor.cameFrom = currentNode; // Link the node to the parent (currentNode)
                neighbor.gScore = tentativeGScore; // Update the gScore for this neighbor
                neighbor.hScore = Vector2.Distance(neighbor.transform.position, end.transform.position); // Recalculate heuristic
            }
        }

        Debug.LogWarning("No valid path found!");
        return null; // No valid path
    }

    private List<Node> ReconstructPath(Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != null)
        {
            path.Insert(0, current);
            current = current.cameFrom;
        }

        return path;
    }
}