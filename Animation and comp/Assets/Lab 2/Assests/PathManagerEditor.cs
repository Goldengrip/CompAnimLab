using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(PathManager))]

public class PathManagerEditor : Editor
{
    [SerializeField] 
    PathManager pathManager;

    [SerializeField]
    List<WayPoint> ThePath;
    List<int> toDelete;

    WayPoint selectedPoint = null;
    bool doRepaint = true; 

    private void OnSceneGUI()
    {
        ThePath = pathManager.GetPath();
        DrawPath(ThePath);
    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }


    //---------------------

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        ThePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("path");

        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path"))
            {
            pathManager.CreateAddPoint();
            }
        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();

    }
     void DrawGUIForPoints()
    {
        if (ThePath !=null && ThePath.Count > 0)
        {
            for(int i = 0; i < ThePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                WayPoint p = ThePath[i];

                Vector3 oldpos = p.GetPos();
                Vector3 newpos = EditorGUILayout.Vector3Field("", oldpos);
                //the delete button
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    //do somthing for deletion
                }
                EditorGUILayout.EndHorizontal();
               
                
            }
        }
        if (toDelete.Count > 0)
        {
            foreach(int i in toDelete)
                ThePath.RemoveAt(i);//remove from the path 
            toDelete.Clear();// clear the delete list for the next time 
        }
    }
    public void DrawPath(List<WayPoint>path)
    {
        if (path != null)
        {


            int current = 0;
            foreach (WayPoint wp in path)
            {
                //draw current point 
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                WayPoint wpnext = path[next];

                DrawPathLine(wp, wpnext);

                current += 1;



            }

        }
        if(doRepaint) Repaint();
         
    }

    public void DrawPathLine(WayPoint p1,WayPoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.grey;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    public bool DrawPoint(WayPoint p)
    {
        bool isChanged = false;
        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldpos = p.GetPos();
            Vector3 newpos = Handles.PositionHandle(oldpos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newpos);
            Handles.SphereHandleCap(-1, newpos, Quaternion.identity, 0.4f * handleSize, EventType.Repaint);
            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newpos);

            }
            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float HandlesSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity,0.25f * HandlesSize,0.25f *HandlesSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }
        return isChanged;
    }

}
