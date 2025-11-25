using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int totalFails = 0;
    public int failThreshold = 3;

    [Header("Demon")]
    public CandyDemonAI demonAI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Call when the player fails a recipe
    /// </summary>
    public void PlayerFailedRecipe()
    {
        totalFails++;
        Debug.Log($"GameManager: Player failed a recipe. Total fails = {totalFails}");

        if (demonAI != null)
        {
            demonAI.OnPlayerFail(totalFails);
        }
    }

    /// <summary>
    /// Call when the player successfully completes a recipe
    /// </summary>
    public void PlayerCompletedRecipe()
    {
        Debug.Log("GameManager: Player successfully completed a recipe!");
    }
}
