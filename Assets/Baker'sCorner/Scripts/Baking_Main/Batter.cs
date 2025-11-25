using UnityEngine;

public class Batter : MonoBehaviour
{
    private Recipe recipeOutcome;
    [SerializeField] private int totalScoops = 6;  // default (cookie)
    private int scoopsRemaining;

    private void Awake()
    {
        scoopsRemaining = totalScoops;
    }

    public void SetRecipeOutcome(Recipe recipe)
    {
        recipeOutcome = recipe;

        // Optional: set totalScoops based on recipe type
        if (recipe.recipeName.ToLower().Contains("cake"))
            totalScoops = 5;
        else
            totalScoops = 6;

        scoopsRemaining = totalScoops;
    }

    public Recipe GetRecipeOutcome() => recipeOutcome;

    public void RemoveScoop()
    {
        scoopsRemaining--;
        Debug.Log($"Batter scooped! {scoopsRemaining} scoops left.");

        if (scoopsRemaining <= 0)
        {
            Debug.Log("Batter bowl empty!");
            Destroy(gameObject);
        }
    }
}
