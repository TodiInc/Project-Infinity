using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] public float spawnRate = 3f;
    [SerializeField] public GameObject enemy;
    [SerializeField] public Vector3 spawnPointOne;
    [SerializeField] public Vector3 spawnPointTwo;

    [SerializeField, HideInInspector] private float height;

    void Start()
    {
        InvokeRepeating("Spawn", spawnRate, spawnRate);
    }
    void Spawn()
    {
        GameManager.Instance.getEnemies().Add(enemy);
        float y = Random.Range(spawnPointOne.y, spawnPointTwo.y);
        Vector3 spawnPoint = new Vector3(transform.position.x + spawnPointOne.x, y, 0);
        Instantiate(enemy, spawnPoint, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + spawnPointOne, new Vector3(1, 1, 0));
        Gizmos.DrawWireCube(transform.position + spawnPointTwo, new Vector3(1, 1, 0));
    }
}
