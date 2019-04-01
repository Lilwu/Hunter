using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    public List<Transform> icons;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].transform.LookAt(icons[i].transform.position + transform.rotation * Vector3.back,
                                        transform.rotation * Vector3.up);
        }


    }
}
