using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public GameObject[] npcHUD;

    [Range(0.01f , 1.0f)]
    public float smoothFactor = 0.5f;

    //CameraRotate 20190328
    private bool RotateAroundPlayer = true;
    private float RotateSpeed = 5.0f;
    private Vector3 _cameraOffset;

    //Camera 滾輪控制距離 20190410
    public float distence; //當前攝影機與主角的距離
    public float disSpeed = 1;
    public float minDistence = 1;
    public float maxDistence = 5;
    private Vector3 cameraPosition;

    private void Awake()
    {
        target = GameObject.Find("Character").transform;
    }

    private void Start()
    {
        _cameraOffset = transform.position - target.position;
    }

    private void Update()
    {
        //NPC_HUD追隨攝影機
        if(npcHUD != null)
        {
            for (int i = 0; i < npcHUD.Length; i++)
            {
                if(npcHUD[i].gameObject != null)
                {
                    npcHUD[i].transform.LookAt(npcHUD[i].transform.position + transform.rotation * Vector3.back,
                                            transform.rotation * Vector3.up);
                }
                else if(npcHUD[i].gameObject == null)
                {
                    return;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (RotateAroundPlayer && Input.GetKey(KeyCode.R))
        {
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotateSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset ;

        }
        Vector3 newPos = target.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position + cameraPosition, newPos, smoothFactor);
        transform.LookAt(target) ;

        //Camera 滾輪控制距離 20190410
        distence -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;
        distence = Mathf.Clamp(distence, minDistence, maxDistence);
        cameraPosition = new Vector3(0, 0, -distence);
    }

    public void ResetHUDFollow()
    {
        npcHUD = GameObject.FindGameObjectsWithTag("UI_NPC");
    }
}
