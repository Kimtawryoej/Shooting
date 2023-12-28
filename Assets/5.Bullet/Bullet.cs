using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingleTone<Bullet>
{
    //�÷��̾ ������ �ϸ� ������ �������� bullet���ݷ� up and �Ѿ˰���
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

    IEnumerator Time() //Ÿ�̸� ��ũ��Ʈ ���� ��������
    {
        yield return new WaitForSeconds(3);
        ObjectPool.Instance.InObject(layer, gameObject);
    }
}
