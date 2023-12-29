using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Unit
{
    [SerializeField] private GameObject bullet;
    private float x;
    private float z;
    private Vector3 dir = new Vector3(0, 0, 0);
    public static Player Instance;

    override protected void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeManager = new TimeAgent(0.5f, (TimeAgent) => Debug.Log("ÃÑ¾Ë»ý¼º"), (TimeAgent) => Shoot(bullet, unitStat.AttackPower, 0.5f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimerSystem.Instance.AddTimer(timeManager);
        }
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
