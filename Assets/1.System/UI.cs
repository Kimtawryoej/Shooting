using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    #region UI
    [SerializeField] private Slider Hp;
    [SerializeField] private Slider FskillCool;
    #endregion
    void Update()
    {
        Hp.value = Player.Instance.CurrentHp / Player.Instance.UnitStat.MaxHp;
        FskillCool.value = Player.Instance.FskillCool.CurrentTime / 8;
    }
}
