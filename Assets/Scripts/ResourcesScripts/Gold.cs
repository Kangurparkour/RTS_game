using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Gold gold;
    private void Start()
    {
        gold = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        gold.amount += value;

        if (gold.amount > gold.maxAmount)
        {
            gold.amount = gold.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (gold.amount < value)
            return false;


        gold.amount -= value;
        return true;
    }

    public static bool HaveEnaughResources(uint value)
    {
        return value <= gold.amount;
    }

}
