using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class Monster : Unit, I_ObseverManager
{
    [SerializeField] private GameObject bullet;
    public static Monster Instance;
    private List<I_Obsever> Obsevers = new List<I_Obsever>();
    #region Layer변수
    [SerializeField] private LayerMask MonsterType;
    private BitArray MonsterTypeBit;
    #endregion
    public TypesAction MonsterAction;
    private TimeAgent timeNull;

    //왜 함수 밖에서는 매개변수를 불러올수 없음?
    override protected void Awake()
    {
        MonsterTypeBit = new BitArray(new int[] { MonsterType.value.ConvertTo<int>() });
        MonsterAction = new TypesAction(MonsterTypeBit, unitStat, transform.rotation,gameObject);
        MonsterAction.ActionSet();
        Instance = this;
        timeManager = new TimeAgent(1, (TimeAgent) => { }, (TimeAgent) => Shoot(bullet, unitStat.AttackPower, 0.5f, gameObject.transform.rotation));
    }
    override protected void OnEnable()
    {
        ReSetStat();
        MonsterAction.GetValue();
    }

    
    private void FixedUpdate()
    {
        
        //transform.Translate(-Vector3.forward * unitStat.MoveSpeed);
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
public class TypesAction : MonoBehaviour
{
    private BitArray MonsterType;
    private UnitStatInfo unitStat;
    private Quaternion rotation;
    private GameObject myObject;
    private Dictionary<int, Action> Actions;
    private Vector3[] SaveEndPos = { new Vector3(-11, 0, 3.115f), new Vector3(1, 0, 3.115f), new Vector3(11, 0, 3.115f) };
    private static HashSet<Vector3> EndPos = new HashSet<Vector3>();

    public TypesAction(BitArray MonsterType, UnitStatInfo unitStat, Quaternion rotation,GameObject myObject)
    {
        this.MonsterType = MonsterType;
        this.unitStat = unitStat;
        this.rotation = rotation;
        this.myObject = myObject;
    }

    public void ActionSet()
    {
        Actions = new Dictionary<int, Action>()
        {
            {10,()=>{StopMove(); } },
            {11,()=>{Debug.Log("11");} },
            {12,()=>{Debug.Log("12");} },
        };
    }

    public void GetValue()
    {
        foreach (var item in Actions)
        {
            if (MonsterType[item.Key].Equals(true))
            {
                item.Value();
            }
        }
    }

    public void StopMove()
    {
        foreach (Vector3 item in SaveEndPos)
        {
            if (!EndPos.Contains(item))
            {
                EndPos.Add(item);
                break;
            }
        }
        for (float i = 0; i <= 1; i += Time.deltaTime*0.05f)
        {
            float j = 0;
            while(j<Time.deltaTime*2)
            {
                j += Time.deltaTime;
            }
            myObject.transform.position = Vector3.Lerp(GameManager.Instance.MonsterAppearPos.transform.position, EndPos.Last(), i);
            Debug.Log("이동");
        }
        if(SaveEndPos.Last().Equals(EndPos.Last()))
        {
            EndPos.Clear();
        }
    }
}

