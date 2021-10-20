using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 20)] [SerializeField] private float speed = 15;

        //public float MouseSensitivity = 4f;

        [Header("Running")]
        [SerializeField] private bool _canRun = true;

        [SerializeField] private Animator _anim;
        private float _animKoef = 0.5f;
        private float _animationInterpolation = 1f;

        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                _isRunning = value;
                if (IsRunning)
                {
                    _animKoef = 1f;
                }
                else
                {
                    _animKoef = 0.5f;
                }
            }
        }
        private bool _isRunning;

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

        // ??? Если использовать обычный Update, то персонаж начинает дёргаться
        protected void FixedUpdate()
        {
            TurnLogic();
            MovementLogic();
        }

        private void MovementLogic()
        {
            IsRunning = _canRun && Input.GetKey(_runningKey);
            var targetMovingSpeed = IsRunning ? _runSpeed : speed;


            var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * _animKoef;

            _animationInterpolation = Mathf.Lerp(_animationInterpolation, 1f, Time.deltaTime * 3);
            var test_Vector = transform.rotation * inputDirection;

            var y = transform.rotation.eulerAngles.y;

            if (y > 60 && y < 140 || y > 220 && y < 300)
            {
                _anim.SetFloat("Local Z", -test_Vector.z);
            }
            else
            {
                _anim.SetFloat("Local Z", test_Vector.z);
            }

            _anim.SetFloat("Local X", test_Vector.x);

            //Debug.Log("Y = " + y);

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