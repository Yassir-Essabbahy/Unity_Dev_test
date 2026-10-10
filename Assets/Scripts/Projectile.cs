using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class Projectile : MonoBehaviour
{
    [Header("settings")]
    public ParticleSystem impactFX;
    public float lifeTime = 3f; //bullet set inactive time

    private Coroutine disableCoroutine;

    void OnEnable()
    {
        disableCoroutine = StartCoroutine(DeactivateAfterDelay());
    }

    void OnDisable()
    {
        //stop timer before times up
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
    }

        private IEnumerator DeactivateAfterDelay()
        {
            yield return new WaitForSeconds(lifeTime);
            gameObject.SetActive(false);
        }

    private void OnCollisionEnter(Collision collision)
    {
        //cinemachine impulse on collision
        CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
        if (impulse != null)
        {
            impulse.GenerateImpulse();
        }

        if (impactFX != null)
        {
        Instantiate(impactFX, transform.position, Quaternion.identity);

        }

        if(collision.gameObject.CompareTag("PlayerTank"))
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
