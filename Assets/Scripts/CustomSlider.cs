using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{

    public float value = 1f;
    public float valueMax = 1f;

    [SerializeField] private RectTransform ValueObj;

    private void OnDrawGizmos()
    {
        UpdateValue();
    }

    private float MaxWidht;
    private void UpdateValue()
    {
        value = Mathf.Clamp(value, 0f, valueMax);
        MaxWidht = GetComponent<RectTransform>().sizeDelta.x;
        if (ValueObj)
        {
            ValueObj.sizeDelta = new Vector2(Mathf.Clamp((value / valueMax) * MaxWidht, 0f, MaxWidht), ValueObj.sizeDelta.y);
        }
    }

    public void SetValue(float val)
    {
        value = val;
        UpdateValue();
    }
}
