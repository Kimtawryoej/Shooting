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
    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void Update()
    {

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
        transform.Translate(dir * speed * Time.fixedDeltaTime);
    }

    IEnumerator Shoot()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            ObjectPool.Instance.OutObject(bullet, transform.position, Quaternion.identity);
 
        }
    }
}
