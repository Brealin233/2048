using UnityEngine;

public class TileGrid : MonoBehaviour
{
    private TileRow[] rows { get; set; }
    private TileCell[] cells { get; set; }

    private int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                rows[i].cells[j].coordinates = new Vector2Int(j, i);
            }
        }
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, size);
        int startIndex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
                index = 0;

            if (index == startIndex)
            {
                return null;
            }
        }
       
        return cells[index];
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            return rows[y].cells[x];
        } 
        
        return null;
    }

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }
    
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }
}

