using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class Monster : Unit, I_ObseverManager
{
    [SerializeField] private GameObject bullet;
    public GameObject Bullet { get => bullet; }
    public static Monster Instance;
    private List<I_Obsever> Obsevers = new List<I_Obsever>();
    #region Layer변수
    [SerializeField] private LayerMask MonsterType;
    private BitArray MonsterTypeBit;
    #endregion
    private TypesAction MonsterAction;
    private TimeAgent timeNull;

    //왜 함수 밖에서는 매개변수를 불러올수 없음?
    override protected void Awake()
    {
        #region ActionsSet
        MonsterTypeBit = new BitArray(new int[] { MonsterType.value.ConvertTo<int>() });
        MonsterAction = new TypesAction(MonsterTypeBit, unitStat, transform.rotation, this);
        MonsterAction.Set();
        #endregion
        Instance = this;

    }
    override protected void OnEnable()
    {
        ReSetStat();

    }

    private void Start()
    {
        MonsterAction.GetValue();
    }

    private void FixedUpdate()
    {


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
    private Monster myObject;

    private Dictionary<int, Action> Actions;

    private Vector3[] SaveEndPos = { new Vector3(-11, 0, 3.115f), new Vector3(1, 0, 3.115f), new Vector3(11, 0, 3.115f) };
    private static HashSet<Vector3> EndPos = new HashSet<Vector3>();
    #region 타이머
    private TimeAgent MoveTimeManager;
    private TimeAgent WaitTimeManager;
    private TimeAgent BulletTimeManagerCount;
    private TimeAgent BulletTimeManager;
    #endregion
    public TypesAction(BitArray MonsterType, UnitStatInfo unitStat, Quaternion rotation, Monster myObject)
    {
        this.MonsterType = MonsterType;
        this.unitStat = unitStat;
        this.rotation = rotation;
        this.myObject = myObject;
    }

    public void Set()
    {
        MoveTimeManager = new TimeAgent(10, Time.deltaTime, (TimeAgent) => { myObject.transform.Translate(-Vector3.forward * unitStat.MoveSpeed); }, (TimeAgent) => { });

        WaitTimeManager = new TimeAgent(1, Time.deltaTime, (TimeAgent) => { myObject.transform.position = Vector3.Lerp(GameManager.Instance.MonsterAppearPos.transform.position, EndPos.Last(), WaitTimeManager.CurrentTime); }, (TimeAgent) => { if (SaveEndPos.Last().Equals(EndPos.Last())) { EndPos.Clear(); } });

        BulletTimeManagerCount = new TimeAgent(10, 1, (TimeAgent) => { TimerSystem.Instance.AddTimer(BulletTimeManager); }, (TimeAgent) => { });
        BulletTimeManager = new TimeAgent(Time.deltaTime, Time.deltaTime, (TimeAgent) => { myObject.Shoot(myObject.Bullet, unitStat.AttackPower, 0.5f, myObject.transform.rotation); }, (TimeAgent) => { });

        Actions = new Dictionary<int, Action>()
        {
            {10,()=>{StopMove(); } },
            {11,()=>{ Move(); } },
            {12,()=>{PlayerAngleShoot(); } },
            {13,()=>{Debug.Log("13");} }
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

    #region ActionMethods
    public void Move()
    {
        TimerSystem.Instance.AddTimer(MoveTimeManager);
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
        TimerSystem.Instance.AddTimer(WaitTimeManager);
    }

    public void PlayerAngleShoot()
    {
        TimerSystem.Instance.AddTimer(BulletTimeManagerCount);
    }
    #endregion
}

