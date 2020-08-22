using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UpdatableTMP : MonoBehaviour
{
    [SerializeField] private AudioSource aS;
    TMP_Text txt;
    public float timeToClear = 10f;
    public float charsPerSecond = 30f;
    public UnityEvent OnDialogFinished;
    public float minPitch = 0.85f;
    public float maxPitch = 1.15f;
    public AudioSource AS { get => aS; set => aS = value; }

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
        if (aS == null) aS = GetComponent<AudioSource>();
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
        if (AS.clip != null) {
            AS.Play();
            StartCoroutine(TalkSoundCr());
        }
        char[] characters = newText.ToCharArray();
        bool inTag = false;

        foreach( char c in characters)
        {
            if (c == '<')
                inTag = true;
            else if (c == '>')
                inTag = false;

            txt.text += c;

            if(!inTag && c != ' ')
                yield return new WaitForSeconds(1 / charsPerSecond);
        }
        StopCoroutine("TalkSoundCr");
        AS.Pause();
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

    IEnumerator TalkSoundCr()
    {
        while (true)
        {
            aS.pitch = Random.Range(minPitch, maxPitch);
            yield return new WaitUntil(() => aS.time < 0.02f);
        }
    }
}
