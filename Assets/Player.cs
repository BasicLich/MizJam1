using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Analytics;

public class Player : MonoBehaviour
{
    float timeStart;
    public static Player instance;
    public bool onTutorial = true;
    public SpriteRenderer mySprite;
    public Sprite[] weaponSprites;
    public TMPro.TMP_Text questsUI;
    public UpdatableTMP messagesUI;
    public ParticleSystem upgradeParticles;
    public Animator anim;

    public ParticleSystem deathParticles;
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip levelUpSound;
    public SpriteRenderer fadeoutPanel;
    public AudioSource weaponAS;

    private float finishGameTime;
    int enemiesKilled = 0;
    int level;
    int currentDamage = 2;
    bool dead = false;

    public float FinishGameTime { get => finishGameTime; set => finishGameTime = value; }

    private void Awake()
    {
        instance = this;
        mySprite.sprite = weaponSprites[0];
    }

    private void Start()
    {
        timeStart = Time.time;
        GameManager.gm.enemiesCount++;
        weaponAS.volume = 0.2f;
        //fadeoutPanel.DOFade(0, 1);
    }

    private void Update()
    {
        if (!dead && Input.GetButtonDown("Fire1"))
        {
            anim.Play("hit");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy hitEnemy = other.GetComponent<Enemy>();

        if(hitEnemy != null)
        {
            int extraDmg = 0;
            weaponAS.volume = 0.1f + 0.5f * (enemiesKilled / GameManager.gm.enemiesCount);
            weaponAS.pitch = Random.Range(0.9f, 1.1f);
            if (Random.Range(0f, 1f) > 0.5f)
            {
                extraDmg += Random.Range(0, Mathf.RoundToInt(Mathf.Sqrt(currentDamage)));
                Debug.Log("Extra damage = " + extraDmg);
                weaponAS.pitch += 0.4f;
            }
            hitEnemy.Damage(currentDamage + extraDmg);
            PlayHit();
        }
    }
    
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        if (weaponSprites.Length > enemiesKilled)
        {
            GetComponent<AudioSource>().PlayOneShot(levelUpSound);
            mySprite.sprite = weaponSprites[enemiesKilled];
            anim.Play("newWeapon");
            upgradeParticles.Play();
            currentDamage++;
            if (!onTutorial)
                messagesUI.UpdateText("<color=\"yellow\"><i>Level up!</i></color>\n <color=\"red\"> Weapon upgraded</color>");
        }
        questsUI.text = "<color=yellow>.    - QUESTS -    . </color>\n";
        questsUI.text += enemiesKilled >= GameManager.gm.enemiesCount ? "<color=green>" : "<color=white>";
        questsUI.text += "* Kill everyone\n"
                      + "   " + enemiesKilled + "/" + GameManager.gm.enemiesCount;

        if(enemiesKilled >= GameManager.gm.enemiesCount - 1)
        {
            StartCoroutine(EndGameCr());
        }
    }
    
    public void EndTutorial()
    {
        onTutorial = false;
    }

    IEnumerator EndGameCr()
    {
        messagesUI.UpdateText("You are almost done! But there's still someone alive in this place...");
        yield return new WaitForSeconds(20);
        messagesUI.UpdateText("I told you about what the right mouse button does, didn't I?");
        yield return new WaitUntil(() => Input.GetButtonDown("Fire2"));
        float timeToComplete = Time.time;
        anim.Play("die");
        GetComponent<FirstPersonAIO>().ControllerPause();
        dead = true;
        enemiesKilled++;
        questsUI.text = "<color=yellow>.    - QUESTS -    . </color>\n";
        questsUI.text += enemiesKilled >= GameManager.gm.enemiesCount ? "<color=green>" : "<color=white>";
        questsUI.text += "* Kill everyone\n"
                      + "   " + enemiesKilled + "/" + GameManager.gm.enemiesCount;
        yield return new WaitForSeconds(4);

        FinishGameTime = Time.time - timeStart;
        int minutes = (int)FinishGameTime / 60;
        int seconds = (int)FinishGameTime % 60;
        int cents = (int)((FinishGameTime - (minutes * 60) - seconds) * 100);
        messagesUI.timeToClear = 10000f;
        messagesUI.UpdateText("<color=\"red\"><size=150%><align=\"center\">Game Over\nCompleted in "+ minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString() + "." + ((cents < 10) ? ("0") : ("")) + cents.ToString() +"\n</size></color>Made by @SamberoDev");
    }

    public void PlayHit()
    {
        weaponAS.PlayOneShot(hitSound);
    }
    public void PlayDeathShot()
    {
        weaponAS.PlayOneShot(deathSound);
    }
}
