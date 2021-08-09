using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocks : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Rocks rocks;
    private void Start()
    {
        rocks = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        rocks.amount += value;

        if (rocks.amount > rocks.maxAmount)
        {
            rocks.amount = rocks.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (rocks.amount < value)
            return false;


        rocks.amount -= value;
        return true;
    }

    public static bool HaveEnaughResources(uint value)
    {
        return value <= rocks.amount;
    }

}
