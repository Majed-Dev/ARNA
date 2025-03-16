using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    public List<Node> FindPath(Node startNode, Node targetNode, Node[] allNodes) {
        List<Node> openSet = new List<Node>();
        openSet.Add(startNode);

        List<Node> closedSet = new List<Node>();

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].totalCost < currentNode.totalCost
                    || (openSet[i].totalCost.Equals(currentNode.totalCost)
                        && openSet[i].heuristicCost < currentNode.heuristicCost)) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                Debug.Log("RETURNING CORRECT NODE!!");
                return RetracePath(startNode, targetNode);
            }

            foreach (Node connection in currentNode.neighbors) {
                if (!closedSet.Contains(connection)) {
                    float costToConnection = currentNode.pathCost + GetEstimate(currentNode, connection) + connection.traversalCost;

                    if (costToConnection < connection.pathCost || !openSet.Contains(connection)) {
                        connection.pathCost = costToConnection;
                        connection.heuristicCost = GetEstimate(connection, targetNode);
                        connection.Parent = currentNode;

                        if (!openSet.Contains(connection)) {
                            openSet.Add(connection);
                        }
                    }
                }
            }
        }
        Debug.Log("RETURNING NULL");
        return null;
    }

    private static List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        return path;
    }

    private float GetEstimate(Node first, Node second) {
    float xDistance = Mathf.Abs(first.pos.x - second.pos.x);
    float yDistance = Mathf.Abs(first.pos.z - second.pos.z); // Assuming a 2D grid on the X-Z plane

    // Apply diagonal movement cost
    if (xDistance > yDistance) {
        return 14 * yDistance + 10 * (xDistance - yDistance);
    } else {
        return 14 * xDistance + 10 * (yDistance - xDistance);
    }

    // Straight movement -> 10
    // Diagonal movement -> 14
}

}