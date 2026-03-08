using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Title : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] int timeAfk;
    [SerializeField] bool inVideo;
    [SerializeField] GameObject Video;

    private void OnEnable()
    {
        playerInput.onActionTriggered += PressStartButton;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= PressStartButton;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions.Disable(); // Desactiva todo
        playerInput.SwitchCurrentActionMap("Player"); // Activa solo el que quieres
        StartCoroutine(SpawnVideo());
    }


    public void PressStartButton(InputAction.CallbackContext context)
    {
        if(context.action.name != "Start") { return; }

        if(context.started)
        {
            if (inVideo)
            {
                Video.SetActive(false);
                inVideo = false;
                StartCoroutine(SpawnVideo());
            }
            else
            {
                SceneManager.instance.LoadScene("PlayerSelector");
                PlayerPrefs.DeleteAll();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SpawnVideo()
    {
        yield return new WaitForSeconds(timeAfk);

        Video.SetActive(true);
        inVideo = true;
    }
}
