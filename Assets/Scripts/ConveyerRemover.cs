using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerRemover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>())
        {
            Destroy(other.gameObject);
        }
    }


}
