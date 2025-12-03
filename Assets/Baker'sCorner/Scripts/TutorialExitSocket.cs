using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TutorialExitSocket : MonoBehaviour
{
    [Header("Scene To Load")]
    public string nextSceneName = "NextLevel";   // Set this in the Inspector

    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnItemPlaced);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnItemPlaced);
    }

    private void OnItemPlaced(SelectEnterEventArgs args)
    {
        Debug.Log("Tutorial Exit: Object detected in the socket.");

        // OPTIONAL: require a specific tag (“CompletedRecipe”)
        // if (!args.interactableObject.transform.CompareTag("CompletedRecipe"))
        // {
        //     Debug.Log("Wrong item placed, not teleporting.");
        //     return;
        // }

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}

