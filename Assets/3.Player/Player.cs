using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] private GameObject bullet;
    private float x;
    private float z;
    private Vector3 dir = new Vector3(0, 0, 0);
    private GameObject SaveBullet;
    public static Player Instance;

    override protected void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Shoot());
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

    private IEnumerator Shoot()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            Debug.Log("´­¸§");
            ObjectPool.Instance.OutObject(layer, bullet, transform.position, Quaternion.identity);
        }
    }
}
