using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UpdatableTMP : MonoBehaviour
{
    TMP_Text txt;
    public float timeToClear = 10f;
    public float charsPerSecond = 30f;
    public UnityEvent OnDialogFinished;

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }
    public void ClearText()
    {
        txt.text = "";
    }

    public void UpdateText(string newText)
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTextCr(newText));
    }

    IEnumerator UpdateTextCr(string newText)
    {
        ClearText();

        char[] characters = newText.ToCharArray();

        bool inTag = false;

        foreach( char c in characters)
        {
            if (c == '<')
                inTag = true;
            else if (c == '>')
                inTag = false;

            txt.text += c;

            if(!inTag)
                yield return new WaitForSeconds(1 / charsPerSecond);
        }

        yield return new WaitForSeconds(timeToClear);
        ClearText();
        OnDialogFinished.Invoke();
    }

    public void SetTimeToClear(float ttc)
    {
        this.timeToClear = ttc;
    }

    public void SetTimeCharsPerSecond(float cps)
    {
        this.charsPerSecond = cps;
    }
}
