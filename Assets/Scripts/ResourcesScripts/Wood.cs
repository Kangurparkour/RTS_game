using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wood : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Wood wood;
    private void Start()
    {
        wood = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        wood.amount += value;

        if (wood.amount > wood.maxAmount)
        {
            wood.amount = wood.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (wood.amount < value)
            return false;


        wood.amount -= value;
        return true;
    }

    public static bool HaveEnaughResources(uint value)
    {
        return value <= wood.amount;
    }

}
