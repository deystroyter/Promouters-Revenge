using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //Player GameObject
    public Transform target;

    private Vector3 _position;

    public float ScrollSensivity = 1;
    public float ZoomSpeed = 9f;

    [Header("Y Axis")]
    [Range(1, 30)]
    public float CameraDistanceY = 10f;

    public float MinDistanceY = 1f;
    public float MaxDistanceY = 30f;

    [Header("Z Axis")]
    public float CameraDistanceZ = 10f;

    public float MinDistanceZ = 1f;
    public float MaxDistanceZ = 20f;

    private void Start()
    {
        transform.LookAt(target);
        //Vector3 DistanceVector = transform.TransformPoint(transform.position);
    }

    private void LateUpdate()
    {
        //Sphere Effect to Zoom
        transform.LookAt(target);

        #region CameraFollow XZ

        if (CameraDistanceZ >= MinDistanceZ && CameraDistanceZ <= MaxDistanceZ)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z - CameraDistanceZ), 0.2f);
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z - CameraDistanceZ), 0.2f);
        }

        #endregion CameraFollow XZ

        //#TODO: Zooming at center of player model + *Add - Camera Bump with zoom

        #region CameraMove

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Debug.LogWarning(Input.GetMouseButtonDown(1));
            if (Input.GetMouseButton(1))
            {
                //Y Axis
                CameraDistanceY -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
                if (CameraDistanceY < MinDistanceY) CameraDistanceY = MinDistanceY;
                if (CameraDistanceY > MaxDistanceY) CameraDistanceY = MaxDistanceY;

                transform.position = Vector3.Slerp(transform.position, new Vector3(target.position.x, target.position.y + CameraDistanceY, transform.position.z), 1f);
                return;
            }
            //Z Axis
            CameraDistanceZ -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            if (CameraDistanceZ < MinDistanceZ) CameraDistanceZ = MinDistanceZ;
            if (CameraDistanceZ > MaxDistanceZ) CameraDistanceZ = MaxDistanceZ;

            transform.position = Vector3.Slerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z - CameraDistanceZ), 1f);
        }

        #endregion CameraMove

        #region CameraEffects

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position = Vector3.Slerp(transform.position, transform.position + transform.forward.normalized * 30, 0.1f);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            transform.position = Vector3.Slerp(transform.position, transform.position - transform.forward.normalized * 30, 0.2f);
        }

        #endregion CameraEffects
    }
}