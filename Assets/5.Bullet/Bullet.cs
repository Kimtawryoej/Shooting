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
