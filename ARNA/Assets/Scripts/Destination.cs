using UnityEngine;

public class Destination : MonoBehaviour
{
   Vector3 rotate = new Vector3(0, 1, 0);
    public GameObject visual;

    private void Awake() {
        Activate(false);
    }

    // Update is called once per frame
    void Update() {
        transform.eulerAngles += rotate;
    }

    public void Activate(bool active) {
        visual.SetActive(active);
    }
}
