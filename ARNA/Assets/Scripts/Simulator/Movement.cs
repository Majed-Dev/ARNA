using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementZ = Input.GetAxisRaw("Vertical");

        transform.Translate(movementX * movementSpeed * Time.deltaTime, 0f, movementZ * movementSpeed * Time.deltaTime);
    }
}
