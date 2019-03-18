using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateFont : MonoBehaviour
{
    GameObject damageCanvas;
    Text damageText;
    int criticalHit;

    private void Awake()
    {
        damageCanvas = Resources.Load("DamageCanvas", typeof(GameObject)) as GameObject;
    }

    public void AttackPointFont(int attack)
    {
        criticalHit = Random.Range(1, 10);
        damageText = damageCanvas.transform.Find("DamageText").gameObject.GetComponent<Text>();
        if (criticalHit == 1)
        {
            damageText.color = Color.red;
            damageCanvas.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            damageText.color = Color.white;
            damageCanvas.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
       
        Instantiate(damageCanvas, transform.Find("HeadPoint")).transform.localPosition = transform.position;
    }

}