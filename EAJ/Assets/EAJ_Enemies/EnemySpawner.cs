using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    private bool bSpawn = false; 
    public SixDOFController Player;

    public List<Enemy> ActiveEnemies;
    public List<Enemy> AllEnemies;

    private int nAllEnemies;
    private int nCurrentEnemy = 0;
    private bool bInitialized = false;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

        if (bInitialized == false && SceneManager.GetActiveScene().name != "EAJ_Menu")
        {
            AllEnemies = GetAllEnemies(GameObject.Find("EnemyQueue").transform);


            nAllEnemies = AllEnemies.Count;

            Player = FindObjectOfType<SixDOFController>();

            foreach (Enemy enemy in AllEnemies)
            {
                enemy.transform.parent.transform.parent.gameObject.SetActive(false);
            }

            bInitialized = true;
        }

        if (!bInitialized)
        {
            return;
        }
        
        bSpawn = ActiveEnemies.Count == 0 ? true : false;
        
        if (bSpawn)
        {
            SpawnEnemy();
        }
    }

    public List<Enemy> GetAllEnemies(Transform enemyQueueParent)
    {
        List<Enemy> enemies = new List<Enemy>();
        GetComponentsInChildrenIncludingInactive(enemyQueueParent, enemies);
        return enemies;
    }

    private void GetComponentsInChildrenIncludingInactive(Transform parent, List<Enemy> result)
    {
        foreach (Transform child in parent)
        {
            Enemy enemy = child.GetComponent<Enemy>();
            if (enemy != null)
            {
                result.Add(enemy);
            }

            // Recursively get enemies from child objects
            GetComponentsInChildrenIncludingInactive(child, result);
        }
    }


    private void SpawnEnemy()
    {
        if (bSpawn == false)
        {
            return;
        }
        
        if (nCurrentEnemy >= nAllEnemies)
        {
            bSpawn = false;
            return;
        }

        Enemy nextEnemy = AllEnemies[nCurrentEnemy];

        if (nextEnemy == null)
        {
            bSpawn = false;
            return;
        }
        
        EEnemySpawnGroup spawnGroup = nextEnemy.EnemySpawnGroup;

        do
        {
            Enemy enemyToSpawn = AllEnemies[nCurrentEnemy];
            enemyToSpawn.transform.parent.transform.parent.gameObject.SetActive(true);
            
            ActiveEnemies.Add(enemyToSpawn);
            
            if (enemyToSpawn.GetComponent<EnemyAI>() != null)
            {
                enemyToSpawn.GetComponent<EnemyAI>().SetPlayerTarget(Player);
            }

            nCurrentEnemy++;
        } while (AllEnemies[nCurrentEnemy].EnemySpawnGroup == spawnGroup &&
                 AllEnemies[nCurrentEnemy].EnemySpawnGroup != EEnemySpawnGroup.NONE);

    }
}