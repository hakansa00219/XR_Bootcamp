using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol = 0,
        Investigate = 1
    }

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _threshold;
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private PatrolRoute _patrolRoute;
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyState _state = EnemyState.Patrol;


    private int _routeIndex = 0;
    private bool _increasing = true;
    private Vector3 _investigationPoint;
    private float _waitTimer = 0;
    private void Start()
    {

    }

    private void Update()
    {
        if (_fov.IsObjectSeen(out Vector3 visibleObject))
        {
            InvestigatePoint(visibleObject);
        }
        
        switch (_state)
        {
            case EnemyState.Patrol:
                Patrolling();
                break;
            case EnemyState.Investigate:
                Investigating();
                break;
        }
    }

    public void InvestigatePoint(Vector3 investigatePoint)
    {
        _state = EnemyState.Investigate;
        _investigationPoint = investigatePoint;
        _agent.SetDestination(_investigationPoint);
    }

    private void Investigating()
    {
        if(Vector3.Distance(transform.position, _investigationPoint) < _threshold)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer > _waitTime)
            {
                ReturnToPatrol();
            }
        }
    }

    private void ReturnToPatrol()
    {
        _state = EnemyState.Patrol;
        _waitTimer = 0;
    }

    private void Patrolling()
    {
        AgentRoute(_patrolRoute.Route[_routeIndex]);
    }

    private bool CheckDistance(Transform t) => Vector3.Distance(t.position, transform.position) <= _threshold;

    private void AgentRoute(Transform destination)
    {
        _agent.SetDestination(destination.position);
        if (CheckDistance(destination))
        {
            switch (_patrolRoute.Patrol)
            {
                case PatrolRoute.PatrolType.Loop: LoopRouteCalculation(); break;
                case PatrolRoute.PatrolType.PingPong: PingPongRouteCalculation(); break;
            }
        }
        
    }

    private void PingPongRouteCalculation()
    {
        if (_routeIndex == _patrolRoute.Route.Count - 1) _increasing = false;
        else if (_routeIndex == 0) _increasing = true;
        _routeIndex += _increasing ? 1 : -1;
    }

    private void LoopRouteCalculation()
    {
        _routeIndex = (_routeIndex + 1) % _patrolRoute.Route.Count;
    }
}
