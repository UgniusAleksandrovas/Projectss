using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    [SerializeField] Slider sliderUI;
    [SerializeField] Vector2 range;
    [SerializeField] Text textSliderValue;

    void Start()
    {
        DisplaySliderValue();
    }

    public void DisplaySliderValue()
    {
        float percent = (sliderUI.value - sliderUI.minValue) / (sliderUI.maxValue - sliderUI.minValue);
        float displayAmount = percent * (range.y - range.x);
        textSliderValue.text = displayAmount.ToString("f2");
    }
}
