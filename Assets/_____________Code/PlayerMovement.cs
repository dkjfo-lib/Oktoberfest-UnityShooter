using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    public float speed = 200;
    public float speedMult = .7f;
    [Space]
    public float speedMultInAir = .98f;
    public float controlInAir = .2f;
    public float jumpForce = 200;

    Rigidbody rb;
    GroundDetector gd;

    Vector3 input;
    public Vector3 CurrentInput => input;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gd = GetComponentInChildren<GroundDetector>();
    }

    void FixedUpdate()
    {
        var input = CreateInput();

        if (gd.onGround)
        {
            // move xz
            if (input.x != 0 || input.z != 0)
            {
                var inputXZ = new Vector3(input.x, 0, input.z).normalized;
                var addMovementXZ = inputXZ * speed * Time.fixedDeltaTime;
                var newMovementXZ = rb.velocity + addMovementXZ;

                rb.velocity = newMovementXZ;
            }
            rb.velocity = new Vector3(rb.velocity.x * speedMult, rb.velocity.y, rb.velocity.z * speedMult);
            // move y
            if (input.y != 0)
            {
                var movementY = input.y * jumpForce;
                rb.velocity = new Vector3(rb.velocity.x, movementY, rb.velocity.z);
            }
        }
        else
        {
            // move xz
            if (input.x != 0 || input.z != 0)
            {
                var inputXZ = new Vector3(input.x, 0, input.z).normalized;
                var addMovementXZ = inputXZ * speed * Time.fixedDeltaTime * controlInAir;
                var newMovementXZ = rb.velocity + addMovementXZ;

                rb.velocity = newMovementXZ;
            }
            rb.velocity = new Vector3(rb.velocity.x * speedMultInAir, rb.velocity.y, rb.velocity.z * speedMultInAir);
            // move y
            if (input.y != 0)
            {
                var movementY = input.y * jumpForce;
                rb.velocity = new Vector3(rb.velocity.x, movementY, rb.velocity.z);
            }
        }
    }

    private Vector3 CreateInput()
    {
        input = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            input -= Vector3.right;
        if (Input.GetKey(KeyCode.D))
            input += Vector3.right;
        if (Input.GetKey(KeyCode.W))
            input += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            input -= Vector3.forward;
        if (Input.GetKey(KeyCode.Space))
            input -= Vector3.up;
        return input;
    }
}

public interface IMovement
{
    Vector3 CurrentInput { get; }
}