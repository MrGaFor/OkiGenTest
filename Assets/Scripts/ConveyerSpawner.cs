using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerSpawner : MonoBehaviour
{
    [Header("Settings")]
    [Space(10f)]
    [SerializeField] private List<Variants> Objects;
    [Range(0.2f, 3f)]
    [SerializeField] private float SpawnTimer = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float TimerMaxDeviation = 0.5f;

    bool start = false;
    public void SetStart(bool value)
    {
        start = value;
        FindObjectOfType<ConveyerArrow>().SetStart(value);
    }

    float _timer = 0f;
    void FixedUpdate()
    {
        if (start)
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _timer = SpawnTimer + Random.Range(-TimerMaxDeviation, TimerMaxDeviation);
                int prefabNo = Random.Range(0, Objects.Count);
                Instantiate<Transform>(Objects[prefabNo].PodObjects[Random.Range(0, Objects[prefabNo].PodObjects.Count)], transform.position, Quaternion.identity, null);
            }
        }
    }
}

[System.Serializable]
public class Variants
{
    public List<Transform> PodObjects;
}

