using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] private Color patrolRouteColor;
    [SerializeField] private PatrolType patrolType;
    [SerializeField] private List<Transform> route = new List<Transform>();


    public List<Transform> Route => route;

    [Button]
    private void AddPatrolPoint()
    {
        GameObject point = new GameObject();
        point.transform.parent = transform;
        point.transform.position = route[route.Count - 1] != null ? route[route.Count - 1].position : Vector3.zero;
        point.name = "Point" + (route.Count + 1);
        route.Add(point.transform);

#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(point, "Add Patrol Point");
#endif
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = patrolRouteColor;
        Handles.Label(transform.position, gameObject.name);

#endif

        Gizmos.color = patrolRouteColor;

        if (patrolType == PatrolType.Loop && route.Count > 2)
        {
            Gizmos.DrawLine(route[0].position, route[route.Count - 1].position);
        }
        for (int i = 1; i < route.Count; i++)
        {
            Gizmos.DrawLine(route[i].position, route[i - 1].position);
        }    
    }


    public enum PatrolType
    {
        Loop = 0,
        PingPong = 1,
    }
}
