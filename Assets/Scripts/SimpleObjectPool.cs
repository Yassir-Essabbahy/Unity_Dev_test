using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour
{
    public static SimpleObjectPool Instance;

    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 25;

    private List<GameObject> pooledProjectiles = new List<GameObject>();


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
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is missing", this);
            enabled = false;
            return;
        }

        //load bullets to memory
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform);
            projectile.SetActive(false);
            pooledProjectiles.Add(projectile);
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

        GameObject newProjectile = Instantiate(projectilePrefab, transform);
        newProjectile.SetActive(false);
        pooledProjectiles.Add(newProjectile);

        return newProjectile;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}