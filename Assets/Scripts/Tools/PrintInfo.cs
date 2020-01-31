using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintInfo : MonoBehaviour
{
  public void PrintNotification()
  {
    Debug.Log(gameObject.name);
  }

  public void PrintData(int data)
  {
    Debug.Log(data + " " + gameObject.name);
  }

  public void PrintData(string data)
  {
    Debug.Log(data + " " + gameObject.name);
  }
  public void PrintData(float data)
  {
    Debug.Log(data + " " + gameObject.name);
  }
}
