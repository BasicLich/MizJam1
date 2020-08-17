using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public bool onTutorial = true;
    public SpriteRenderer mySprite;
    public Sprite[] weaponSprites;
    public TMPro.TMP_Text questsUI;
    public UpdatableTMP messagesUI;
    public ParticleSystem upgradeParticles;
    public Animator anim;
    int enemiesKilled = 0;
    int level;
    int currentDamage = 2;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.Play("hit");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy hitEnemy = other.GetComponent<Enemy>();

        if(hitEnemy != null)
        {
            int extraDmg = 0;
            if (Random.Range(0f, 1f) > 0.5f)
            {
                extraDmg += Random.Range(0, Mathf.RoundToInt(Mathf.Sqrt(currentDamage)));
                Debug.Log("Extra damage = " + extraDmg);
            }
            hitEnemy.Damage(currentDamage + extraDmg);
        }
    }
    
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        if (weaponSprites.Length > enemiesKilled)
        {
            mySprite.sprite = weaponSprites[enemiesKilled];
            anim.Play("newWeapon");
            upgradeParticles.Play();
            currentDamage++;
            if (!onTutorial)
                messagesUI.UpdateText("<color=\"yellow\"><i>Level up!</i></color>\n <color=\"red\"> Damage +1</color>");
        }
        questsUI.text = "<color=yellow>.    - QUESTS -    . </color>\n";
        questsUI.text += enemiesKilled >= GameManager.gm.enemiesCount ? "<color=green>" : "<color=white>";
        questsUI.text += "* Kill everyone\n"
                      + "   " + enemiesKilled + "/" + GameManager.gm.enemiesCount;
    }
    
    public void EndTutorial()
    {
        onTutorial = false;
    }
}
