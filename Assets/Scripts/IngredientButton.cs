using UnityEngine;

public class IngredientButton : MonoBehaviour
{
    public int ingredientIndex;
    public GameManager gameManager;

    public void OnClickAdd()
    {
        if (gameManager != null)
        {
            gameManager.AddIngredient(ingredientIndex);
        }
    }
}