using System.Collections;
using System.Collections.Generic;
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
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>(); // To avoid revisiting nodes

        foreach (Node n in FindObjectsOfType<Node>())
        {
            n.gScore = float.MaxValue;
            n.cameFrom = null; // Reset path
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            // Find node with the lowest F-score
            int lowestF = 0;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.RemoveAt(lowestF);
            closedSet.Add(currentNode);

            // If we reached the end, reconstruct the path
            if (currentNode == end)
            {
                List<Node> path = new List<Node>();
                while (currentNode != null) // Build the path by backtracking
                {
                    path.Insert(0, currentNode);
                    currentNode = currentNode.cameFrom;
                }
                return path;
            }

            // Check neighbors
            foreach (Node connectedNode in currentNode.connections)
            {
                if (closedSet.Contains(connectedNode)) continue; // Ignore visited nodes

                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                if (!openSet.Contains(connectedNode))
                {
                    openSet.Add(connectedNode);
                }
                else if (heldGScore >= connectedNode.gScore) // Skip if no better path
                {
                    continue;
                }

                // Update node with better path
                connectedNode.cameFrom = currentNode;
                connectedNode.gScore = heldGScore;
                connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);
            }
            Debug.Log($"Processing node: {currentNode.name}, F-score: {currentNode.FScore()}");
        }

        return null; // No valid path found
    }
}