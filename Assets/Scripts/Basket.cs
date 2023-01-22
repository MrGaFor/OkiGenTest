using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Basket : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private List<Transform> Positions;
    [SerializeField] private List<Item> Items;
    [SerializeField] private TMPro.TextMeshPro TextCount;

    private Manager Manager_;
    private List<NeedItems> Needs_;

    private void Start()
    {
        Manager_ = FindObjectOfType<Manager>();
        TextCount.text = Items.Count + "/" + Positions.Count;
    }

    public Transform GetLastPos()
    {
        return Positions[Items.Count];
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
        bool res = TestNeed();
        if (res)
        {
            Manager_.Win();
        }
        if (Items.Count >= Positions.Count && !res)
            Manager_.FullBasket();

        StartCoroutine(AddItemAnim());
    }

    IEnumerator AddItemAnim()
    {
        Transform obj = TextCount.transform;
        float timer = 0.1f;
        bool once = true;
        while (timer > -0.1f)
        {
            timer -= Time.deltaTime;
            if (timer > 0f)
            {
                obj.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer * 10f);
            }
            else
            {
                if (once)
                {
                    once = false;
                    TextCount.text = Items.Count + "/" + Positions.Count;
                }
                obj.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer * -10f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public List<Item.Type> haveItems;
    private bool TestNeed()
    {
        haveItems = new List<Item.Type>();
        Needs_ = Manager_.GetNeedItems();
        bool res = true;
        for (int i = 0; i < Items.Count; i++)
        {
            haveItems.Add(Items[i].MeType);
        }
        for (int i = 0; i < Needs_.Count; i++)
        {
            if (!(Needs_[i].Count <= GetCount(haveItems, Needs_[i].Type)))
            {
                res = false;
                break;
            }
        }
        return res;
    }

    private int GetCount(List<Item.Type> list, Item.Type needType)
    {
        return list.Count(n => n == needType);
    }

    public void EndGame()
    {
        TextCount.gameObject.SetActive(false);
    }

}

[System.Serializable]
public class NeedItems
{
    public Item.Type Type = Item.Type.Apple;
    public int Count = 1;

    public NeedItems(Item.Type type, int count)
    {
        Type = type;
        Count = count;
    }
}