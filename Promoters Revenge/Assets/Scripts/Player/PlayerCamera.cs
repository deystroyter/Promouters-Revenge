using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        //Player GameObject

        [SerializeField] private Transform _target;

        [Serializable]
        public struct Distance
        {
            [SerializeField] public float Curr
            {
                get => _curr;
                set => _curr = Mathf.Clamp(value, _min, _max);
            }

            private float _curr;
            [SerializeField] private float _min;
            [SerializeField] private float _max;

            public Distance(float curr, float min, float max)
            {
                _min = min;
                _max = max;
                _curr = Mathf.Clamp(curr, min, max);
            }
        }

        [Header("Camera Distance")]
        [SerializeField] private Distance _distY;

        [SerializeField] private Distance _distZ;

        [Header("Mouse Scroll")]
        public float ScrollSensivity = 10f;

        private const string MouseScrollWheelInputString = "Mouse ScrollWheel";
        private Vector3 _cameraVelocity = Vector3.zero;

        [Header("Smooth Bamp")]
        [SerializeField] [Range(0.01f, 1.00f)] private float _smoothTime = 0.6f;

        protected void Start()
        {
        }

        protected void LateUpdate()
        {
            CameraMoveAndFollowLogic();
        }

        private void CameraMoveAndFollowLogic()
        {
            var wasAxisY = Input.GetMouseButton(1) ? 1 : 0;
            var wasAxisZ = Input.GetMouseButton(1) ? 0 : 1;
            var axisOffset = Input.GetAxis(MouseScrollWheelInputString) * ScrollSensivity;

            _distY.Curr += axisOffset * wasAxisY;
            _distZ.Curr += -axisOffset * wasAxisZ;

            var newPos = new Vector3(_target.position.x,
                _target.position.y + _distY.Curr,
                _target.position.z - _distZ.Curr);


            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _cameraVelocity, _smoothTime);
        }
    }
}