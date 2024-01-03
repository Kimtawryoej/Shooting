using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monster : Unit, I_ObseverManager
{
    [SerializeField] private GameObject bullet;
    public static Monster Instance;
    private List<I_Obsever> Obsevers = new List<I_Obsever>();
    [SerializeField] private LayerMask MonsterType;
    private int typeLayer;
    override protected void OnEnable()
    {
        ReSetStat();
    }

    override protected void Awake()
    {
        Debug.Log(MonsterType.GetType());
        typeLayer = (1 << 10) | (1 << 11);
        Debug.Log(typeLayer);
        Instance = this;
        timeManager = new TimeAgent(1, (TimeAgent) => { }, (TimeAgent) => Shoot(bullet, unitStat.AttackPower, 0.5f, gameObject.transform.rotation));
    }

    private void FixedUpdate()
    {
        transform.Translate(-Vector3.forward * unitStat.MoveSpeed);
        TimerSystem.Instance.AddTimer(timeManager);
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (bullet.Layer == LayerMask.GetMask("PlayerBullet"))
            {
                ChangeHp(-bullet.Power);
                bullet.InObj();
            }
        }
    }



    #region ObseverManager
    public void Add(I_Obsever obsever)
    {
        Obsevers.Add(obsever);
    }
    public void Delete(I_Obsever obsever)
    {
        Obsevers.Clear();
    }
    public void NotifyObserver<T>(List<I_Obsever> obsevers, T value)
    {
        foreach (I_Obsever Obsevers in Obsevers)
        {
            Obsevers.Refresh<T>(value);
        }
    }
    #endregion
}
class TypesAction : MonoBehaviour
{
    private LayerMask MonsterType;
    private UnitStatInfo unitStat;
    private Quaternion rotation;
    private Action StopMonster;
    private Dictionary<LayerMask, Action> Actions;

    TypesAction(LayerMask MonsterType, UnitStatInfo unitStat, Quaternion rotation)
    {
        this.MonsterType = MonsterType;
        this.unitStat = unitStat;
        this.rotation = rotation;
    }

    //public void ActionSet()
    //{
    //    Actions = new Dictionary<LayerMask, Action>()
    //    {
    //        {(1<<10),()=>}
    //    };
    //}

}

