using System.Collections;
using UnityEngine;

public class CandyDemonAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameManager gameManager;

    [Header("Teleport Settings")]
    public Transform[] teleportPoints;
    public float minWait = 2f;
    public float maxWait = 5f;
    public float appearChance = 0.3f;

    [Header("Jump Scare")]
    public GameObject jumpScarePrefab;
    public float failThreshold = 3;

    [Header("Audio")]
    public AudioClip subtleSound;
    public AudioClip appearSound;
    public AudioSource audioSource;

    private bool jumpScareTriggered = false;
    private Renderer[] demonRenderers;

    private void Start()
    {
        demonRenderers = GetComponentsInChildren<Renderer>();
        SetVisible(false);

        if (gameManager == null)
            gameManager = GameManager.Instance;

        StartCoroutine(TeleportRoutine());
    }

    // =============================
    //  FAIL EVENT FROM GAME MANAGER
    // =============================
    public void OnPlayerFail(int totalFails)
    {
        Debug.Log($"CandyDemonAI: Player fail count = {totalFails}");

        if (totalFails >= failThreshold)
        {
            TriggerJumpScare();
        }
    }

    // =============================
    //  MAIN TELEPORT LOOP
    // =============================
    private IEnumerator TeleportRoutine()
    {
        while (!jumpScareTriggered)
        {
            if (teleportPoints.Length == 0)
                yield break;

            Transform target = teleportPoints[Random.Range(0, teleportPoints.Length)];
            transform.position = target.position;
            transform.rotation = target.rotation;

            bool willAppear = Random.value < appearChance;
            SetVisible(willAppear);

            // Audio
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = willAppear ? appearSound : subtleSound;
                audioSource.Play();
            }

            // Make demon look at player ONLY horizontally
            if (willAppear && player != null)
            {
                Vector3 lookTarget = new Vector3(
                    player.position.x,
                    transform.position.y,
                    player.position.z
                );
                transform.LookAt(lookTarget);
            }

            Debug.Log($"CandyDemonAI: Teleported to {target.name} | Visible: {willAppear}");

            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }

    // =============================
    //  JUMP SCARE
    // =============================
    private void TriggerJumpScare()
    {
        if (jumpScareTriggered) return;

        jumpScareTriggered = true;
        Debug.Log("CandyDemonAI: Jump Scare Triggered!");

        // Position demon directly in front of the player
        if (player != null)
        {
            transform.position =
                player.position + player.forward * 0.6f + Vector3.up * 0.2f;

            transform.LookAt(player);
            SetVisible(true);
        }

        // Jump scare prefab spawn
        if (jumpScarePrefab != null && player != null)
        {
            Vector3 spawnPos = player.position + player.forward * 0.5f + Vector3.up * 0.2f;
            Quaternion rot = Quaternion.LookRotation(player.forward);
            Instantiate(jumpScarePrefab, spawnPos, rot);
        }
    }

    // =============================
    //  VISIBILITY HELPERS
    // =============================
    private void SetVisible(bool visible)
    {
        if (demonRenderers == null) return;

        foreach (Renderer r in demonRenderers)
        {
            if (r != null)
                r.enabled = visible;
        }
    }

    // Called by projectiles or weapons when hitting the demon
    // Called by projectiles or weapons when hitting the demon
    public void OnHit()
    {
        if (jumpScareTriggered) return; // disable hits after scare

        Debug.Log("CandyDemonAI HIT! Vanishing...");

        // Hide demon
        SetVisible(false);

        // Hit sound
        if (audioSource != null && appearSound != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(appearSound);
        }

        // Stop teleporting temporarily
        StopAllCoroutines();

        // Start respawn cycle
        StartCoroutine(HitRespawnRoutine());
    }

    private IEnumerator HitRespawnRoutine()
    {
        float respawnDelay = Random.Range(2f, 4f);
        yield return new WaitForSeconds(respawnDelay);

        // Teleport to a new point
        Transform point = teleportPoints[Random.Range(0, teleportPoints.Length)];
        transform.position = point.position;
        transform.rotation = point.rotation;

        Debug.Log($"CandyDemonAI respawned at {point.name} after being hit.");

        // Stay invisible until the teleport loop chooses to show it again
        SetVisible(false);

        // Resume teleport routine
        StartCoroutine(TeleportRoutine());
    }


}
