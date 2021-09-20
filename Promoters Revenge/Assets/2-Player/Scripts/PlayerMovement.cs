
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 20)]
    public float speed = 10;
    //public float MouseSensitivity = 4f;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    [Range(1, 20)]
    public float runSpeed = 15;
    public KeyCode runningKey = KeyCode.LeftShift;
    new Rigidbody rigidbody;

    Camera PlayerCamera;
    Quaternion CameraCorrection;
    float forwardBackwardInput = 0f;
    float leftRightInput = 0f;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        PlayerCamera = Camera.main;
    }

    void FixedUpdate()
    {
        #region TurnAround

        //TODO: Slow rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == transform.gameObject) { return; }
            hit.point = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(hit.point, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.time * speed);
            //Debug.Log(hit.transform.gameObject + " --------  " + hit.point);

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 2f);
        }

        #endregion TurnAround

        #region Movement

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
            rigidbody.velocity = targetVelocity + Vector3.forward;
        }
        #endregion Movement
    }
}
