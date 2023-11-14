using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private Color _gizmoColor = Color.red;
    [SerializeField] private float _viewRadius = 6f;
    [SerializeField] private float _viewAngle = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius);

        foreach (var target in targetsInViewRadius)
        {
            if (!target.TryGetComponent(out Creature _)) continue;

            Debug.Log(target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Vector3 lineA = Quaternion.AngleAxis(_viewAngle, transform.up) * transform.forward;
        Vector3 lineB = Quaternion.AngleAxis(-_viewAngle, transform.up) * transform.forward;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawLine(transform.position, transform.position + (lineA * _viewRadius));
        Gizmos.DrawLine(transform.position, transform.position + (lineB * _viewRadius));
    }
}
