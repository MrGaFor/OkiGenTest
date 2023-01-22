using UnityEngine;
using TMPro;
using System.Collections;

public class TextSpawner : MonoBehaviour
{
    [Header("Settings")]
    [Space(10f)]
    [SerializeField] private Transform Prefab;
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float RemoveTimer = 0.4f;

    public void SpawnText(int count)
    {
        Transform lastObj = Instantiate<Transform>(Prefab, transform.position, Quaternion.Euler(0, 180, 0), transform);
        lastObj.GetComponent<TextMeshPro>().text = "+" + count.ToString();
        StartCoroutine(TextMoving(lastObj));
    }

    IEnumerator TextMoving(Transform obj)
    {
        Vector3 finalPos = obj.position + new Vector3(Random.Range(-0.2f, 0.2f), 1f, 0f);
        float sValue = Random.Range(0.9f, 1.1f);
        Vector3 finalScale = new Vector3(sValue, sValue, sValue);
        bool testDestroi = true;
        while (testDestroi)
        {
            float distance = Vector3.Distance(obj.position, finalPos);
            obj.position = Vector3.Lerp(obj.position, finalPos, Time.deltaTime * Speed * distance);
            obj.localScale = Vector3.Lerp(obj.localScale, finalScale, Time.deltaTime * Speed * distance);
            yield return new WaitForEndOfFrame();
            if (distance < 0.1f)
                testDestroi = false;
        }
        StartCoroutine(TextRemoving(obj));
    }

    IEnumerator TextRemoving(Transform obj)
    {
        TextMeshPro objText = obj.GetComponent<TextMeshPro>();
        float timer = RemoveTimer;
        while (timer > 0f)
        {
            objText.alpha = Mathf.Lerp(0f, objText.color.a, timer / RemoveTimer);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(obj.gameObject);

    }



}
