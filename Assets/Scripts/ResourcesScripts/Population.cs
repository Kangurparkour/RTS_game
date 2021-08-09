using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Population : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Population population;
    private void Start()
    {
        population = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        population.amount += value;

        if (population.amount > population.maxAmount)
        {
            population.amount = population.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (population.amount < value)
            return false;


        population.amount -= value;
        return true;
    }

    public static bool HaveEnaughResources(uint value)
    {
        return value <= population.amount;
    }
}
