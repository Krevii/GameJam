using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PerlinNoiseGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public int width = 50;
    public int height = 50;
    public float scale = 1f;
    public int octaves = 3;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public Vector2 offset;
    public int islandRadius = 50;

    public Tile[] tiles;
    public Tile highTile;
    public Tile lowTile;
    public Tile waterTile;
    public Tile lineTile;
    public Tile[] groundTile;

    Tilemap tilemap;
    private List<List<Vector3Int>> islands = new List<List<Vector3Int>>();
    private GameObject player;
    public GameObject bonfire;
    public GameObject spawner;
    public GameObject upgrader;
    public List<GameObject> resources = new List<GameObject>();
    public List<GameObject> bonfires = new List<GameObject>();
    public List<GameObject> rocks = new List<GameObject>();
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> rocksObj = new List<GameObject>();
    private GameObject upgraderObj = null;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        GenerateLevel(offset = new Vector2(Random.value * 100, Random.value * 100));
        GenerateBase();
        player = GameObject.FindGameObjectWithTag("Player");

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.ClearDeveloperConsole();
            GenerateLevel(offset = new Vector2(Random.value * 10000, Random.value * 10000));
            GenerateBase();
        }
    }

    private void GenerateLevel(Vector2 offset)
    {
        try
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = 0f;
                    float amplitude = 1f;
                    float frequency = 1f;
                    for (int i = 0; i < octaves; i++)
                    {
                        float xCoord = (float)x / width * scale * frequency + offset.x;
                        float yCoord = (float)y / height * scale * frequency + offset.y;

                        noise += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    int index = Mathf.FloorToInt(noise * (tiles.Length - 1));

                    Tile tile = tiles[index];

                    Vector3Int position = new Vector3Int(x, y, 0);
                    tilemap.SetTile(position, tile);
                }
            }

            FindIslands();

            if (islands.Count <= 4)
            {
                throw new("слишком мало островов");
            }


            islands = islands.OrderByDescending(x => x.Count).Take(5).ToList();

            // ¬ыводим количество найденных островов
            Debug.LogError("Found " + islands.Count + " islands.\n--------------------");

            foreach (var island in islands)
            {
                Debug.LogError("island: " + island.Count);

            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"√енераци€ не удалась по причине {ex.Message}, пробуем еще...");
            GenerateLevel(offset = new Vector2(Random.value * 100, Random.value * 100));
            GenerateBase();

        }


    }

    private void OnDrawGizmosSelected()
    {
        List<Color> color = new List<Color>()
        {
            Color.red,
            Color.magenta,
            Color.blue,
            Color.green,
            Color.white,
            Color.cyan,
            Color.yellow,
        };


        int j = 0;
        foreach (var island in islands)
        {
            Gizmos.color = color[j];
            j++;
            for (int i = 0; i < island.Count; i++)
            {
                //Gizmos.color = new Color(Random.value, Random.value, Random.value, 1f);
                //Gizmos.DrawWireSphere(new Vector3(island[i].x + 0.5f, island[i].y + 0.5f, 0) , .5f);
                Gizmos.DrawWireCube(new Vector3(island[i].x + 0.5f, island[i].y + 0.5f, 0), Vector3.one);
            }
        }
    }
    public void FindIslands()
    {
        // —начала очищаем список островов
        islands.Clear();

        // ѕроходим по всем клеткам Tilemap
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            // ≈сли это высоко, начинаем поиск острова
            if (tilemap.GetTile(position) == lowTile)
            {
                tilemap.SetTile(position, waterTile);
            }
            if (tilemap.GetTile(position) == highTile)
            {
                List<Vector3Int> island = new List<Vector3Int>();
                SearchIsland(position, island);
                if (island.Count >= islandRadius) islands.Add(island);
            }
        }




    }

    private void SearchIsland(Vector3Int position, List<Vector3Int> island)
    {
        Stack<Vector3Int> stack = new Stack<Vector3Int>();
        stack.Push(position);

        while (stack.Count > 0)
        {
            Vector3Int currentPos = stack.Pop();

            if (tilemap.GetTile(currentPos) == highTile)
            {
                island.Add(currentPos);
                tilemap.SetTile(currentPos, groundTile[(int)Random.Range(0, groundTile.Length - 2)]);

                stack.Push(currentPos + new Vector3Int(1, 0, 0)); // вправо
                stack.Push(currentPos + new Vector3Int(-1, 0, 0)); // влево
                stack.Push(currentPos + new Vector3Int(0, 1, 0)); // вверх
                stack.Push(currentPos + new Vector3Int(0, -1, 0)); // вниз
            }

            if (tilemap.GetTile(currentPos) == waterTile || tilemap.GetTile(currentPos) == lowTile)
            {
                tilemap.SetTile(currentPos, waterTile);
            }

            if (groundTile.Contains(tilemap.GetTile(currentPos)) && (tilemap.GetTile(currentPos + Vector3Int.down) == waterTile || tilemap.GetTile(currentPos + Vector3Int.down) == null))
            {
                tilemap.SetTile(currentPos, lineTile);
            }
        }
    }


    private void GenerateBase()
    {
        Vector3 bonfirePosition = Vector3.zero;
        Vector3 bonfirePositionCellCorrect = Vector3.zero;

        Vector3 spawnerPosition = Vector3.zero;
        Vector3 spawnerPositionCellCorrect = Vector3.zero;

        Vector3 upgraderPosition = Vector3.zero;
        Vector3 upgraderPositionCellCorrect = Vector3.zero;

        
        int rank = 4;
        int maxCountResource = 300;
        int maxCanSpwan = 200;

        if (bonfires.Count > 0)
        {
            foreach (var bonfire in bonfires)
            {
                Destroy(bonfire);
            }
        }
        if (spawners.Count > 0)
        {
            foreach (var spawner in spawners)
            {
                Destroy(spawner);
            }
        }
        if (rocksObj.Count > 0)
        {
            foreach (var rocks in rocksObj)
            {
                Destroy(rocks);
            }
        }

        if (upgraderObj != null)
        {
            Destroy(upgraderObj);
        }


        foreach (var island in islands)
        {
            foreach (var cell in island)
            {
                if (groundTile.Contains(tilemap.GetTile(cell)))
                {
                    bonfirePosition = cell;
                    bonfirePositionCellCorrect = new Vector3(bonfirePosition.x + 0.5f, bonfirePosition.y + 0.5f, 0);
                    GameObject bonObj = (Instantiate(bonfire, bonfirePositionCellCorrect, Quaternion.identity));

                    bonObj.GetComponent<Bonfire>();

                    Debug.Log(bonObj.transform.position);
                    bonfires.Add(bonObj);

                    break;
                }
            }
            
            foreach (var cell in island)
            {
                if (groundTile.Contains(tilemap.GetTile(cell)) && Vector3.Distance(bonfirePositionCellCorrect, cell) >= island.Count / 80)
                {
                    spawnerPosition = cell;
                    spawnerPositionCellCorrect = new Vector3(spawnerPosition.x + 0.5f, spawnerPosition.y + 0.5f, 0);

                    GameObject spawnerObj = (Instantiate(spawner, spawnerPositionCellCorrect, Quaternion.identity));

                    //Transform[] point = spawnerObj.GetComponent<Spawner>().point;
                    //point[0].position = island[island.Count - 1];
                    //point[1].position = island[0];
                    //spawnerObj.GetComponent<Spawner>().point = point;
                    
                    spawnerObj.GetComponent<Spawner>().rank = rank;
                    rank--;

                    spawnerObj.GetComponent<Spawner>().maxCountResource = maxCountResource;
                    maxCountResource -= 50;

                    spawnerObj.GetComponent<Spawner>().maxCanSpwan = maxCanSpwan;
                    maxCanSpwan -= 50;



                    

                    spawnerObj.GetComponent<Spawner>().island = island;
                    spawners.Add(spawnerObj);
                    break;
                }
            }
            foreach (var cell in island)
            {
                if (groundTile.Contains(tilemap.GetTile(cell)) && Random.Range(1, 500) == 1)
                {

                    Vector3 rockPosition = new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0);

                    rocksObj.Add(Instantiate(rocks[Random.Range(0, rocks.Count-1)], rockPosition, Quaternion.identity));
                    
                }
            }
            
        }

        foreach (var cell in islands[islands.Count-1])
        {
            if (groundTile.Contains(tilemap.GetTile(cell)) && Vector3.Distance(bonfirePositionCellCorrect, cell) >= 30)
            {
                upgraderPosition = cell;
                upgraderPositionCellCorrect = new Vector3(upgraderPosition.x + 0.5f, upgraderPosition.y + 0.5f, 0);
                upgraderObj = Instantiate(upgrader, upgraderPositionCellCorrect, Quaternion.identity);

                break;
            }
        }

        bonfires.Reverse();
        spawners.Reverse();

        Destroy(spawners.First());
        GameObject.FindGameObjectWithTag("Player").transform.position = bonfirePositionCellCorrect;
    }

}
