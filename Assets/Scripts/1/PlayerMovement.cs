using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    Vector2 playerInput;
    Rigidbody rb;
    [SerializeField] Transform cameraMovement;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        readInput();
        writeInputs();
    }
    void readInput()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }
    void writeInputs()
    {
        Vector3 cameraForward = cameraMovement.forward;
        Vector3 cameraRight = cameraMovement.right;
        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movementTotal = cameraForward * playerInput.y + cameraRight * playerInput.x;
        movementTotal.Normalize();

        Vector3 v = rb.linearVelocity;
        v.x = movementTotal.x * speed;
        v.z = movementTotal.z * speed;
        //Vector3 force = new Vector3(playerInput.x, 0, playerInput.y);
        //rb.AddForce(movementTotal * speed);
        rb.linearVelocity = v;
    }
}
