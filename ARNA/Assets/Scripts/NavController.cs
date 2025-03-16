using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavController : MonoBehaviour
{
    public AStar AStar;
    private Transform destination;
    public bool initialized = false;
    private List<Node> path = new List<Node>();
    public List<Destination> destinations = new List<Destination>();
    private int currNodeIndex = 0;
    private float maxDistance = 1.1f;
    [SerializeField] private int activeDestinationIndex;
    
    [SerializeField] private Button backButton; // Reference to UI back button

    private void Start()
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(false); // Hide button at start
            backButton.onClick.AddListener(StopNavigation); // Assign function
        }
    }

    private void Update()
    {
        activeDestinationIndex = GameObject.Find("Dropdown").GetComponent<GetValueFromDropdown>().GetDropdownValue();
    }

    public void SetActiveDestination(int index)
    {
        activeDestinationIndex = index;
    }

    public void InitNav()
    {
        if (!initialized)
        {
            initialized = true;
            Debug.Log("Initializing Navigation!");

            Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
            Debug.Log("Nodes: " + allNodes.Length);

            Node closestNode = ReturnClosestNode(allNodes, transform.position);
            Debug.Log("Closest: " + closestNode.gameObject.name);

            Node target = destinations[activeDestinationIndex].GetComponent<Node>();
            Debug.Log("Target: " + target.gameObject.name);

            foreach (Node node in allNodes)
            {
                node.FindNeighbors(maxDistance);
            }

            path = AStar.FindPath(closestNode, target, allNodes);

            if (path == null)
            {
                maxDistance += 0.1f;
                Debug.Log("Increasing search distance: " + maxDistance);
                initialized = false;
                InitNav();
                return;
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                path[i].NextInList = path[i + 1];
            }

            path[0].Activate(true);

            if (backButton != null)
                backButton.gameObject.SetActive(true); // Show back button when navigation starts
        }
    }

    public void StopNavigation()
    {
        Debug.Log("Stopping Navigation!");
        initialized = false;
        foreach (Node node in path)
        {
            node.Activate(false); // Deactivate path nodes
        }
        path.Clear();

        if (backButton != null)
            backButton.gameObject.SetActive(false); // Hide back button
    }

    private Node ReturnClosestNode(Node[] nodes, Vector3 point)
    {
        float minDist = Mathf.Infinity;
        Node closestNode = null;
        foreach (Node node in nodes)
        {
            float dist = Vector3.Distance(node.pos, point);
            if (dist < minDist)
            {
                closestNode = node;
                minDist = dist;
            }
        }
        return closestNode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("waypoint"))
        {
            currNodeIndex = path.IndexOf(other.GetComponent<Node>());
            if (currNodeIndex < path.Count - 1)
            {
                path[currNodeIndex + 1].Activate(true);
            }
        }
    }
}
