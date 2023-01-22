using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallHovering : MonoBehaviour
{

    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform ShowPos;
    [SerializeField] private Transform HidePos;

    bool once = true;
    public void SetStart(bool value)
    {
        if (once)
        {
            once = false;
            StartCoroutine(Anim(ShowPos));
        }
    }

    IEnumerator Anim(Transform final)
    {
        while (Vector3.Distance(transform.position, final.position) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, final.position, Time.deltaTime * speed);
            yield return new WaitForFixedUpdate();
        }
    }

}
