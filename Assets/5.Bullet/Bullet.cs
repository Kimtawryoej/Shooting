using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingleTone<Bullet>
{
    //플레이어가 레벨업 하면 옵저버 패턴으로 bullet공격력 up and 총알개수
    [SerializeField] private LayerMask layer;
    public LayerMask Layer { get => layer; }
    public int Power { get; set; }
    public float Speed { get; set; }
    private void OnEnable()
    {
        StartCoroutine(Time());
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Speed);
    }

    IEnumerator Time() //타이머 스크립트 새로 만들어야함
    {
        yield return new WaitForSeconds(3);
        InObj();
    }

    public void InObj()
    {
        ObjectPool.Instance.InObject(layer, gameObject);
    }
}
