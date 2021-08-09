using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Food food;
    private void Start()
    {
        food = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        food.amount += value;

        if (food.amount > food.maxAmount)
        {
            food.amount = food.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (food.amount < value)
            return false;


        food.amount -= value;
        return true;
    }

    public static bool HaveEnaughResources(uint value)
    {
        return value <= food.amount;
    }

}
