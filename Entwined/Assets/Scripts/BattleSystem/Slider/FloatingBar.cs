using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(long value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetValue(long value)
    {
        slider.value = value;
    }
}
