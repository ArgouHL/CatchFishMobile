using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Cat",menuName = "CreatObject/New Cat")]
public class CatClass:ScriptableObject
{
    [SerializeField]
    private catType catType;
    public catType CatType { get { return catType; } }

    [SerializeField]
    private string catName;
    public string Name { get { return catName; } }



}

public enum catType {Fat,Slim,Small}

