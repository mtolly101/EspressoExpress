using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public Text scoreText;
    public Text timerText;
    public Text orderText;
    public Text cupText;
    public GameObject gameOverPanel;

    [Header("Game Settings")]
    public float startTime = 60f;
    public int scorePerCorrectOrder = 1;

    private float timeLeft;
    private int score;
    private List<Ingredient> currentOrder = new List<Ingredient>();
    private List<Ingredient> cup = new List<Ingredient>();

    private void Start()
    {
        timeLeft = startTime;
        score = 0;
        gameOverPanel.SetActive(false);

        // For Milestone 1, use one static recipe:
        currentOrder = new List<Ingredient> { Ingredient.Espresso, Ingredient.Milk }; // "Latte"
        orderText.text = "Order: Latte (Espresso + Milk)";

        UpdateUI();
        UpdateCupText();
    }

    private void Update()
    {
        if (gameOverPanel.activeSelf) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            GameOver();
        }

        UpdateUI();
    }

    public void AddIngredient(int ingredientIndex)
    {
        if (gameOverPanel.activeSelf) return;

        Ingredient ing = (Ingredient)ingredientIndex;
        cup.Add(ing);
        UpdateCupText();
    }

    public void Serve()
    {
        if (gameOverPanel.activeSelf) return;

        // Simple check: exact match
        if (cup.Count == currentOrder.Count)
        {
            bool match = true;
            for (int i = 0; i < cup.Count; i++)
            {
                if (cup[i] != currentOrder[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                score += scorePerCorrectOrder;
                // Later: random new order. For now, keep same order.
            }
        }

        // Clear cup either way
        cup.Clear();
        UpdateCupText();
        UpdateUI();
    }

    public void Restart()
    {
        timeLeft = startTime;
        score = 0;
        cup.Clear();
        UpdateCupText();
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    private void UpdateCupText()
    {
        if (cup.Count == 0)
        {
            cupText.text = "Cup: (empty)";
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Cup: ");
        for (int i = 0; i < cup.Count; i++)
        {
            sb.Append(cup[i].ToString());
            if (i < cup.Count - 1) sb.Append(" + ");
        }
        cupText.text = sb.ToString();
    }
}