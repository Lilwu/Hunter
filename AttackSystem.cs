using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public GameObject monsterModel;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Monster")
        {
            monsterModel = other.gameObject;
        }
        else if (other.tag == "Dragon")
        {
            monsterModel = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            monsterModel = null;
        }
        else if (other.tag == "Dragon")
        {
            monsterModel = other.gameObject;
        }
    }
}
