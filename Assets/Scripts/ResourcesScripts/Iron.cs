using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iron : MonoBehaviour
{
    public uint amount;
    public uint maxAmount = 1000000;

    Text text;
    static Iron iron;
    private void Start()
    {
        iron = this;
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = amount.ToString();
    }

    public static bool TryAddResources(uint value)
    {
        iron.amount += value;

        if (iron.amount > iron.maxAmount)
        {
            iron.amount = iron.maxAmount;
            return false;
        }
        return true;
    }

    public static bool TrySpendResources(uint value)
    {
        if (iron.amount < value)
            return false;


        iron.amount -= value;
        return true;
    }
    public static bool HaveEnaughResources(uint value)
    {
        return value <= iron.amount;
    }


}
