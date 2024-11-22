using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] public bool isAgro;

    public Enemy()
    {
        isAgro = false;
    }
}
