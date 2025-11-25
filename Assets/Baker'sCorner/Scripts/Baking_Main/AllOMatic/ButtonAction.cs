using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public AllOMatic targetAllOMatic;
    public string actionName; // "Melt", "Pop", "Start", or number

    public void OnButtonPressed()
    {
        if (targetAllOMatic == null)
        {
            Debug.LogWarning("ButtonAction: No AllOMatic assigned!");
            return;
        }

        switch (actionName)
        {
            case "Melt":
                targetAllOMatic.PressMelt();
                break;

            case "Pop":
                targetAllOMatic.PressPop();
                break;

            case "Start":
                targetAllOMatic.PressStart();
                break;

            default:
                targetAllOMatic.PressNumber(actionName);
                break;
        }
    }
}
