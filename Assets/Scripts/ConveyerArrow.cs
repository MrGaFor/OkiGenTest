using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerArrow : MonoBehaviour
{
    private SpriteRenderer Me;
    [SerializeField] private float Speed = 5f;
    void Start()
    {
        Me = GetComponent<SpriteRenderer>();
        Me.size = new Vector2(100f, 0.5f);
    }

    bool start = false;
    public void SetStart(bool value)
    {
        start = value;
    }

    void LateUpdate()
    {
        if (start)
        {
            Me.size = Vector2.Lerp(Me.size, new Vector2(Me.size.x - 1f, Me.size.y), Time.deltaTime * Speed);
            if (Me.size.x < 20f)
                Me.size = new Vector2(100f, 0.5f);
        }
    }
}
