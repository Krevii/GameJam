using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActive = false;
    public GameObject[] resource;
    public List<Vector3Int> island = new List<Vector3Int>();
    public Transform[] point = new Transform[2];
    public int rank = 0;
    public int maxCountResource;
    public int howMuchMoreCanSpawn;
    public int maxCanSpwan;
    public float delayMax;

    private List<GameObject> bonfires = new List<GameObject>();
    private float _delay;
    private GameObject player;
    private bool isTaked = true;
    public List<GameObject> countResource = new List<GameObject>();
    public int teleportPoint = 0;
    private List<GameObject> objectsToRemove = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        howMuchMoreCanSpawn = maxCanSpwan;
        bonfires = FindObjectOfType<PerlinNoiseGenerator>().bonfires;

    }

    // Update is called once per frame
    void Update()
    {
        _delay -= Time.deltaTime;
        if (isActive)
        {
            SpawnObj();
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 5f)
        {
            transform.localScale = Vector3.one * 1.2f;
            if (Input.GetKeyDown(KeyCode.E) && !isActive)
            {
                isActive = true;
            }
        }
        else
        {
            transform.localScale = Vector3.one;
        }
        if (Input.GetKeyDown(KeyCode.H) && countResource.Count <= 0 && Vector3.Distance(player.transform.position, transform.position) < 10f)
        {
            player.transform.position = bonfires[0].transform.position;

        }
        UpdateResources();
    }
    private void UpdateResources()
    {
        

        // Проходим по каждому объекту в листе countResource
        foreach (var item in countResource)
        {
            // Если объект умер, то добавляем его в список на удаление
            if (item.gameObject.GetComponent<Enemy>().isDead)
            {
                objectsToRemove.Add(item.gameObject);
            }
        }

        // Удаляем объекты из листа countResource
        foreach (var obj in objectsToRemove)
        {
            countResource.Remove(obj);
        }
        if (countResource.Count <= 0 && howMuchMoreCanSpawn <= 0)
        {
            if (isTaked)
            {

                teleportPoint++;
                player.GetComponent<Player>().teleportPoint += teleportPoint;
                player.GetComponent<Player>().hitPoint = player.GetComponent<Player>().maxhitPoint;

                howMuchMoreCanSpawn = maxCanSpwan;
                isTaked = false;
            }
        }

        objectsToRemove.Clear();
    }
    public void SpawnObj()
    {

        if (_delay <= 0 && countResource.Count <= maxCountResource && howMuchMoreCanSpawn >= 1)
        {
            var position = new Vector3(Random.Range(point[0].position.x, point[1].position.x), Random.Range(point[0].position.y, point[1].position.y), 0);
            RaycastHit2D hit = Physics2D.Raycast(position, position);
            if (hit.collider != null)
            {
                //return;
            }

            switch (rank)
            {
                case 1:
                    countResource.Add(Instantiate(resource[0], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[1], position, Quaternion.identity));
                    break;
                case 2:
                    countResource.Add(Instantiate(resource[2], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[3], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[4], position, Quaternion.identity));
                    break;
                case 3:
                    countResource.Add(Instantiate(resource[5], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[6], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[7], position, Quaternion.identity));
                    break;
                case 4:
                    countResource.Add(Instantiate(resource[8], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[9], position, Quaternion.identity));
                    countResource.Add(Instantiate(resource[10], position, Quaternion.identity));
                    
                    break;

            }
            _delay = delayMax;
            howMuchMoreCanSpawn--;
            if (howMuchMoreCanSpawn <= 0)
            {
                isActive = false;
                //howMuchMoreCanSpawn = maxCanSpwan;
                
                
            }
            Debug.Log(countResource.Count + $" Реурсов у {gameObject.name}");
        }

    }
}
