using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    public Slider slider;

    public int slideValue;//�����̴��� ����� ��

    private void Update()
    {
        slideValue = (int)(slider.value*10);
        //Debug.Log(slideValue);
    }
}
