using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLOWMO : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Speed = 1f;
    private float prevSpeed = 1f;
    void Update()
    {
        if (Speed != prevSpeed)
        {
            prevSpeed = Speed;
            Time.timeScale = Speed;
        }
    }
}
