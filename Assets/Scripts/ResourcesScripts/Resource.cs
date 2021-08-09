using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Resource resouce;
    private void Start()
    {
        resouce = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        resouce.amount += value;

        if (resouce.amount > resouce.maxAmount)
        {
            resouce.amount = resouce.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (resouce.amount < value)
            return false;


        resouce.amount -= value;
        return true;
    }

}
