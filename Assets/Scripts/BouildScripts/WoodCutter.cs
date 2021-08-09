using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCutter : Unit, ISelectable
{
    [Header("WoodCutter")]
    uint procesing_Capacity;
    float productionTime ;

    bool isActive = true;

    public GameObject panel;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Production(isActive);
    }

    private void Production(bool buildIsActive)
    {
       
    }



    public void SetSelected(bool isSelected)
    {
        healthBar.gameObject.SetActive(isSelected);

    }

}
