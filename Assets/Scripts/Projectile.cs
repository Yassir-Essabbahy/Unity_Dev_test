using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class Projectile : MonoBehaviour
{
    [Header("settings")]
    public ParticleSystem impactFX;
    public float lifeTime = 3f; 

    private Coroutine disableCoroutine;
    private Rigidbody rb;
    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void OnEnable()
    {
        disableCoroutine = StartCoroutine(DeactivateAfterDelay());
    }

    void OnDisable()
    {
        // stop timer before time is up
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // cinemachine impulse on collision
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        // Spawn pooled impact FX
        if (impactFX != null)
        {
            if (SimpleObjectPool.Instance != null)
            {
                SimpleObjectPool.Instance.SpawnExplosion(impactFX.gameObject, transform.position, Quaternion.identity);
            }
            else
            {
                GameObject fx = Instantiate(impactFX.gameObject, transform.position, Quaternion.identity);
                Destroy(fx, 1.5f);
            }
        }

        if (collision.gameObject.CompareTag("PlayerTank"))
        {
            if (collision.gameObject.TryGetComponent<TankHealth>(out var playerHealth))
            {
                playerHealth.takeDamage(20);
            }
        }

        if (collision.gameObject.CompareTag("EnemyTank"))
        {
            if (collision.gameObject.TryGetComponent<EnemyTankHealth>(out var enemyHealth))
            {
                enemyHealth.takeDamage(20);
            }
        }

        gameObject.SetActive(false);
    }
}
