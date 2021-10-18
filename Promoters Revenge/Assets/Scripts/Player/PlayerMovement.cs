using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 20)] [SerializeField] private float speed = 15;

        //public float MouseSensitivity = 4f;

        [Header("Running")]
        [SerializeField] private bool _canRun = true;

        [SerializeField] private Animator _anim;
        private float _animationInterpolation = 1f;
        public bool IsRunning { get; private set; }

        [SerializeField] [Range(1, 20)] private float _runSpeed = 20;

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

        // ??? ≈сли использовать обычный Update, то персонаж начинает дЄргатьс€
        protected void FixedUpdate()
        {
            TurnLogic();
            MovementLogic();
        }

        private void MovementLogic()
        {
            IsRunning = _canRun && Input.GetKey(_runningKey);
            var targetMovingSpeed = IsRunning ? _runSpeed : speed;
            var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;


            if (inputDirection.magnitude < 1e-2)
            {
                return;
            }

            _rigidbody.velocity = inputDirection * targetMovingSpeed * 100 * Time.deltaTime;

            // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
            _animationInterpolation = Mathf.Lerp(_animationInterpolation, 1f, Time.deltaTime * 3);

            var Z_Vector = transform.forward * Input.GetAxis("Vertical") * _animationInterpolation;
            Debug.Log($"Z_Vector || {Z_Vector}");
            Debug.DrawRay(transform.forward, Z_Vector, Color.blue);

            var X_Vector = transform.right * Input.GetAxis("Horizontal") * _animationInterpolation;
            Debug.Log($"X_Vector || {X_Vector}");
            Debug.DrawRay(transform.forward, X_Vector, Color.blue);

            //_anim.SetFloat("Local X", X_Vector);
            //_anim.SetFloat("Local Z", Z_Vector);


            Debug.DrawRay(Vector3.zero, X_Vector * 100, Color.red);
            Debug.DrawRay(Vector3.zero, Z_Vector * 100, Color.blue);
            //currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
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