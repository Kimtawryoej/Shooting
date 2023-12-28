using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Obsever
{
    public void Refresh<T>(T value);
}
public interface I_ObseverManager
{
    public void Add(I_Obsever obsever); //UI.csó���� ��ũ��Ʈ�� �������� obsever�� ���� ����Ʈ�� �ʿ����� ������ ����
    public void Delete(I_Obsever obsever);
    public void NotifyObserver<T>(List<I_Obsever> obsevers, T value);
}

