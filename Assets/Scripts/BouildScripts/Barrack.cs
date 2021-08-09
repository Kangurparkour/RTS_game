using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Unit, ISelectable
{
    public Transform spawnPoint;
    public Transform stopPoint;



    protected override void Start()
    {
        base.Start();

    }

    public void SetSelected(bool isSelected)
    {
        healthBar.gameObject.SetActive(isSelected);
        
    }

    private void GetCommand(GameObject prefab)
    {
        var buyable = prefab.GetComponent<Buyalbe>();
        if (!buyable || !Gold.TrySpendResources(buyable.gold) || !Iron.TrySpendResources(buyable.iron)) return;

        var unit = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        unit.SendMessage("GetCommand", stopPoint.position,SendMessageOptions.DontRequireReceiver);
        Debug.Log(stopPoint.position);
    }
   
}
