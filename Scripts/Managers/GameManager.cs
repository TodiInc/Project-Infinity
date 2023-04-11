using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{

    [SerializeField, HideInInspector] private float spawnTimer = 5.0f;
    [SerializeField, HideInInspector] private List<GameObject> players = new List<GameObject>();
    [SerializeField, HideInInspector] private List<GameObject> enemies = new List<GameObject>();

    [SerializeField, HideInInspector] public int enemiesKilled = 0;

    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public float spawnCooldown = 5.0f;
    [SerializeField] private Canvas HUD;

    [SerializeField] private static GameManager _Instance;

    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<GameManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    public void addToPlayersList(GameObject player){
        players.Add(player);
    }
    public void addToEnemiesList(GameObject enemy){
        enemies.Add(enemy);
    }
    public List<GameObject> getEnemies(){
        return enemies;
    }
    public List<GameObject> getPlayers(){
        return players;
    }

    public void Awake(){
        enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy");
        InvokeRepeating("SpawnEnemies",spawnTimer,spawnCooldown);
    }

    public void SpawnEnemies(){
        Vector3 leftMostPosition = GetFurthestPlayerPosition(true);
        Vector3 rightMostPosition = GetFurthestPlayerPosition(false);

        leftMostPosition.x += 15f;
        rightMostPosition.x -= 15f;
        float spawnX = leftMostPosition.x;
        if (Random.value > 0.5f)
        {
            spawnX = rightMostPosition.x;
        }

        float spawnY = Random.Range(-2f, 2f);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        GameObject enemyClone = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(enemyClone);
    }

    private Vector3 GetFurthestPlayerPosition(bool leftMost)
    {
        int randomPlayer = Random.Range(0,GameManager.Instance.getPlayers().Count);
        Vector3 furthestPosition = players[randomPlayer].transform.position;
        foreach (GameObject player in players)
        {
            if (leftMost && player.transform.position.x > furthestPosition.x)
            {
                furthestPosition = player.transform.position;
            }
            else if (!leftMost && player.transform.position.x < furthestPosition.x)
            {
                furthestPosition = player.transform.position;
            }
        }
        return furthestPosition;
    }
}
