using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour {
    //Minimum value of the stat bar to display
    public float MinValue = 0.0f;
    //Maximum value of the stat bar to display
    public float MaxValue = 100.0f;

    public float CurrentValue { get; private set; }

    public Text valueText;
    public Image valueImage;

    private float startWidth;

	// Use this for initialization
	void Awake ()
    {
        CurrentValue = MaxValue;
        startWidth = valueImage.rectTransform.rect.width;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(valueText != null)
            valueText.text = CurrentValue + "/" + MaxValue;
        if (valueImage != null)
            valueImage.rectTransform.sizeDelta = new Vector2(Map(MinValue, startWidth, CurrentValue / MaxValue), valueImage.rectTransform.rect.height);

    }

    public void SetValue(float amount)
    {
        CurrentValue = amount;
    }

    float Map(float min, float max, float t)
    {
        return min + t * (max - min);
    }
}
