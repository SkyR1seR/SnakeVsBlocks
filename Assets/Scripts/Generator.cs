using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using static UnityEditor.Progress;

public class Generator : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private GameObject _nextLevelWin;
    [SerializeField] private GameObject _restartLevelWin;
    [SerializeField] private GameObject _menuWin;
    public int Lvl = 1;

    [SerializeField] private int _roomCount = 6;
    [SerializeField] private Transform _player;
    [SerializeField] private Move _playerMove;

    [SerializeField] private GameObject _foodPref;
    [SerializeField] private GameObject _wallPref;
    [SerializeField] private GameObject _barricadePref;
    [SerializeField] private GameObject _finishPref;
    [SerializeField] private GameObject _roadPref;

    public static Generator generator;

    List<GameObject> currentLvl = new List<GameObject>();

    private void Start()
    {

        GenerateLevel();

        if (generator == null)
        {
            generator = this;
        }
        else
        {
            generator = null;
        }
    }

    private void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
        {
            ShowMenu();
        }
    }

    public void ShowMenu()
    {
        
        if (!_menuWin.active)
        {
            _playerMove.speed = 0;
            _menuWin.SetActive(true);
        }
        else
        {
            _playerMove.speed = 2f;
            _menuWin.SetActive(false);
        }
    }

    public void FinishLevel()
    {
        _playerMove.speed = 0;
        SoundScript.soundScript.PlayWin();
        _nextLevelWin.SetActive(true);
    }

    public void DestoyLevel()
    {
        foreach (var item in currentLvl)
        {
            Destroy(item);
        }
    }

    public void NextLevel()
    {
        DestoyLevel();
        currentLvl = new List<GameObject>();
        Lvl++;
        _levelText.text = $"Уровень: {Lvl}";

        GenerateLevel();

        _nextLevelWin.SetActive(false);
        _playerMove.speed = 2f;
    }

    public void GameOver()
    {
        SoundScript.soundScript.PlayLose();
        Score.score.RemoveTail();
        _playerMove.GoToStart();
        _restartLevelWin.SetActive(true);
        _playerMove.speed = 0f;
        //GameObject[] oldLevel = GameObject.FindGameObjectsWithTag("Level");
        //foreach (var item in oldLevel)
        //{
        //    Destroy(item);
        //}
    }

    public void RestartLevel()
    {
        Score.score.RemoveTail();
        _playerMove.GoToStart();
        _menuWin.SetActive(false);
        Wall bufWall;
        foreach (var item in currentLvl)
        {
            if (item.CompareTag("Wall"))
            {
                bufWall = item.GetComponent<Wall>();
                bufWall.ResetCube();
            }
            item.SetActive(true);
        }

        //GenerateLevel();
        _restartLevelWin.SetActive(false);
        _playerMove.speed = 2f;
    }

    public void GenerateLevel()
    {
        _playerMove.SetStartPos();
        GenerateRoad(_player.position.z);
        GenerateFood(_player.position.z + 7);
        GenerateFood(_player.position.z + 8);
        for (int i = 0; i < _roomCount + Lvl/5; i++)
        {
            GenerateWall(_player.position.z + 11 + 7*i);
            GenerateRoom(_player.position.z + 12 + 7*i);
        }
        GenerateFinish(_player.position.z + 19 + 7 * (_roomCount- 1 + Lvl / 5));
    }

    private void GenerateRoad(float zPos)
    {
        for (int i = 0; i < (_roomCount + Lvl / 5)*1.5f; i++)
        {
            currentLvl.Add(Instantiate(_roadPref, new Vector3(0, 0, zPos -2f + 8f * i), Quaternion.identity, transform));
        }
        
    }

    List<Wall> walls = new List<Wall>();
    private void GenerateWall(float zPos)
    {
        walls = new List<Wall>();
        GameObject first = Instantiate(_wallPref, new Vector3(-1.6f, 0, zPos), Quaternion.identity, transform);
        walls.Add(first.GetComponent<Wall>());
        for (int i = 1; i < 5; i++)
        {
            walls.Add(Instantiate(_wallPref, new Vector3(first.transform.position.x + 0.8f * i, 0, zPos), Quaternion.identity, transform).GetComponent<Wall>());
        }
        foreach (Wall item in walls)
        {
            item.HP = Random.Range(1, 51);
        }
        walls[Random.Range(0, walls.Count)].HP = Random.Range(1, 4);


        foreach (var item in walls)
        {
            currentLvl.Add(item.gameObject);
        }
    }


    float[] pos = { -1.6f, -0.8f, 0f, 0.8f, 1.6f };

    List<Food> foods;
    private void GenerateFood(float zPos)
    {
        foods = new List<Food>();
        bool[] isSpawn = new bool[5];
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            for (int b = 0; b < 1;)
            {
                int line = Random.Range(0, 5);
                if (!isSpawn[line])
                {
                    foods.Add(Instantiate(_foodPref, new Vector3(pos[line], 0, zPos), Quaternion.identity, transform).GetComponent<Food>());
                    isSpawn[line] = true;
                    break;
                }
            }
                
        }
        foreach (Food item in foods)
        {
            item.foodCount = Random.Range(1, 5);
        }

        foreach (var item in foods)
        {
            currentLvl.Add(item.gameObject);
        }
    }

    private void GenerateCubes(float zPos)
    {
        walls = new List<Wall>();
        bool[] isSpawn = new bool[5];
        float[] spawnPos = { zPos, zPos + 3 };
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            for (int b = 0; b < 1;)
            {
                int line = Random.Range(0, 5);
                if (!isSpawn[line])
                {
                    walls.Add(Instantiate(_wallPref, new Vector3(pos[line], 0, spawnPos[Random.Range(0, spawnPos.Count())]), Quaternion.identity, transform).GetComponent<Wall>());
                    isSpawn[line] = true;
                    break;
                }
            }
        }
        foreach (Wall item in walls)
        {
            item.HP = Random.Range(1, 51);
        }

        foreach (var item in walls)
        {
            currentLvl.Add(item.gameObject);
        }
    }

    private void GenerateBarricade(float zPos, int size = 1)
    {
        bool[] isSpawn = new bool[4];
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            for (int b = 0; b < 1;)
            {
                int line = Random.Range(0, 4);
                if (!isSpawn[line])
                {

                    GameObject br = Instantiate(_barricadePref, new Vector3(-1.2f + 0.8f * line, 0, zPos), Quaternion.identity, transform);
                    br.transform.localScale = new Vector3(1, 1, size);
                    currentLvl.Add(br);
                    isSpawn[line] = true;
                    break;
                }
            }

        }
    }

    private void GenerateFinish(float zPos)
    {
        currentLvl.Add(Instantiate(_finishPref, new Vector3(0, 0, zPos), Quaternion.identity, transform));
    }

    private void GenerateRoom(float zPos)
    {
        GenerateBarricade(zPos);
        GenerateFood(zPos);

        GenerateCubes(zPos + 1);

        GenerateBarricade(zPos + 2.5f, 2);
        GenerateFood(zPos+2.5f);

        GenerateBarricade(zPos + 5f);
        GenerateFood(zPos+5f);
    }

}
