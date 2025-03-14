using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NavController : MonoBehaviour {

    public AStar AStar;
    private Transform destination;
    private bool initialized = false;
    private List<Node> path = new List<Node>();
    private int currNodeIndex = 0;
    private float maxDistance = 1.1f;

    private void Start() {
        InitNav();
    }

    /// <summary>
    /// Returns the closest node to the given position.
    /// </summary>
    /// <returns>The closest node.</returns>
    /// <param name="point">Point.</param>
    Node ReturnClosestNode(Node[] nodes, Vector3 point) {
        float minDist = Mathf.Infinity;
        Node closestNode = null;
        foreach (Node node in nodes) {
            float dist = Vector3.Distance(node.pos, point);
            if (dist < minDist) {
                closestNode = node;
                minDist = dist;
            }
        }
        return closestNode;
    }

    void InitNav(){
        if (!initialized) {
            initialized = true;
            Debug.Log("INTIALIZING NAVIGATION!!!");
            Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
            Debug.Log("NODES: " + allNodes.Length);
            Node closestNode = ReturnClosestNode(allNodes, transform.position);
            Debug.Log("closest: " + closestNode.gameObject.name);
            Node target = FindAnyObjectByType<DiamondBehavior>().GetComponent<Node>();
            Debug.Log("target: " + target.gameObject.name);
            //set neighbor nodes for all nodes
            foreach (Node node in allNodes) {
                node.FindNeighbors(maxDistance);
            }

            //get path from A* algorithm
            path = AStar.FindPath(closestNode, target, allNodes);

            if (path == null) {
                //increase search distance for neighbors
                maxDistance += .1f;
                Debug.Log("Increasing search distance: " + maxDistance);
                initialized = false;
                InitNav();
                return;
            }

            //set next nodes 
            for (int i = 0; i < path.Count - 1; i++) {
                path[i].NextInList = path[i + 1];
            }
            //activate first node
            path[0].Activate(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        print("Triggered before compare");
        if (other.CompareTag("waypoint")) {
            print("Triggered");
            currNodeIndex = path.IndexOf(other.GetComponent<Node>());
            if (currNodeIndex < path.Count - 1) {
                path[currNodeIndex + 1].Activate(true);
            }
        }
    }
}
