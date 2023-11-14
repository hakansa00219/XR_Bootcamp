using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _threshold;
    [SerializeField] private PatrolRoute _patrolRoute;


    private int _increment = 1;
    private void Start()
    {
        StartCoroutine(AI());
    }

    private IEnumerator AI()
    {
        while(true)
        {
            yield return Patrolling();
            yield return null;
        }
    }

    private IEnumerator Patrolling()
    {
        switch (_patrolRoute.Patrol)
        {
            case PatrolRoute.PatrolType.Loop:

                foreach (Transform t in _patrolRoute.Route)
                {
                    yield return AgentRoute(t);
                }

                break;
            case PatrolRoute.PatrolType.PingPong:

                for (int i = 1; i < _patrolRoute.Route.Count; i++)
                {
                    yield return AgentRoute(_patrolRoute.Route[i]);
                }
                for (int i = _patrolRoute.Route.Count - 2; i >= 0; i--)
                {
                    yield return AgentRoute(_patrolRoute.Route[i]);
                }

                break;
        }
    }

    private IEnumerator CheckDistance(Transform t) => new WaitUntil(() => Vector3.Distance(t.position, transform.position) <= _threshold);

    private IEnumerator AgentRoute(Transform destination)
    {
        _agent.SetDestination(destination.position);
        yield return CheckDistance(destination);
    }

    //Baþka ping pong örneði.
    private IEnumerator PingPongLoop()
    {
        for (int i = 0; i < _patrolRoute.Route.Count; i += _increment)
        {
            if (i == -1)
            {
                _increment = 1;
                break;
            }

            _agent.SetDestination(_patrolRoute.Route[i].position);
            yield return CheckDistance(_patrolRoute.Route[i]);

            if (i == _patrolRoute.Route.Count - 1)
            {
                _increment = -1;
            }
        }
    }
}
