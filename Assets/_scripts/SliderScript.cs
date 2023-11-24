using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    public Slider slider;

    public int slideValue;//슬라이더가 당겨진 값

    private void Update()
    {
        slideValue = (int)(slider.value*10);
        //Debug.Log(slideValue);
    }
}
