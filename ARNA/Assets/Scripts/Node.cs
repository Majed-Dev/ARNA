using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour {

    public Vector3 pos;

    [Header("A*")]
    public List<Node> neighbors = new List<Node>();
    public float totalCost { get { return pathCost + heuristicCost; } }
    public float heuristicCost { get; set; }
    public float pathCost { get; set; }
    public float traversalCost { get; set; }
    public Node Parent { get; set; }

    //next node in navigation list
    public Node NextInList { get; set; }

    private Vector3 scale;
    private bool isDestination = false;

    private void Awake() {
        transform.GetChild(0).gameObject.SetActive(false);
        //save scale
        scale = transform.localScale;
        //check if destination
        if (GetComponent<Destination>() != null)
        {
            isDestination = true;
        } 
#if UNITY_EDITOR
        pos = transform.position;
#endif
    }

    public void Activate(bool active) {
        transform.GetChild(0).gameObject.SetActive(active);
        if (NextInList != null) {
            transform.LookAt(NextInList.transform);
        }
    }

    void Update() {
        //make pulsate
        if (!isDestination)
            transform.localScale = scale * (1 + Mathf.Sin(Mathf.PI * Time.time) * .2f); 
    }

    public void FindNeighbors(float maxDistance) {
    neighbors.Clear(); // Clear previous neighbors
    foreach (Node node in FindObjectsByType<Node>(FindObjectsSortMode.None)) {
        if (node != this && Vector3.Distance(node.pos, this.pos) < maxDistance) {
            if (!neighbors.Contains(node)) { // Prevent duplicate entries
                neighbors.Add(node);
            }
        }
    }
}
}
