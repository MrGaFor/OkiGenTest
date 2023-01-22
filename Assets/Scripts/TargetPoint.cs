using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public enum TargetState { Base, ToTarget, ToBasket};

    [Header("Settings")]
    [SerializeField] private Transform HomePos;
    [SerializeField] private Transform BasketPos;
    private Moving MeMove;
    private Basket MeBasket;
    private TextSpawner MeTextSpawner;

    [Space(10f)]
    [Header("Debug")]
    [SerializeField] private bool DEBUG = false;
    [SerializeField] private TargetState MeState = TargetState.Base;

    private void Start()
    {
        MeMove = GetComponent<Moving>();
        MeBasket = FindObjectOfType<Basket>();
        MeTextSpawner = FindObjectOfType<TextSpawner>();
    }

    float waitTimer = 0f;
    Transform Food;
    public void StartMove(Transform toObj, float toTime, bool click = false)
    {
        if (!click || (MeState == TargetState.Base && click))
        {
            waitTimer = toTime;
            if (MeState == TargetState.Base)
            {
                Food = toObj;
                MeState = TargetState.ToTarget;
            }
            MeMove.Move(transform, toTime, toObj.position, transform.rotation.eulerAngles, new Vector3(0.0689349f, 0.0689349f, 0.0689349f));
            StartCoroutine(TimerWait());
        }
    }

    IEnumerator TimerWait()
    {
        while (waitTimer > 0f)
        {
            yield return new WaitForEndOfFrame();
            waitTimer -= Time.deltaTime;
        }
        if (MeState == TargetState.ToTarget)
        {
            MeState = TargetState.ToBasket;
            Food.GetComponent<Rigidbody>().isKinematic = true;
            Food.GetComponent<Rigidbody>().useGravity = false;
            MeMove.Move(Food, 0.3f, BasketPos.position, BasketPos.rotation.eulerAngles, Food.localScale / 1.5f);
            StartMove(BasketPos, 0.3f);
        }
        else if (MeState == TargetState.ToBasket)
        {
            Food.SetParent(MeBasket.GetLastPos());
            Food.localPosition = Vector3.zero;
            MeBasket.AddItem(Food.GetComponent<Item>());
            StartMove(HomePos, 0.4f);
            MeState = TargetState.Base;
            MeTextSpawner.SpawnText(1);
        }
    }

    private void OnDrawGizmos()
    {
        if (DEBUG)
        {
            Debug.DrawLine(transform.position, HomePos.position, Color.red, 0.01f);
            if (!MeBasket)
                MeBasket = FindObjectOfType<Basket>();
            Debug.DrawLine(transform.position, MeBasket.transform.position, Color.blue, 0.01f);
        }
    }

}
