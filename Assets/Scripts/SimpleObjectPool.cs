using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour
{
    public static SimpleObjectPool Instance;

    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 25;

    [System.NonSerialized] public GameObject explosionPrefab;
    private int explosionPoolSize = 15;

    private List<GameObject> pooledProjectiles = new List<GameObject>();
    private List<GameObject> pooledExplosions = new List<GameObject>();

    // setup singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (projectilePrefab != null)
        {
            // load bullets to memory
            for (int i = 0; i < poolSize; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform);
                projectile.SetActive(false);
                pooledProjectiles.Add(projectile);
            }
        }
        else
        {
            Debug.LogWarning("SimpleObjectPool: Projectile prefab is missing.", this);
        }

        if (explosionPrefab != null)
        {
            for (int i = 0; i < explosionPoolSize; i++)
            {
                GameObject fx = Instantiate(explosionPrefab, transform);
                fx.SetActive(false);
                pooledExplosions.Add(fx);
            }
        }
    }

    public GameObject GetProjectile()
    {
        foreach (GameObject projectile in pooledProjectiles)
        {
            if (projectile != null && !projectile.activeSelf)
            {
                return projectile;
            }
        }

        if (projectilePrefab != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, transform);
            newProjectile.SetActive(false);
            pooledProjectiles.Add(newProjectile);
            return newProjectile;
        }

        return null;
    }

    public GameObject SpawnExplosion(GameObject fallbackPrefab, Vector3 position, Quaternion rotation, float duration = 1.5f)
    {
        //find inactive explosion in pool
        foreach (GameObject fx in pooledExplosions)
        {
            if (fx != null && !fx.activeSelf)
            {
                fx.transform.position = position;
                fx.transform.rotation = rotation;
                fx.SetActive(true);
                StartCoroutine(DeactivateAfterDelay(fx, duration));
                return fx;
            }
        }

        // Expand pool
        GameObject template = explosionPrefab != null ? explosionPrefab : fallbackPrefab;
        if (template != null)
        {
            GameObject newFx = Instantiate(template, position, rotation, transform);
            newFx.SetActive(true);
            pooledExplosions.Add(newFx);
            StartCoroutine(DeactivateAfterDelay(newFx, duration));
            return newFx;
        }

        return null;
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}