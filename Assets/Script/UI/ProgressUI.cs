using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressImage;

    private string sceneToLoad;
    private void Start()
    {
        sceneToLoad = SaveManagers.Instance.CurrentSaveData.sceneName;
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressImage.fillAmount = progress;
            if (asyncOperation.progress >= .9f)
            {
                progressImage.fillAmount = 1f;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
