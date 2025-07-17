using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public float health = 100f;
    public TextMeshProUGUI healthText;
    public GameManager gameManager;
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Hit(float damage)
    {
        health -= damage;
        healthText.text = $"{health} HP";
        if (health <= 0)
        {
            gameManager.GameOver();
            //Debug.Log("Game Over");
            //SceneManager.LoadScene(0);
        }
    }
}

