using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelValue : MonoBehaviour
{
    // Start is called before the first frame update
    Slider slider;

    public void setLevel() {
        slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        GameManager.level = (int) slider.value;
    }
}
