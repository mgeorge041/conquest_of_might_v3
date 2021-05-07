using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLabel : MonoBehaviour
{
    public Text updateLabel;
    public Slider slider;

    public void Update()
    {
        updateLabel.text = slider.value.ToString();
    }
}
