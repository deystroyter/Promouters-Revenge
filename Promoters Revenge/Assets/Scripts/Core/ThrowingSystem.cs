using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class ThrowingSystem : MonoBehaviour
    {
        public string ThrowingInput;
        public GameObject ProjectilePrefab;
        public GameObject EndPointCursorPrefab;

        public Transform ThrowingStartPoint;

        public LineRenderer LineRenderer;
        public int lineSegment = 10;

        public float maxThrowRadius = 10f;
        public float flightTime = 4f;

        private Camera _camera;
        private Transform _endPointCursor;

        // Start is called before the first frame update
        protected void Start()
        {
            _camera = Camera.main;
            LineRenderer.positionCount = lineSegment + 1;

            if (_endPointCursor == null)
            {
                _endPointCursor = Instantiate(EndPointCursorPrefab, Vector3.zero, Quaternion.identity).transform;
                _endPointCursor.SetParent(this.transform);
                _endPointCursor.localPosition = Vector3.zero;
                _endPointCursor.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            if (GameInput.Key.GetKeyDown(ThrowingInput))
            {
                LineRenderer.enabled = true;
                _endPointCursor.gameObject.SetActive(true);
            }
            else if (GameInput.Key.GetKeyUp(ThrowingInput))
            {
                LineRenderer.enabled = false;
                _endPointCursor.gameObject.SetActive(false);
            }

            if (GameInput.Key.GetKey(ThrowingInput))
            {
                ShowTrajectory();
            }
        }


        private void ShowTrajectory()
        {
            var viewportPosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            Ray cameraRay = _camera.ViewportPointToRay(viewportPosition);
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit))
            {
                var newHitPoint = hit.point - ThrowingStartPoint.position;
                var xzVector = new Vector2(newHitPoint.x, newHitPoint.z);

                if (xzVector.sqrMagnitude > Mathf.Pow(maxThrowRadius, 2))
                {
                    xzVector = new Vector2(ThrowingStartPoint.position.x, ThrowingStartPoint.position.z) + xzVector.normalized * maxThrowRadius;

                    newHitPoint = new Vector3(xzVector.x, hit.point.y, xzVector.y);
                    if (Physics.Raycast(newHitPoint, Vector3.down, out var yDownHit))
                    {
                        newHitPoint = yDownHit.point;
                    }
                    else
                    {
                        newHitPoint = new Vector3(xzVector.x, hit.point.y, xzVector.y);
                    }
                }
                else
                {
                    newHitPoint = hit.point;
                }

                Vector3 projectileVelocity = CalculateVelocity(newHitPoint, ThrowingStartPoint.position, flightTime);
                Visualize(projectileVelocity, newHitPoint);
                _endPointCursor.transform.position = newHitPoint;

                transform.rotation = Quaternion.LookRotation(projectileVelocity);

                if (Input.GetMouseButtonDown(0))
                {
                    var obj = Instantiate(ProjectilePrefab, ThrowingStartPoint.position, transform.rotation);
                    obj.GetComponent<Rigidbody>().velocity = projectileVelocity;
                    obj.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
                }
            }
        }

        private void Visualize(Vector3 projectileVelocity, Vector3 endPoint)
        {
            for (int i = 0; i < lineSegment; i++)
            {
                Vector3 pos = CalculatePositionInTime(projectileVelocity, (i / (float) lineSegment) * flightTime);
                LineRenderer.SetPosition(i, pos);
            }

            LineRenderer.SetPosition(lineSegment, endPoint);
        }

        private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
        {
            Vector3 distance = target - origin;
            Vector3 distanceXz = distance;
            distanceXz.y = 0f;

            float sY = distance.y;
            float sXz = distanceXz.magnitude;

            float Vxz = sXz / time;
            float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

            Vector3 result = distanceXz.normalized;
            result *= Vxz;
            result.y = Vy;

            return result;
        }

        private Vector3 CalculatePositionInTime(Vector3 projectileVelocity, float time)
        {
            Vector3 Vxz = projectileVelocity;
            Vxz.y = 0f;

            Vector3 result = ThrowingStartPoint.position + projectileVelocity * time;
            float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (projectileVelocity.y * time) + ThrowingStartPoint.position.y;

            result.y = sY;

            return result;
        }
    }
}