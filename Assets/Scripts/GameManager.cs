using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text timerText;
    public Text orderText;
    public Text cupText;
    public Text livesText;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public float startTime = 60f;
    public float maxExtraTime = 30f;
    public int maxLives = 3;
    public int targetScore = 10;
    public List<OrderDef> menu = new List<OrderDef>();
    public Color normalCupColor = Color.black;
    public Color successCupColor = Color.green;
    public Color failCupColor = Color.red;
    public RectTransform cupPanelTransform;
    public AudioSource sfxSource;
    public AudioClip buttonClickClip;
    public AudioClip serveSuccessClip;
    public AudioClip serveFailClip;
    private float timeLeft;
    private int score;
    private int lives;
    private bool gameEnded = false;
    private OrderDef currentOrder;
    private readonly List<Ingredient> cup = new List<Ingredient>();

    private void Start()
    {
        timeLeft = startTime;
        score = 0;
        lives = maxLives;

        if (cupText != null)
        {
            cupText.color = normalCupColor;
        }

        gameOverPanel.SetActive(false);
        PickRandomOrder();
        UpdateAllUI();
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

        UpdateTimerUI();
    }

    public void AddIngredient(int ingredientIndex)
    {
        if (gameOverPanel.activeSelf) return;

        Ingredient ing = (Ingredient)ingredientIndex;
        cup.Add(ing);

        if (sfxSource != null && buttonClickClip != null)
        {
            sfxSource.PlayOneShot(buttonClickClip);
        }

        UpdateCupText();
    }

    public void Serve()
    {
        if (gameOverPanel.activeSelf) return;

        bool match = IsCupMatchingOrder();

        if (match)
        {
            // score and time
            score += currentOrder != null ? currentOrder.scoreValue : 1;

            float bonus = currentOrder != null ? currentOrder.timeBonus : 5f;
            timeLeft = Mathf.Min(timeLeft + bonus, startTime + maxExtraTime);

            if (score >= targetScore)
            {
                gameEnded = true;
                gameOverPanel.SetActive(true);

                if (gameOverText != null)
                    gameOverText.text = "Shift Complete! You Win!";

                cup.Clear();
                UpdateCupText();
                UpdateAllUI();
                return;
            }

            StartCoroutine(FlashCup(successCupColor, 1.1f));

            if (sfxSource != null && serveSuccessClip != null)
            {
                sfxSource.PlayOneShot(serveSuccessClip);
            }

            PickRandomOrder();
        }
        else
        {
            // minus lives
            lives--;
            StartCoroutine(FlashCup(failCupColor, 1.1f));

            if (sfxSource != null && serveFailClip != null)
            {
                sfxSource.PlayOneShot(serveFailClip);
            }

            if (lives <= 0)
            {
                GameOver();
            }
        }

        cup.Clear();
        UpdateCupText();
        UpdateAllUI();
    }

    public void Restart()
    {
        timeLeft = startTime;
        score = 0;
        lives = maxLives;
        cup.Clear();
        if (cupText != null)
        {
            cupText.color = normalCupColor;
        }

        gameOverPanel.SetActive(false);
        PickRandomOrder();
        UpdateAllUI();
    }

    private void GameOver()
    {
        gameEnded = true;
        gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "Game Over :(";
    }

    private void PickRandomOrder()
    {
        if (menu == null || menu.Count == 0)
        {
            currentOrder = null;
            if (orderText != null)
            {
                orderText.text = "Order: (no recipes)";
            }
            return;
        }

        int index = Random.Range(0, menu.Count);
        currentOrder = menu[index];

        if (orderText != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Order: ");
            sb.Append(currentOrder.orderName);
            sb.Append(" (");

            for (int i = 0; i < currentOrder.ingredients.Count; i++)
            {
                sb.Append(currentOrder.ingredients[i].ToString());
                if (i < currentOrder.ingredients.Count - 1)
                    sb.Append(" + ");
            }
            sb.Append(")");
            orderText.text = sb.ToString();
        }
    }

    private bool IsCupMatchingOrder()
    {
        if (currentOrder == null) return false;

        var recipe = currentOrder.ingredients;
        if (cup.Count != recipe.Count) return false;

        for (int i = 0; i < cup.Count; i++)
        {
            if (cup[i] != recipe[i]) return false;
        }

        return true;
    }

    private void UpdateAllUI()
    {
        UpdateScoreUI();
        UpdateTimerUI();
        UpdateLivesUI();
        UpdateCupText();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + Mathf.Max(lives, 0);
        }
    }

    private void UpdateCupText()
    {
        if (cupText == null) return;

        if (cup.Count == 0)
        {
            cupText.text = "Cup: (empty)";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("Cup: ");

        for (int i = 0; i < cup.Count; i++)
        {
            sb.Append(cup[i].ToString());
            if (i < cup.Count - 1)
                sb.Append(" + ");
        }

        cupText.text = sb.ToString();
    }

    // cup flashes when right or wrong
    private System.Collections.IEnumerator FlashCup(Color targetColor, float scaleMultiplier)
    {
        if (cupText == null)
            yield break;

        Color originalColor = cupText.color;
        Vector3 originalScale = cupPanelTransform != null ? cupPanelTransform.localScale : Vector3.one;

        float upDuration = 0.1f;
        float downDuration = 0.1f;

        cupText.color = targetColor;

        float t = 0f;
        while (t < upDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(1f, scaleMultiplier, t / upDuration);
            if (cupPanelTransform != null)
            {
                cupPanelTransform.localScale = new Vector3(s, s, 1f);
            }
            yield return null;
        }

        t = 0f;
        while (t < downDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(scaleMultiplier, 1f, t / downDuration);
            if (cupPanelTransform != null)
            {
                cupPanelTransform.localScale = new Vector3(s, s, 1f);
            }
            yield return null;
        }

        cupText.color = normalCupColor;
        if (cupPanelTransform != null)
        {
            cupPanelTransform.localScale = originalScale;
        }
    }
}