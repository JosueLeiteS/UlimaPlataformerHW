using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoweBar : MonoBehaviour
{
    private Image bar;

    private void Awake() {
        bar = transform.Find("Bar").GetComponent<Image>();

        bar.fillAmount = 0f;
    }
}
