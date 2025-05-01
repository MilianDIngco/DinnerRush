using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StressLevel : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI gameOver;

    void Start(){
        slider.value = 0;
    }
    public void SetSlider(float value){
        slider.value += value;
        checkMax();
    }

    void checkMax(){
        if (slider.value >= 100){
            gameOver.enabled = true;
        }
    }
}
