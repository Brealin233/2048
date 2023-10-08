using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileBoard : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<TileState> tileStateList;

    private TileGrid tileGrid;
    private List<Tile> tileLists;
    private int tileSize = 16;

    private void Awake()
    {
        tileGrid = GetComponentInChildren<TileGrid>();
        tileLists = new List<Tile>(tileSize);
        
        GameManager.Instance.inputManager.Move.Up.performed += MoveUpMethod;
        GameManager.Instance.inputManager.Move.Down.performed += MoveDownMethod;
        GameManager.Instance.inputManager.Move.Left.performed += MoveLeftMethod;
        GameManager.Instance.inputManager.Move.Right.performed += MoveRightMethod;
    }
    

    private void MoveRightMethod(InputAction.CallbackContext obj)
    {
        MoveTiles(Vector2Int.right, tileGrid.width - 2, -1, 0, 1);
    }

    private void MoveLeftMethod(InputAction.CallbackContext obj)
    {
        MoveTiles(Vector2Int.left, 1, 1, 0, 1);
    }

    private void MoveDownMethod(InputAction.CallbackContext obj)
    {
        MoveTiles(Vector2Int.down, 0, 1, tileGrid.height - 2, -1);
    }

    private void MoveUpMethod(InputAction.CallbackContext obj)
    {
        MoveTiles(Vector2Int.up, 0, 1, 1, 1);
    }
    
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, tileGrid.transform);
        tile.SetTileState(tileStateList[0], 2);
        tile.Spawn(tileGrid.GetRandomEmptyCell());
        tileLists.Add(tile);
    }

    public void ClearBoard()
    {
        //tileGrid = null;
        
        foreach (var tile in tileLists)
            Destroy(tile.gameObject);
        
        tileLists.Clear();
    }

    /// <summary>
    /// 找到方格内的数字
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="startX"></param>
    /// <param name="incrementX"></param>
    /// <param name="startY"></param>
    /// <param name="incrementY"></param>
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        
        for (int x = startX; x >= 0 && x < tileGrid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < tileGrid.height; y += incrementY)
            {
                TileCell cell = tileGrid.GetCell(x, y);
                
                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
            StartCoroutine(WaitForChanges());
        
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = tileGrid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile,adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
                break;
            }
            
            newCell = adjacent;
            adjacent = tileGrid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;

            //! 坐标轴反向了
            // print($"{newCell.coordinates.x}+{newCell.coordinates.y}"); 
        }

        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private void Merge(Tile a, Tile b)
    {
        tileLists.Remove(a);
        
        a.Merge(b.cell);
        
        var index = Mathf.Clamp(IndexOf(b.tileState) + 1, 0, tileLists.Count);
        var number = b.number * 2;
        
        b.SetTileState(tileStateList[index],number);
        GameManager.Instance.IncreaseScore(number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStateList.Count; i++)
        {
            if (state == tileStateList[i])
                return i;
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        GameManager.Instance.inputManager.Disable();
        yield return new WaitForSeconds(.1f);
        GameManager.Instance.inputManager.Enable();
        
        foreach (var tile in tileLists)
            tile.locked = false;
        
        if (tileLists.Count != tileSize)
            CreateTile();

        if (CheckForGameOver())
            GameManager.Instance.GameOver();
        
    }

    private bool CheckForGameOver()
    {
        if (tileLists.Count != tileSize)
            return false;

        foreach (var tile in tileLists)
        {
            TileCell up = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
                return false;
            if (down != null && CanMerge(tile, down.tile))
                return false;
            if (left != null && CanMerge(tile, left.tile))
                return false;
            if (right != null && CanMerge(tile, right.tile))
                return false;
        }

        return true;
    }
}
