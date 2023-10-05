using System.Collections;
using System.Collections.Generic;
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
            this.cell.tile = null;

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
            this.cell.tile = null;

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position));
    }

    private IEnumerator Animate(Vector3 to)
    {
        float elapsed = 0f;
        float duration = .1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        // transform.position = to;
    }
}
