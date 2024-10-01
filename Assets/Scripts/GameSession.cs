using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerMaxLives = 3;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull; 
    [SerializeField] private Sprite heartEmpty;
    [SerializeField] private int totalKeys = 3;
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private GameObject gameOverCanvas;

    private int currentKeys = 0;
    private int playerLives;
    private HashSet<string> collectedKeys = new HashSet<string>();
    
    public static GameSession instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
        Time.timeScale = 1f;
    }
    private void Start()
    {    
        playerLives = playerMaxLives;
        UpdateHeartUI();
        UpdateKeysUI();
    }
    private void UpdateHeartUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerLives)
            {
                hearts[i].sprite = heartFull; 
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }
    public void ProcessPlayerHit()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            GameOver();
        }
    }
    private void TakeLife()
    {
        playerLives--;
        UpdateHeartUI();
    }
    public void ResetGameSession()
    {
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }
    public void AddHearts()
    {
        if (playerLives < playerMaxLives)
        {
            playerLives++;
        }
        UpdateHeartUI();
    }
    public void CollectKeys(string keyID)
    {
        if (!collectedKeys.Contains(keyID))
        {
            collectedKeys.Add(keyID);
            currentKeys++;
            UpdateKeysUI();
        }
    }
    private void UpdateKeysUI()
    {
        keysText.text = "собрано " + currentKeys.ToString() + "/" + totalKeys + " ключей";
    }
    public bool IsKeyCollected(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }
    public bool AreAllKeysCollected()
    {
        return currentKeys >= totalKeys;
    }
    public void ResetKeyCount(int newTotalKeys)
    {
        currentKeys = 0;
        totalKeys = newTotalKeys;
        keysText.text = "собрано " + currentKeys.ToString() + "/" + newTotalKeys + " ключей";
    }
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
