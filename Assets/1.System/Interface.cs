using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Obsever
{
    public void Refresh<T>(T value);
}
public interface I_ObseverManager
{
    public void Add(I_Obsever obsever); //UI.cs처럼한 스크립트에 여러개의 obsever을 담을 리스트가 필요하지 않을수 있음
    public void Delete(I_Obsever obsever);
    public void NotifyObserver<T>(List<I_Obsever> obsevers, T value);
}

