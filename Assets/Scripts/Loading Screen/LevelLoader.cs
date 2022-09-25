using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int sceneIndex = 0;
    [SerializeField] TMP_Text percentageLoadingText;

    void Start()
    {

        StartCoroutine(LoadSceneAsync(sceneIndex));

    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone)
        {
            var progress = asyncLoad.progress * 100f;
            percentageLoadingText.SetText(string.Format("{0:%%}.", progress.ToString()));
            yield return null;
        }
    }
}
