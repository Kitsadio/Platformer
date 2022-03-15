using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using UnityEngine;


public class Chunks
{
    public Chunk chunk;
    public Vector2Int chunkPos;
}


public class GenerateWorld : MonoBehaviour
{
    public int WorldSize;
    public Chunk chunkPrefab;
    public int index;
    private Vector3 pos;
    private Vector2 chunkEndPos;
    Vector2 seed;
    Chunks tempChunk;

    private Chunks c;
    public List<Chunks> chunkList;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        seed.x = Random.Range(0.0f, 100000.0f);
        seed.y = Random.Range(0.0f, 100000.0f);
    }

    public void SetupWorld()
    {
        chunkList = new List<Chunks>();
        chunkList.Add(new Chunks());
        chunkList[0].chunk =  Instantiate(chunkPrefab, new Vector3(-150, -100, 0), Quaternion.identity);
        chunkList[0].chunkPos.x = -1;
        chunkList[0].chunkPos.y = 0;

        chunkPrefab.GenerateChunk(seed, chunkList[0].chunkPos);

        chunkList.Add(new Chunks());
        chunkList[1].chunk = Instantiate(chunkPrefab, new Vector3(-50, -100, 0), Quaternion.identity);
        chunkList[1].chunkPos.x = 0;
        chunkList[1].chunkPos.y = 0;

        chunkPrefab.GenerateChunk(seed, chunkList[1].chunkPos);

        chunkList.Add(new Chunks());
        chunkList[2].chunk = Instantiate(chunkPrefab, new Vector3(50, -100, 0), Quaternion.identity);
        chunkList[2].chunkPos.x = 1;
        chunkList[2].chunkPos.y = 0;

        chunkPrefab.GenerateChunk(seed, chunkList[2].chunkPos);
    }

    private bool IsChunkFree(bool isLeft)
    {
        if (index == 0)
            return true;

        int chunkX;

        if(isLeft)
        {
            chunkX = chunkList[0].chunkPos.x + 1;
        }
        else
        {
            chunkX = chunkList[2].chunkPos.x - 1;
        }

        for (int i = 0; i < 1; ++i)
        {
            if (chunkList[i].chunkPos.x == chunkX && chunkList[i].chunkPos.y == chunkList[0].chunkPos.y)
            {
                return false;
            }
        }

        return true;
    }

    private void spawnChunk(Vector3 position, bool down, bool left)
    {
        bool isChunkFree = IsChunkFree(left);

        if (isChunkFree)
        {
            if (down == false && left == false)
            {
                Destroy(chunkList[0].chunk.gameObject);
                chunkList[0] = chunkList[1];
                chunkList[1] = chunkList[2];
                chunkList.RemoveAt(2);
                chunkList.Add(new Chunks());
                chunkList[2].chunk = Instantiate(chunkPrefab, position, Quaternion.identity);
                chunkList[2].chunkPos.x = chunkList[1].chunkPos.x + 1;
                chunkList[2].chunkPos.y = chunkList[1].chunkPos.y;
                chunkPrefab.GenerateChunk(seed, chunkList[2].chunkPos);
            }
            else if (down == false && left == true)
            {
                Destroy(chunkList[2].chunk.gameObject);
                chunkList.RemoveAt(2);
                chunkList.Insert(0, new Chunks());
                chunkList[0].chunk = Instantiate(chunkPrefab, position, Quaternion.identity);
                chunkList[0].chunkPos.x = chunkList[1].chunkPos.x - 1;
                chunkList[0].chunkPos.y = chunkList[1].chunkPos.y;
                chunkPrefab.GenerateChunk(seed, chunkList[0].chunkPos);
            }
        }
    }

    private void Update()
    {
        Vector2 heroPos = GameObject.Find("hero").transform.position;

        if (heroPos.x > (chunkList[2].chunk.transform.position.x + (chunkList[2].chunk.area.size.x /5)))
        {
            pos = new Vector3(chunkList[2].chunk.transform.position.x + chunkPrefab.area.size.x, chunkList[2].chunk.transform.position.y, 0);
            spawnChunk(pos , false, false);
        } 
        else if (heroPos.x < (chunkList[0].chunk.transform.position.x + (chunkList[0].chunk.area.size.x - (chunkList[0].chunk.area.size.x / 5))))
        {
            pos = new Vector3(chunkList[0].chunk.transform.position.x - chunkPrefab.area.size.x, chunkList[0].chunk.transform.position.y, 0);
            spawnChunk(pos, false, true);
        }
    }
}
