using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField] private GameObject monsterAppearPos;
    public GameObject MonsterAppearPos { get => monsterAppearPos; }

    public static Monster Instance;



    override protected void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        transform.rotation = GameManager.Instance.RotationCheck(layer);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * 0.5f);
    }

}
