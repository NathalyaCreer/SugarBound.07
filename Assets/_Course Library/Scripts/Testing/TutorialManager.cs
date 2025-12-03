using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI Panels (in order of steps)")]
    public GameObject[] tutorialPopups;

    private int currentStep = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowStep(0);
    }

    public void ShowStep(int stepIndex)
    {
        currentStep = stepIndex;

        for (int i = 0; i < tutorialPopups.Length; i++)
            tutorialPopups[i].SetActive(i == stepIndex);
    }

    public void StepComplete()
    {
        int next = currentStep + 1;

        if (next < tutorialPopups.Length)
            ShowStep(next);
        else
            Debug.Log("Tutorial Finished!");
    }

    public int GetCurrentStep()
    {
        return currentStep;
    }
}
