using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider hpSlider;
    Unit unit;
    [SerializeField] Vector3 sliderOffset;


    private void Start()
    {
        hpSlider = GetComponent<Slider>();
        unit = GetComponentInParent<Unit>();
        var canvas = GameObject.FindGameObjectWithTag("HP Canvas");
        if (canvas != null)
        {
            transform.SetParent(canvas.transform);
        }
    }

    private void Update()
    {
        if (!unit)
        {
            Destroy(this.gameObject);
            return;
        }

        hpSlider.value = unit.Hp_Percent;
        transform.position = unit.transform.position + sliderOffset;
        transform.LookAt(Camera.main.transform);
    }
}
