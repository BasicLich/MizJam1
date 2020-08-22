using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public TMPro.TMP_Text pressAnyButtonText;

    IEnumerator Start()
    {
        yield return null;

        pressAnyButtonText.text = "Loading...";
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {

            pressAnyButtonText.text = "Loading (" + async.progress * 100 + "%)";

            if (async.progress >= 0.9f)
            {
                pressAnyButtonText.text = "Press any button to start";
                Debug.Log("Loading complete");
                yield return new WaitUntil(() => Input.anyKeyDown);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void Update()
    {

        pressAnyButtonText.alpha = Mathf.Abs(Mathf.Sin(Time.time));
    }


}
