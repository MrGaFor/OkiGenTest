using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    [Header("Settings")]
    [Space(10f)]
    [SerializeField] private float Speed = 1.5f;
    private Rigidbody RBody;

    void Start()
    {
        RBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 _pos = RBody.position;
        RBody.position += -transform.right * Speed * Time.deltaTime;
        RBody.MovePosition(_pos);
    }
}
