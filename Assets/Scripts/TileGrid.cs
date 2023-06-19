using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    private TileRow[] rows { get; set; }
    private TileCell[] cells { get; set; }

    private int size => cells.Length;
    private int height => rows.Length;
    private int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rows[x].cells[y].coordinates = new Vector2Int(x, y);
            }
        }
        
        print(cells.Length);
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = UnityEngine.Random.Range(0, size);
        int startIndex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= size)
                index = 0;

            if (index == startIndex)
            {
                return null;
            }
            
        }
       
        print(cells[index].transform.position.x);
        return cells[index];
    }
}
