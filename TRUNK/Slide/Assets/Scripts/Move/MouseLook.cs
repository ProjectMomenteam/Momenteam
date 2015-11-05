using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    public float _Sensibility = 3;
    private Transform _CameraContainer;

    public float _Angle = 50f;
    public float _MaxCameraSpeed = 1;
    // Use this for initialization
    void Awake ()
    {
        _CameraContainer = transform.GetChild(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * _Sensibility, 0));

        if (-Input.GetAxis("Mouse Y") > 0 && (_CameraContainer.rotation.eulerAngles.x < _Angle || _CameraContainer.rotation.eulerAngles.x > 360f - 89f) || (-Input.GetAxis("Mouse Y") < 0 && (_CameraContainer.rotation.eulerAngles.x > 360f - (_Angle) || _CameraContainer.rotation.eulerAngles.x < 89f)))
        {
            _CameraContainer.Rotate(new Vector3(-Mathf.Clamp( Input.GetAxis("Mouse Y") * _Sensibility, -_MaxCameraSpeed, _MaxCameraSpeed), 0, 0));

        }
    }
}
