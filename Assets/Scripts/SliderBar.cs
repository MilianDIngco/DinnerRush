using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    public Slider slider;
    public void SetSlider(float value){
        slider.value = value;
    }
}
