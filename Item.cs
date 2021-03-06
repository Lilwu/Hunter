﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }
    public string ItemName;
    public Sprite Icon;
    public int Price;
    public bool UseByRightClick;

    //20190224 
    [Range(1,999)]
    public int MaximumStacks = 1;


    //20190222

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }
}

