using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Net;


public class Player : Unit
{
    #region key
    public Dictionary<KeyCode, Action> InputKey;
    #endregion
    #region Skill
    [SerializeField] private GameObject shield;
    private Vector3 centerPoint;
    private Vector3 dis;
    #endregion
    [SerializeField] private GameObject bullet;
    private float x;
    private float z;
    private Vector3 dir = new Vector3(0, 0, 0);
    public static Player Instance;
    private Skill skill = new Skill();
    private bool check = true;
    private bool SkillCheck = true;
    private TimeAgent timeManager;
    public TimeAgent FskillCool;
    protected void Awake()
    {
        Instance = this;
        timeManager = new TimeAgent(0.2f, (TimeAgent) => { if (check) { Shoot(bullet, unitStat.AttackPower, 0.5f, Quaternion.identity, transform.position); check = false; } }, (TimeAgent) => check = true);
        FskillCool = new TimeAgent(8, (TimeAgent) => { if (FskillCool.CurrentTime <= 3) { Shield(); } else if (FskillCool.CurrentTime > 3) { shield.gameObject.SetActive(false); IgnoreLayer(false); } if (SkillCheck) { IgnoreLayer(true); SkillCheck = false; } }, (TimeAgent) => { SkillCheck = true; });
        InputKey = new Dictionary<KeyCode, Action>()
        {
            {KeyCode.Space,()=>TimerSystem.Instance.AddTimer(timeManager)},
            {KeyCode.F,()=>{shield.gameObject.SetActive(true);TimerSystem.Instance.AddTimer(FskillCool);}}
        };
    }

    private void Start()
    {
        StartCoroutine(Skill());

    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        dir.x = x;
        dir.z = z;
        dir.Normalize();
        transform.Translate(dir * UnitStat.MoveSpeed * Time.fixedDeltaTime, Space.World);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(x * 40, -40, 40));
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (bullet.gameObject.layer == 9)
            {
                Debug.Log(bullet.Power);
                ChangeHp(-bullet.Power);
                bullet.InObj();
            }
        }
    }

    private bool ShootCheck(bool check)
    {
        return check;
    }

    #region Key
    IEnumerator Skill()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (var dic in InputKey)
                {
                    if (Input.GetKeyDown(dic.Key))
                    {
                        dic.Value();
                        break;
                    }
                }
            }
            yield return null;
        }
    }
    #endregion

    #region skillMethod
    private void Shield()
    {
        centerPoint = transform.position;
        dis = (shield.transform.position - centerPoint).normalized;
        shield.transform.position = centerPoint + dis * 3;
        shield.transform.RotateAround(transform.position, Vector3.up, 80 * Time.deltaTime);
    }
    private void IgnoreLayer(bool b)
    {

        Physics.IgnoreLayerCollision(7, 9, b);
        Physics.IgnoreLayerCollision(7, 6, b);
    }
    #endregion
}
