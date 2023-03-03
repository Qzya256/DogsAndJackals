using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBarFill;
    [SerializeField] private float speed;
    [SerializeField] private GameObject soundOnSprite;
    [SerializeField] private GameObject soundOffSprite;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("audioListenerEnebable"))
        {
            PlayerPrefs.SetInt("audioListenerEnebable", 1);
        }
        if (PlayerPrefs.GetInt("audioListenerEnebable") == 0)
        {
            AudioListener.volume  = 0;
            soundOnSprite.SetActive(false);
            soundOffSprite.SetActive(true);
        }
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SoundSwitcher()
    {
        if (AudioListener.volume == 1)
        {
            soundOnSprite.SetActive(false);
            soundOffSprite.SetActive(true);
            PlayerPrefs.SetInt("audioListenerEnebable", 0);
            AudioListener.volume = 0;
        }
        else
        {
            soundOnSprite.SetActive(true);
            soundOffSprite.SetActive(false);
            PlayerPrefs.SetInt("audioListenerEnebable", 1);
            AudioListener.volume = 1;
        }
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / speed);
            loadingBarFill.value = progressValue;

            yield return null;
        }
    }
}