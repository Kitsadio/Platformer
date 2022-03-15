using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private Collider2D coll;
    public int miningSize = 4;
    public Chunk chunkPrefab;
    public Transform groundCheck;
    public GenerateWorld gW;
    private Vector3 heroPos;
    private Tilemap map;
    private Grid grid;
    Vector2Int chunkPos = new Vector2Int(0, 0);
    // Start is called before the first frame update
    void Start()
    {
        gW = GameObject.Find("World").GetComponent<GenerateWorld>();
        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    public Tilemap GetMapAtPosition(Vector3 position)
    {
        if(position.x > gW.chunkList[0].chunk.transform.position.x && position.x < gW.chunkList[0].chunk.transform.position.x + chunkPrefab.area.size.x)
        {
            map = gW.chunkList[0].chunk.GetComponent<Tilemap>();
        }
        else if (position.x > gW.chunkList[2].chunk.transform.position.x && position.x < gW.chunkList[2].chunk.transform.position.x + chunkPrefab.area.size.x)
        {
            map = gW.chunkList[2].chunk.GetComponent<Tilemap>();
        }
        else
        {
            map = gW.chunkList[1].chunk.GetComponent<Tilemap>();
        }

        return map;
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            heroPos = GameObject.Find("hero").transform.position;
            Vector3Int d = new Vector3Int();
            Vector3 a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            a.z = 0;
            map = GetMapAtPosition(a);
            d = map.WorldToCell(a);
            d.z = 0;

            if (Vector3.Distance(heroPos, a) <= miningSize)
                map.SetTile(d, null);
        }
    }

    void InitGame()
    {
        gW.SetupWorld();
    }
}
