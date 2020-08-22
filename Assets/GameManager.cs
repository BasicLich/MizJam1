using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int enemiesCount = 0;
    public float timeToEnd = 0;
    public int tutorialStage = 0;

    public UpdatableTMP txt;

    private void Awake()
    {
        gm = this;
        StartCoroutine(TutorialCr());
    }

    private void Update()
    {
        if (tutorialStage == 0)
        {

        }
    }

    IEnumerator TutorialCr()
    {
        txt.timeToClear = 999;
        txt.charsPerSecond = 15;
        txt.UpdateText("Greetings, adventurer. Welcome to this fierceful lands. I am Tutorial Guy.");
        yield return new WaitForSeconds(10f);
        txt.UpdateText("You can move using <color=\"red\"><i>WASD</i></color>. You don't even need a mouse. What a futuristic control scheme!");
        yield return new WaitForSeconds(10f);
        txt.UpdateText("However, you actually need a mouse, as you attack with <color=\"red\"><i>Left click</i></color>.");
        yield return new WaitForSeconds(10f);
        txt.UpdateText("Kill somebody to start the quest. It seems like I'm the only one nearby \\(^.^)/");

    }
}
