using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameObject prefab;
    Button button;
    private void Awake()
    {
        button = GetComponentInChildren<Button>();

        Buyalbe buyable;
        if (prefab && (buyable = prefab.GetComponent<Buyalbe>()))
            button.image.sprite = buyable.icon;

    }

    private void Update()
    {
        Buyalbe buyable;
        if (prefab && (buyable = prefab.GetComponent<Buyalbe>()))
        {
            button.interactable = Gold.HaveEnaughResources(buyable.gold) || Iron.HaveEnaughResources(buyable.iron);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public virtual void SpawnUnit()
    {
        CameraController.SpawnUnit(prefab);
    }
}
