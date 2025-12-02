using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Fail tracking")]
    public int totalFails = 0;
    public int failThreshold = 3;

    [Header("Demon")]
    public CandyDemonAI demonAI; // assign in inspector (optional)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Call this when the player fails a recipe (burned, wrong, or otherwise failed).
    /// </summary>
    public void PlayerFailedRecipe()
    {
        totalFails++;
        Debug.Log($"GameManager: player fail #{totalFails}");
        demonAI ??= FindObjectOfType<CandyDemonAI>();
        demonAI?.OnPlayerFail(totalFails);

        // Optionally: raise events, update UI, etc.
    }

    /// <summary>
    /// Call this when player completes a recipe successfully (optional).
    /// </summary>
    public void PlayerCompletedRecipe()
    {
        Debug.Log("GameManager: player completed recipe");
        // Reset fails or partially reduce count if you want
        // totalFails = Mathf.Max(0, totalFails - 1);
    }

    /// <summary>
    /// Hard reset of fail counter (for new rounds / scenes)
    /// </summary>
    public void ResetFails()
    {
        totalFails = 0;
    }
}
