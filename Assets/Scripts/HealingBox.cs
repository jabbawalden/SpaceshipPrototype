using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBox : MonoBehaviour
{
    bool haveHealed;

    private void Update()
    {
        transform.Rotate(1, 0, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && !haveHealed && other.GetComponentInParent<PlayerController>())
        {
            if (other.GetComponentInParent<PlayerController>().currentHealth < other.GetComponentInParent<PlayerController>().maxHealth)
            {
                haveHealed = true;
                other.GetComponentInParent<PlayerController>().HealPlayer();
                Destroy(gameObject);
            }
        }
    }
}
