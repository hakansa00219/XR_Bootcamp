using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private float _soundRadius = 5f;
    [SerializeField] private float _impulseThreshold = 2f;

    private float _collisionTimer = 0f;
    
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_collisionTimer < 2f)
        {
            _collisionTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_collisionTimer < 2f) return;
        
        
        if (collision.impulse.magnitude > _impulseThreshold || collision.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();

            Collider[] collisions = Physics.OverlapSphere(transform.position, _soundRadius);

            foreach (Collider collider in collisions)
            {
                if (collider.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.InvestigatePoint(transform.position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _soundRadius);
    }
}
