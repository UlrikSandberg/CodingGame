using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player playerPrefab;
    public GameObject levelPrefab;
    public Transform spawnPosition;
    public Transform enemySpawn;

    // Hold the different gamePlay objects
    private Player player;
    private List<Enemy> enemies;
    private GameObject level;
    private List<Turret> turrets;

    private bool isActive;
    
    // Start is called before the first frame update
    void Start()
    {
        turrets = new List<Turret>(FindObjectsOfType<Turret>());
        Restart();
    }

    void Restart()
    {
        foreach(var turret in turrets)
        {
            turret.isActive = false;
            turret.Reset();
        }

        isActive = false;
        if(level != null)
        {
            Destroy(level);
        }
        if(player != null)
        {
            Destroy(player.gameObject);
        }

        level = Instantiate(levelPrefab, enemySpawn.position, Quaternion.identity);
        player = Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity);

        player.SetStartLineCallback(StartCallback);
        player.SetGoalCallback(GoalCallBack);

        var f = FindObjectsOfType<FragmentSpawner>();
        for(int i = f.Length -1; i >= 0; i--)
        {
            Destroy(f[i].gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        // Find all the guards and set them to active when player steps onto startLine!
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        if(isActive)
        {
            foreach(var enemy in enemies)
            {
                enemy.isActive = true;
            }
            foreach(var turret in turrets)
            {
                turret.isActive = true;
            }
        }
        else
        {
            foreach(var enemy in enemies)
            {
                enemy.isActive = false;
            }
            foreach (var turret in turrets)
            {
                turret.isActive = false;
            }
        }

        foreach(var turret in turrets)
        {
            turret.TargetClosest(enemies.ToArray());
        }
    }

    private void GoalCallBack()
    {
        isActive = false;
    }

    private void StartCallback()
    {
        isActive = true;
    }
}
