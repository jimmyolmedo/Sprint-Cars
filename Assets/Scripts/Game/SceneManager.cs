using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager =  UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    //variables
    [SerializeField] Image image;

    //properties
    protected override bool persistent => true;

    //methods
    protected override void Awake()
    {
        base.Awake();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Fade(sceneName));
    }

    IEnumerator Fade(string sceneName)
    {
        Color a = new Color(0,0,0,0);
        Color b = new Color(0,0,0,1);

        for (float i = 0;  i < 1.5f;  i += Time.deltaTime)
        {
            image.color = Color.Lerp(a,b,i/1.5f);
            yield return null;
        }
        image.color = b;

        AsyncOperation asyncLoad = UnitySceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Esperar a que esté casi cargada
        while (asyncLoad.progress < 0.9f)
            yield return null;

        // Activar escena
        asyncLoad.allowSceneActivation = true;

        yield return new WaitForSeconds(0.2f); // pequeña pausa opcional


        for (float i = 0; i < 1.5f; i += Time.deltaTime)
        {
            image.color = Color.Lerp(b,a,i/1.5f);
            yield return null;
        }
        image.color = a;
    }
}
