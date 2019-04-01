using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Range(0.01f , 1.0f)]
    public float smoothFactor = 0.5f;

    //CameraRotate 20190328
    private bool RotateAroundPlayer = true;
    private float RotateSpeed = 5.0f;
    private Vector3 _cameraOffset;

    private void Start()
    {
        _cameraOffset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (RotateAroundPlayer && Input.GetKey(KeyCode.R))
        {
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotateSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset;

        }
        Vector3 newPos = target.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        transform.LookAt(target);
    }
}
