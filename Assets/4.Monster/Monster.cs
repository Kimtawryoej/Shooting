using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit, I_ObseverManager
{
    [SerializeField] private GameObject monsterAppearPos;
    public GameObject MonsterAppearPos { get => monsterAppearPos; }
    public static Monster Instance;
    public List<I_Obsever> Obsevers = new List<I_Obsever>();

    override protected void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        transform.rotation = GameManager.Instance.RotationCheck(layer);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * unitStat.MoveSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            ChangeHp(-bullet.Power);
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
