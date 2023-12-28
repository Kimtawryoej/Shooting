using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingleTone<Bullet>
{
    //플레이어가 레벨업 하면 옵저버 패턴으로 bullet공격력 up and 총알개수
    [SerializeField] private LayerMask layer;
    public int Power { get; set; }

    private void OnEnable()
    {
        StartCoroutine(Time());
    }

    private void Start()
    {
        Power = Player.Instance.UnitStat.AttackPower;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * 0.5f);
    }

    IEnumerator Time() //타이머 스크립트 새로 만들어야함
    {
        yield return new WaitForSeconds(3);
        ObjectPool.Instance.InObject(layer, gameObject);
    }
}
