using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    [SerializeField] private LayerMask ClickMask;
    private TargetPoint TargetP;

    private void Start()
    {
        TargetP = FindObjectOfType<TargetPoint>();
    }

    bool start = false;
    public void SetStart(bool value)
    {
        start = value;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (start)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, ClickMask))
                {
                    TargetP.StartMove(hit.transform, 0.3f, true);
                }
            }
        }
    }
}