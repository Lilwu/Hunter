using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    public GameObject[] icons;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);


        if(icons != null)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if(icons[i].gameObject != null)
                {
                    icons[i].transform.LookAt(icons[i].transform.position + transform.rotation * Vector3.back,
                                            transform.rotation * Vector3.up);
                }
                else if(icons[i].gameObject == null)
                {
                    return;
                }
            }
        }
    }

    public void ResetIconFollow()
    {
        icons = GameObject.FindGameObjectsWithTag("MinimapIcon");
    }

}
