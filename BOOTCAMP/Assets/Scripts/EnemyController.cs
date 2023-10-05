using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float threshold;
    [SerializeField] private PatrolRoute patrolRoute;

    private void Start()
    {
        StartCoroutine(AI());
    }

    private IEnumerator AI()
    {
        while(true)
        {
            foreach(Transform t in patrolRoute.Route)
            {
                agent.SetDestination(t.position);
                yield return new WaitUntil(() => Vector3.Distance(t.position, transform.position) <= threshold);
            }
            yield return null;
        }
    }
}
