using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct a
{
    public static int b;
}

public class Player : Unit
{
    [SerializeField] private GameObject bullet;
    private float x;
    private float z;
    private Vector3 dir = new Vector3(0, 0, 0);
    private GameObject SaveBullet;
    public static Player Instance;
    a A;
    override protected void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Shoot(() => Input.GetKeyDown(KeyCode.Space), bullet, unitStat.AttackPower, 0.5f));
    }

    private void FixedUpdate()
    {
        Move();
        
    }

    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        dir.x = x;
        dir.z = z;
        dir.Normalize();
        transform.Translate(dir * UnitStat.MoveSpeed * Time.fixedDeltaTime);
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (bullet.Layer == LayerMask.GetMask("MonsterBullet"))
            {
                ChangeHp(-bullet.Power);
                bullet.InObj();
            }
        }
    }
}
