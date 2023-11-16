using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public List<Transform> visibleObjects = new List<Transform>();

    [SerializeField] private Color _gizmoColor = Color.red;
    [SerializeField] private float _viewRadius = 6f;
    [SerializeField] private float _viewAngle = 30f;
    [SerializeField] private Creature _creature;
    [SerializeField] private LayerMask _blockedLayers;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        visibleObjects.Clear();
        var targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius);

        foreach (var target in targetsInViewRadius)
        {
            if (!target.TryGetComponent(out Creature targetCreature)) continue;

            if (_creature.team == targetCreature.team) continue;

            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < _viewAngle)
            {
                Vector3 headPos = _creature.head.position;
                Vector3 targetHeadPos = targetCreature.head.position;

                Vector3 dirToTargetHead = (targetHeadPos - headPos).normalized;
                float distToTargetHead = Vector3.Distance(headPos, targetHeadPos);

                if(Physics.Raycast(headPos, dirToTargetHead, distToTargetHead, _blockedLayers))
                {
                    continue;
                }
                Debug.DrawLine(headPos, targetHeadPos, Color.green);

                visibleObjects.Add(target.transform);
            }
        }
    }

    public bool IsObjectSeen(out Vector3 seenObject)
    {
        if(visibleObjects.Count > 0)
        {
            seenObject = visibleObjects[0].position;
            return true;
        }
        seenObject = default;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Handles.color = _gizmoColor;

        Handles.DrawWireArc(transform.position, transform.up, transform.forward, _viewAngle, _viewRadius);
        Handles.DrawWireArc(transform.position, transform.up, transform.forward, -_viewAngle, _viewRadius);

        Vector3 lineA = Quaternion.AngleAxis(_viewAngle, transform.up) * transform.forward;
        Vector3 lineB = Quaternion.AngleAxis(-_viewAngle, transform.up) * transform.forward;
        Handles.DrawLine(transform.position, transform.position + (lineA * _viewRadius));
        Handles.DrawLine(transform.position, transform.position + (lineB * _viewRadius));
    }
}
