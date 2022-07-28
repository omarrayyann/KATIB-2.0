using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatibInformation : MonoBehaviour
{
    public List<Vector2> points;
    private GameObject magnetObject;
    private bool lockOnPath;
    public int hapticLevel { get; set; }

    public KatibInformation(List<Vector2> p, GameObject mObject, bool lOnPath)
    {
        points = p;
        magnetObject = mObject;
        FollowMouse follow = magnetObject.AddComponent<FollowMouse>() as FollowMouse;
        follow.lockOnPath = lockOnPath;
    }
}
