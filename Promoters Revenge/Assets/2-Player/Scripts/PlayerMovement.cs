using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 20)]
    public float speed = 15;

    //public float MouseSensitivity = 4f;

    [Header("Running")]
    public bool canRun = true;

    public bool IsRunning { get; private set; }

    [Range(1, 20)]
    public float runSpeed = 20;

    public KeyCode runningKey = KeyCode.LeftShift;
    private new Rigidbody rigidbody;

    private float forwardBackwardInput = 0f;
    private float leftRightInput = 0f;

    private void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        TurnLogic();
        MovementLogic();
    }

    private void MovementLogic()
    {
        leftRightInput = Input.GetAxis("Horizontal");
        forwardBackwardInput = Input.GetAxis("Vertical");
        if (forwardBackwardInput != 0 || leftRightInput != 0)
        {
            //Vector3 CameraCorrection = new Vector3(PlayerCamera.transform.forward.x, 0f, PlayerCamera.transform.forward.z);

            // Update IsRunning from input.
            IsRunning = canRun && Input.GetKey(runningKey);

            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            // Get targetVelocity from input.
            Vector3 targetVelocity = new Vector3(leftRightInput, 0, forwardBackwardInput).normalized * targetMovingSpeed;

            // Apply movement.
            rigidbody.velocity = targetVelocity;
        }
    }

    //TODO: Slow rotation
    private void TurnLogic()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == transform.gameObject) { return; }
            hit.point = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(hit.point, Vector3.up);

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 2f);
        }
    }
}