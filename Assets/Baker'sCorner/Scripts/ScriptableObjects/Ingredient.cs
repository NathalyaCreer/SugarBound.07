using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Cooking/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public GameObject sourcePrefab; // prefab to spawn
}
