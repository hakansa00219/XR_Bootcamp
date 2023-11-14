using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] private Color _patrolRouteColor;
    [SerializeField] private PatrolType _patrolType;
    [SerializeField] private List<Transform> _route = new List<Transform>();


    public List<Transform> Route => _route;
    public PatrolType Patrol => _patrolType;

    [Button]
    private void AddPatrolPoint()
    {
        GameObject point = new GameObject();
        point.transform.parent = transform;
        point.transform.position = _route != null && _route.Count > 0 ? _route[_route.Count - 1].position : transform.position;
        point.name = "Point" + (_route.Count + 1);
        _route.Add(point.transform);

#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(point, "Add Patrol Point");
#endif
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = _patrolRouteColor;
        Handles.Label(transform.position, gameObject.name);

#endif

        Gizmos.color = _patrolRouteColor;

        if (_patrolType == PatrolType.Loop && _route.Count > 2)
        {
            Gizmos.DrawLine(_route[0].position, _route[_route.Count - 1].position);
        }
        for (int i = 1; i < _route.Count; i++)
        {
            Gizmos.DrawLine(_route[i].position, _route[i - 1].position);
        }    
    }


    public enum PatrolType
    {
        Loop = 0,
        PingPong = 1,
    }
}
