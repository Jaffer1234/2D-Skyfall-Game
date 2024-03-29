﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance;

    private IngameUI ingameUiScript;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        if (IngameUI.Instance)
        {
            ingameUiScript = IngameUI.Instance;
            ingameUiScript.UpdateItemValuesCount();
        }
        else
            Utility.ErrorLog("ingame Ui Script could not be found in ItemController.cs of " + this.gameObject, 1);
    }

    public void UseLineBomb()
    {
        int quantity = EncryptedPlayerPrefs.GetInt("LineBomb");
        quantity--;
        EncryptedPlayerPrefs.SetInt("LineBomb", quantity);
        ingameUiScript.UpdateItemValuesCount();
    }

    public void UseRadiusBomb()
    {
        int quantity = EncryptedPlayerPrefs.GetInt("RadiusBomb");
        quantity--;
        EncryptedPlayerPrefs.SetInt("RadiusBomb", quantity);
        ingameUiScript.UpdateItemValuesCount();
    }

    public void UseTimeBomb()
    {
        int quantity = EncryptedPlayerPrefs.GetInt("TimeBomb");
        quantity--;
        if (quantity < 0)
            quantity = 0;
        EncryptedPlayerPrefs.SetInt("TimeBomb", quantity);
        ingameUiScript.UpdateItemValuesCount();
    }
}