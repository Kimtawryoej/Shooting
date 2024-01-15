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
    protected TimeAgent timeManager;

    override public void Awake()
    {
        timeManager = new TimeAgent(2, (TimeAgent) => { }, (TimeAgent) => /*ObjectPool.Instance.InObject(layer, gameObject)*/Destroy(gameObject));
    }
    private void OnEnable()
    {
        TimerSystem.Instance.AddTimer(timeManager);
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Speed);
    }



    public void InObj()
    {
        
    }
}
