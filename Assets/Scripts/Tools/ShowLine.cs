using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ShowLine : MonoBehaviour
{
  public bool forward;
  public bool back;
  public bool up;
  public bool down;
  public bool right;
  public bool left;
  void Update()
  {
    if (forward)
    {
      Debug.DrawRay(transform.position, transform.forward, Color.red);
    }
    if (back)
    {
      Debug.DrawRay(transform.position, -transform.forward, Color.magenta);
    }
    if (up)
    {
      Debug.DrawRay(transform.position, transform.up, Color.green);
    }
    if (down)
    {
      Debug.DrawRay(transform.position, -transform.up, Color.yellow);
    }
    if (right)
    {
      Debug.DrawRay(transform.position, transform.right, Color.blue);
    }
    if (left)
    {
      Debug.DrawRay(transform.position, -transform.right, Color.cyan);
    }
  }
}
