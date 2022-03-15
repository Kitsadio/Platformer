using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;


public class Chunk : MonoBehaviour
{
    public float threshold;
    public BoundsInt area;
    public TileBase[] tiles;
    private Tilemap map;

    public void GenerateChunk(Vector2 seed, Vector2Int chunkPos)
    {
        float test = 0;

        map = GetComponent<Tilemap>();

        area.size = new Vector3Int(100, 100, 1);

        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        for (int index = 0; index < tileArray.Length; index++)
        {
            test = Mathf.PerlinNoise(seed.x + (chunkPos.x * 100) + (index % area.size.x) * 0.1f, seed.y+ (chunkPos.y * 100) + (index / area.size.y) * 0.1f);
            if (test > threshold)
            {
                tileArray[index] = tiles[0];
            }
            else
            {
                tileArray[index] = null;
            }
        }

        map.SetTilesBlock(area, tileArray);
    }

}
