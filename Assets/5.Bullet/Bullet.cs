using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingleTone<Bullet>
{
    //�÷��̾ ������ �ϸ� ������ �������� bullet���ݷ� up and �Ѿ˰���
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

    IEnumerator Time() //Ÿ�̸� ��ũ��Ʈ ���� ��������
    {
        yield return new WaitForSeconds(3);
        InObj();
    }

    public void InObj()
    {
        ObjectPool.Instance.InObject(layer, gameObject);
    }
}
