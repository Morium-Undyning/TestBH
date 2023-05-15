using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
#region (Parametrs)
    #region (Floats)
    float speedRotY;
    float speedRotX;
    public float Mouse1 = 200f;
    #endregion
    #region (GameObject)
    public GameObject Player;
    #endregion
#endregion
#region (Voids)
    #region (Default voids)
    void Update()
    {
        Camera();
    }
    #endregion
    #region (Options voids)
    void Camera()
    {
        speedRotY = Input.GetAxis("Mouse X") * Mouse1 * Time.deltaTime;
        speedRotX = Input.GetAxis("Mouse Y") * Mouse1 * Time.deltaTime;
        Player.transform.Rotate(speedRotY*new Vector3(0, 1, 0) );
        transform.Rotate(-speedRotX * new Vector3(1, 0, 0));
    }
    #endregion
#endregion
}
