﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int maxHealthPoints;
    int currentHealthPoints;

    public Transform healthBar;
    public GameObject dmgTxtPrefab;
    public Transform damageTxtBasePos;
    Vector3 healthBarStartScale;
    public float timeToDestroy = 1f;

    public UnityEvent OnFirstHit;
    public UnityEvent OnHit;
    public UnityEvent OnKill;
    public float lifeBarTweenTime = 0.2f;
    public Ease lifeLoseEase = Ease.InElastic;

    public float cameraShakeDuration = 0.2f;
    public float cameraShakeMagnitude = 0.2f;
    public ParticleSystem hitParticles;
    public ParticleSystem deathParticles;
    public SpriteRenderer sprite;

    public float CameraShakeMagnitude { get => cameraShakeMagnitude; set => cameraShakeMagnitude = value; }

    private void Start()
    {
        GameManager.gm.enemiesCount++;
        currentHealthPoints = maxHealthPoints;
        healthBarStartScale = healthBar.localScale;
    }
    public void Damage(int damage)
    {
        GameObject dmgTxt = Instantiate(dmgTxtPrefab, damageTxtBasePos.position + (Vector3)Random.insideUnitCircle/3, transform.rotation);
        dmgTxt.GetComponent<DamageText>().ShowDamage(damage);
        hitParticles.Play();
        StartCoroutine(Player.instance.GetComponent<FirstPersonAIO>().CameraShake(cameraShakeDuration, CameraShakeMagnitude));
        if(currentHealthPoints == maxHealthPoints)
        {
            OnFirstHit.Invoke();
        }
        currentHealthPoints -= damage;

        if (currentHealthPoints <= 0)
        {
            OnKill.Invoke();
            //cosas de morirse
            deathParticles.Play();
            Destroy(gameObject, timeToDestroy);
            healthBar.DOScale(new Vector3(0, healthBarStartScale.y, healthBarStartScale.z), 0.1f).SetEase(lifeLoseEase);
            this.enabled = false;
        }
        else
        {
            OnHit.Invoke();
            healthBar.DOScale(new Vector3(((float)currentHealthPoints / (float)maxHealthPoints) * healthBarStartScale.x, healthBarStartScale.y, healthBarStartScale.z)
                , 0.1f).SetEase(lifeLoseEase);
        }
    }

    public void IncreaseCameraShake(float extraShake)
    {
        cameraShakeMagnitude += extraShake;
    }

    public void Vanish(float timeToVanish = 1f)
    {
        sprite.DOFade(0, timeToVanish);
    }
}
