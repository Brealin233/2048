using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState tileState { get; private set; }
    public TileCell cell { get; private set; }
    public int number { get; set; }

    private Image image;
    private TextMeshProUGUI text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTileState(TileState tileState,int number)
    {
        this.tileState = tileState;
        this.number = number;

        image.color = tileState.backgroundColor;
        text.color = tileState.textColor;
        text.text = number.ToString();
    }

    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
            this.cell = null;

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }
}
