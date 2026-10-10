using UnityEngine;
using Unity.Cinemachine;

public class ThrowProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("Drag your Rigidbody‐based projectile prefab here.")]
    public Rigidbody projectilePrefab;

    [Tooltip("Where the projectile will be spawned from.")]
    public Transform throwPoint;

    [Tooltip("Initial speed applied to the projectile.")]
    public float launchForce = 15f;

    [Header("Fire Rate")]
    [Tooltip("Minimum time (in seconds) between consecutive shots.")]
    public float timeBetweenShots = 0.5f;

    // Time at which we’re next allowed to shoot
    private float nextTimeToFire = 0f;

    //Cinemachine impulse component
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        // On left mouse‐button down, check cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            ThrowProjectile1();
            nextTimeToFire = Time.time + timeBetweenShots;
        }
    }

    private void ThrowProjectile1()
    {
        if (projectilePrefab == null || throwPoint == null)
        {
            Debug.LogWarning("ProjectileThrower: Assign 'projectilePrefab' and 'throwPoint' in the Inspector.");
            return;
        }
        
        // impluse fire
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        GameObject proj = SimpleObjectPool.Instance.GetProjectile();
        if (proj != null)
        {
            proj.transform.position = throwPoint.position;
            proj.transform.rotation = throwPoint.rotation;
            proj.SetActive(true);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Immediately give it velocity forward based on throwPoint’s forward vector
                rb.linearVelocity = throwPoint.forward * launchForce;
            }
        }

    }
}
