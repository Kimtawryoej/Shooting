using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
struct StatesSet
{
    public float MaxHp;
    public float MinHp;
    public float StDamage;
}
public class Unit : MonoBehaviour
{
    [SerializeField] private StatesSet states;
    [SerializeField] protected float speed;
    public LayerMask layer;

}
