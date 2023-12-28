using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public struct UnitStatInfo
{
    public int MaxHp;
    public int MinHp;
    public float MoveSpeed;
    public int AttackPower;
    public float Deefense;
    public int AttackPowerUp;
}


//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Rigidbody2D))]
public abstract class Unit : MonoBehaviour
{
    #region
    public LayerMask layer;

    [SerializeField] protected bool hitcheck;
    [SerializeField] protected UnitStatInfo unitStat;
    public UnitStatInfo UnitStat => unitStat;

    [SerializeField] protected int currentHp;
    public int CurrentHp => currentHp;

    protected bool isDead = false;

    protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField] protected Animator animator;
    public Animator Anim => animator;

    protected Rigidbody2D rigid;
    public Rigidbody2D Rigid => rigid;
    #endregion

    protected virtual void OnEnable()
    {
        ReSetStat();
    }

    protected virtual void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
        //rigid = GetComponent<Rigidbody2D>();
    }

    protected abstract  void OnTriggerEnter(Collider other);

    #region ReSet
    protected void ReSetStat()
    {
        ResetHp();
        //ResetSpeed();
        //ResetAttackPower();
    }

    protected virtual void ResetHp()
    {
        currentHp = unitStat.MaxHp;
    }

    //protected abstract void ResetSpeed();

    //protected abstract void ResetAttackPower();
    #endregion

    #region Hp
    public virtual void SetHp(int healValue)
    {
        ChangeHp(healValue);
    }

    public virtual void ChangeHp(int value)
    {
        if (!isDead)
        {
            ClampHp(ref value);
            if (CurrentHp <= UnitStat.MinHp)
            {
                Death();
                isDead = true;
            }
        }
    }

    protected void ClampHp(ref int value)
    {
        if (CurrentHp + value >= UnitStat.MaxHp)
        {
            currentHp = UnitStat.MaxHp;
        }

        else if (CurrentHp + value <= UnitStat.MinHp)
        {
            currentHp = UnitStat.MinHp;
        }

        else { currentHp += value; }
    }
    #endregion

    #region GetValue
    public float GetMoveSpeed()
    {
        return unitStat.MoveSpeed;
    }

    public int GetHp()
    {
        return currentHp;
    }

    public int GetMaxHp()
    {
        return unitStat.MaxHp;
    }

    public int GetAttackPower()
    {
        return unitStat.AttackPower;
    }
    #endregion

    #region Defence
    public void ChangeDeefense(float value)
    {
        unitStat.Deefense += value;
    }
    #endregion

    #region AttackPowerUp
    public void ChangeAttackPowerUp(int value)
    {
        unitStat.AttackPowerUp += value;
    }
    #endregion
    protected virtual void Death()
    {
        Debug.Log("����");
        gameObject.SetActive(false);
        //animator.SetBool("Death", true);
        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    public IEnumerator AniStop(string Aniname, string Aniset)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        animator.SetBool(Aniset, false);
        yield return new WaitForSeconds(2.5f);
    }

    #region BulletAppear
    protected IEnumerator Shoot(Func<bool>condition,GameObject bullet, int bulletPower, float bulletSpeed)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        WaitUntil until = new WaitUntil(condition);
        while (true)
        {
            yield return until;
            ObjectPool.Instance.OutObject(layer, bullet, transform.position, Quaternion.identity).gameObject.TryGetComponent(out Bullet bulletor);
            bulletor.Power = bulletPower;
            bulletor.Speed = bulletSpeed;
        }
    }
    #endregion
}
