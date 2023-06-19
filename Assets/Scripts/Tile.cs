using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileState tileState { get; private set; }
    public TileCell cell { get; private set; }
    public int number { get; set; }
}
