using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public float delayTime = 1f;
    public float repeatRate = 5f;
    public GameObject spawnPointParent;
    public SphereCollider[] spawnPoint;

    private bool playerIsDead = false;
    private void playerDeathAction()
    {
        playerIsDead = true;
    }

    private void OnValidate()
    {
        spawnPoint = spawnPointParent.GetComponentsInChildren<SphereCollider>();
    }

    private void Spawn()
    {
        if(playerIsDead)
        {
            CancelInvoke("Spawn");
            return;
        }

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            Instantiate(enemy, spawnPoint[i].transform.position, spawnPoint[i].transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print(enemy.GetComponent<Monster>().m_name + "出現了！");
            Spawn();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CancelInvoke("Spawn");
        }
    }
}
