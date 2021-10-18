using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    private readonly int width = 20;
    private readonly int height = 20;
    [SerializeField] private GameObject obstaclePrefab;    
    [SerializeField] private GameObject playerPrefab;   
    [SerializeField] private GameObject finishPrefab; 
    [SerializeField] private GameObject coinPrefab; 
    [SerializeField] private GameObject maze;
    private bool playerSpawned = false;
    private bool finishSpawned = false;
    private bool isAutoGenerator = false;

    public static GameManager Instance;
    public int price = 1;
    public int coins = 0;
    public int level = 1;
    public bool isActiveGame;
    public bool isCompleted;
    public bool isFail;

    [SerializeField] GameObject menu, completedMenu, failMenu;
    [SerializeField] Text allCoins, numLevel, mode;
    [SerializeField] GameObject LevelGenerator;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Load();
    }
  
    void Update()
    {
        ModeCheck();
        OutputCoinsAndLevel();
    }
    
    public void Mode()
    {
        isAutoGenerator = !isAutoGenerator;
    }

    public void ModeCheck()
    {
        string nameMode;
        if(isAutoGenerator)
        {
            maze.SetActive(false);
            nameMode = "Auto Generator";
        }
        else
        {
            maze.SetActive(true);
            nameMode = "Normal";
        }
        mode.text = "MODE: " + nameMode;
    }

    public void Buy()
    {
        coins -= price;
        price++;
        Save();
    }

    private void OutputCoinsAndLevel()
    {
        allCoins.text = coins.ToString();
        numLevel.text = "LEVEL: " + level.ToString(); 
    }

    public void Fail()
    {
        playerSpawned = false;
        isFail = true;
        isActiveGame = false;
        failMenu.SetActive(true);
        Save();
    }

    public void Completed()
    {
        level++;
        playerSpawned = false;
        isCompleted = true;
        isActiveGame = false;
        completedMenu.SetActive(true);
        Save();
    }

    public void Play()
    {
        GenerateLevel();
        menu.SetActive(false);
        isActiveGame = true; 
    }

    public void Restart()
    {
        ClearScene();
        GenerateLevel();
        failMenu.SetActive(false);
        isActiveGame = true;
    }

    public void Continue()
    {
        ClearScene();
        GenerateLevel();
        completedMenu.SetActive(false);
        isActiveGame = true;
    }

    public void ClearScene()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (var item in coins)
        {
            Destroy(item.gameObject);
        }
        GameObject[] Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var item in Obstacles)
        {
            Destroy(item.gameObject);
        }
        Destroy(GameObject.Find("Finish(Clone)"));
        Destroy(GameObject.Find("Player(Clone)"));
        finishSpawned = false;
        playerSpawned = false;
    }

    public void GenerateLevel()
    {
        if (!finishSpawned)
        {
            Vector3 posFinish = new Vector3(Random.Range(-9, 9), 10, -1.1f);
            Instantiate(finishPrefab, posFinish, Quaternion.identity);
            finishSpawned = true;
        }
        if (!playerSpawned)
        {
            Vector3 posPlayer = new Vector3(Random.Range(-6, 6), -9, 1.5f);
            Instantiate(playerPrefab, posPlayer, Quaternion.identity);
            playerSpawned = true;
        }
        
        for (int x = 0; x <= width; x += 2)
        {
            for (int y = 0; y <= height; y += 2)
            {
                if (Random.value > .7f && isAutoGenerator)
                {
                    Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, -0.5f);
                    Instantiate(obstaclePrefab, pos, Quaternion.identity, transform);
                }
                else if (Random.value > .9f)
                {
                    Vector3 pos = new Vector3(x - width / 2f, y - height / 2f, -0.5f);
                    Instantiate(coinPrefab, pos, Quaternion.identity);
                }
            }
        }
        surface.BuildNavMesh();
    }



    [System.Serializable]
    class SaveData
    {
        public int saveCoin;
        public int saveLevel;
        public int savePrice;
    }

    public void Save()
    {
        SaveData data = new SaveData
        {
            saveCoin = coins,
            saveLevel = level,
            savePrice = price
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            coins = data.saveCoin;
            level = data.saveLevel;
            price = data.savePrice;
        }
    }

}
