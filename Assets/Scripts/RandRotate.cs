using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandRotate : MonoBehaviour
{

    [SerializeField] private bool X = false;
    [SerializeField] private bool Y = true;
    [SerializeField] private bool Z = false;


    private void OnEnable()
    {
        if (X)
            transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
        if (Y)
            transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        if (Z)
            transform.Rotate(Vector3.right, Random.Range(0f, 360f));

    }

}
