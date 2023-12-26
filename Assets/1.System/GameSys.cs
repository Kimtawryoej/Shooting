using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameSys : MonoBehaviour
{

}
public class LinkedStack<T> : MonoBehaviour
{
    private T OneDestory;
    private LinkedList<T> Stack = new LinkedList<T>();
    public void Add(T Object) { if (!Stack.Contains(Object)) { Stack.AddLast(Object); } }
    public T Push() { OneDestory = Stack.Last(); Stack.Remove(OneDestory); return OneDestory; }
    public void Remove(T Object) { Stack.Remove(Object); }
    public void Clear() { Stack.Clear(); }
    public int Count() { return Stack.Count; }
}
