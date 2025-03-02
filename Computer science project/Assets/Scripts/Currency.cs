using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : Dropable
{
    public enum CurrencyType { Small, Medium, Large, Test}

    public CurrencyType currencyType;
    public int xpValue;       // The XP value associated with this currency type
    public int currencyValue; // The actual currency value associated with this type

    void Start()
    {
        base.Start();
        RandomizeValues();
    }

    private void RandomizeValues()
    {
        switch (currencyType)
        {
            case CurrencyType.Small:
                xpValue = Random.Range(10, 21);  // Random XP between 10-20
                currencyValue = Random.Range(1, 4);  // Random currency between 5-10
                break;
            case CurrencyType.Medium:
                xpValue = Random.Range(30, 51);  // Random XP between 30-50
                currencyValue = Random.Range(4, 8);  // Random currency between 20-40
                break;
            case CurrencyType.Large:
                xpValue = Random.Range(70, 101);  // Random XP between 70-100
                currencyValue = Random.Range(8, 16);  // Random currency between 50-100
                break;
            case CurrencyType.Test:
                xpValue = 100000;
                currencyValue = 100000;
                break;
        }
    }

    protected override void OnPickup()
    {
        player.EarnXP(xpValue);
        player.EarnCurrency(currencyValue);
    }
}