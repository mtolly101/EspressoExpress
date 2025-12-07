using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Coffee/Order", fileName="NewOrder")]
public class OrderDef : ScriptableObject
{
    public string orderName;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public int scoreValue = 1;
    public float timeBonus = 5f;
}