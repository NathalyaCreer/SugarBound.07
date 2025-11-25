using UnityEngine;

public class OvenTimer : MonoBehaviour
{
    [Header("Bake Time Settings")]
    public int maxBakeTime = 30;
    public int increment = 5;

    [Header("Runtime")]
    public int currentBakeTime = 0;     // Updates when pressing AddTime
    public bool ovenRunning = false;

    private float bakeTimer = 0f;

    void Update()
    {
        // Run the countdown when the oven is active
        if (ovenRunning)
        {
            bakeTimer -= Time.deltaTime;

            if (bakeTimer <= 0f)
            {
                ovenRunning = false;
                Debug.Log("Oven finished baking!");
                // Trigger your finished-baking callback here
            }
        }
    }

    // Called by the Add Time button
    public void AddTime()
    {
        if (ovenRunning) return;  // Can't change time while baking

        currentBakeTime += increment;

        if (currentBakeTime > maxBakeTime)
            currentBakeTime = 0; // Reset when passing max

        Debug.Log("AddTime pressed → Bake time now: " + currentBakeTime + " seconds");
    }

    // Called by the Start button
    public void StartOven()
    {
        if (ovenRunning) return;
        if (currentBakeTime <= 0) return;

        bakeTimer = currentBakeTime;
        ovenRunning = true;

        Debug.Log("Oven started for " + currentBakeTime + " seconds");
    }
}
