using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   }

   private void Start()
   {
      CreateTile();   
      CreateTile();   
   }

   private void CreateTile()
   {
      Tile tile = Instantiate(tilePrefab, tileGrid.transform);
      tile.SetTileState(tileStateList[0],2);
      tile.Spawn(tileGrid.GetRandomEmptyCell());
   }
   
}
