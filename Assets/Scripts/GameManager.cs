using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileBoard tileBoard;
    
    public static GameManager Instance { get; private set; }
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestText;

    private int score;
    
    public InputManager inputManager;

    private void Awake()
    {
        Instance = this;
        
        inputManager = new InputManager();
        inputManager.Enable();
    }

    private void Start()
    {
       NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        bestText.text = PlayerPrefs.GetInt("hiscore", LoadHiscore()).ToString();
        
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        
        tileBoard.ClearBoard();
        tileBoard.CreateTile();
        tileBoard.CreateTile();
        tileBoard.enabled = true;
    }

    public void GameOver()
    {
        canvasGroup.interactable = true;
        tileBoard.enabled = false;
        StartCoroutine(Fade(canvasGroup, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = .5f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void IncreaseScore(int point)
    {
        score += point;
        SetScore(score);
    }
    
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        
        SaveHiscore();
    }

    private void SaveHiscore()
    {
        if (score > LoadHiscore())
            PlayerPrefs.SetInt("hiscore",score);
    }

    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
}