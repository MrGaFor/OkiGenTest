using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 ToPos;
    [SerializeField] private Vector3 ToRot;
    [SerializeField] private Vector3 ToScale = Vector3.one;
    [SerializeField] private float ToTime;
    [SerializeField] private AnimationCurve SpeedCurve;
    private Transform Obj;
    [Space(5f)]
    [Header("Debug")]
    [SerializeField] private bool DEBUG = true;
    [SerializeField] private Color DebugColor = Color.white;

    /*private void Start()
    {
        Move();
    }*/

    // Custom Moving
    public void Move(Transform obj, float timer, Vector3 pos, Vector3 rot, Vector3 scale )
    {
        StartCoroutine(TimerCoroutine(obj, timer, obj.position, pos, obj.rotation.eulerAngles, rot, obj.localScale, scale));
    }
    // Set Moving
    public void Move()
    {
        Obj = transform;
        StartCoroutine(TimerCoroutine(Obj, ToTime, transform.position, ToPos, transform.rotation.eulerAngles, ToRot, transform.localScale, ToScale));
    }

    IEnumerator TimerCoroutine(Transform obj, float timer, Vector3 frompos, Vector3 pos, Vector3 fromrot, Vector3 rot, Vector3 fromscale, Vector3 scale)
    {
        float _allTime = timer;
        while (timer > 0f)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            float _value = Mathf.Abs((timer / _allTime) - 1f);
            _value *= SpeedCurve.Evaluate(_value);
            obj.position = Vector3.Lerp(frompos, pos, _value);
            obj.rotation = Quaternion.Lerp(Quaternion.Euler(fromrot), Quaternion.Euler(rot), _value);
            obj.localScale = Vector3.Lerp(fromscale, scale, _value);
        }
    }

    private void OnDrawGizmos()
    {
        if (DEBUG)
        {
            Debug.DrawLine(transform.position, ToPos, DebugColor, 0.01f);
            Debug.DrawRay(ToPos, Quaternion.Euler(ToRot) * Vector3.forward, DebugColor, 0.01f);
        }
    }

}
