using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 20)] [SerializeField] private float speed = 15;

        //public float MouseSensitivity = 4f;

        [Header("Running")]
        [SerializeField] private bool canRun = true;

        public bool IsRunning { get; private set; }

        [SerializeField] [Range(1, 20)] private float runSpeed = 20;

        private KeyCode _runningKey = KeyCode.LeftShift;
        private Rigidbody _rigidbody;
        private Camera _camera;

        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void Start()
        {
            _camera = Camera.main;
        }

        // ??? Если использовать обычный Update, то персонаж начинает дёргаться
        protected void FixedUpdate()
        {
            TurnLogic();
            MovementLogic();
        }

        private void MovementLogic()
        {
            IsRunning = canRun && Input.GetKey(_runningKey);
            var targetMovingSpeed = IsRunning ? runSpeed : speed;
            var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            if (inputDirection.magnitude < 1e-2)
            {
                return;
            }

            _rigidbody.velocity = inputDirection * targetMovingSpeed * 100 * Time.deltaTime;
        }


        private void TurnLogic()
        {
            var mousePosition = Input.mousePosition;
            var viewportPosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
            var ray = _camera.ViewportPointToRay(viewportPosition);

            if (Physics.Raycast(ray, out var hit) && hit.transform != transform)
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z), Vector3.up);

                Debug.DrawRay(ray.origin, ray.direction, Color.red, 2f);
            }
        }
    }
}