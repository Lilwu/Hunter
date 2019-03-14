using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distanceH = 7f;
    public float distanceV = 4f;
    public float smoothSpeed = 10f;

    private void LateUpdate()
    {
        Vector3 nextpos = Vector3.forward * -1 * distanceH + Vector3.up * distanceV + target.position;
        this.transform.position = Vector3.Lerp(this.transform.position , nextpos , smoothSpeed * Time.deltaTime);
        this.transform.LookAt(target);
    }
}
