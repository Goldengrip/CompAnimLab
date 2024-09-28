using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<WayPoint> path;

    public List<WayPoint> GetPath()
    {
        if(path == null) 
            path = new List<WayPoint>();
        return path;
    }

    public void CreateAddPoint()
    {
        WayPoint go = new WayPoint();
        path.Add(go);
    }
    


}
