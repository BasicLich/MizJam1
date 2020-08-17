using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    TMPro.TMP_Text txt;
    public float secondsActive = 0.5f;
    Vector3 startScale;
    Vector3 startPos;
    Tween shakeTween, colorTween;

    private void Awake()
    {
        txt = GetComponent<TMPro.TMP_Text>();
        startScale = transform.localScale;
        startPos = transform.localPosition;
        shakeTween = DOTween.Shake(() => transform.position, x => transform.position = x, 0.2f + secondsActive, new Vector3(0.3f, 0.3f, 0));
        shakeTween.SetAutoKill(false).Pause();
        colorTween = txt.DOColor(new Color(1, 0, 0, 0), secondsActive).SetDelay(0.2f); 
        
        colorTween.SetAutoKill(false).Pause();

    }

    public void ShowDamage(int damage)
    {
        txt.text = "-" + damage.ToString();
        txt.alpha = 1;
        transform.localPosition = startPos + (Vector3)Random.insideUnitCircle/3;
        shakeTween.Play().OnComplete(() => Destroy(this.gameObject));
        colorTween.Play();
    }
}
