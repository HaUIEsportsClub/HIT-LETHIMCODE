using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Space]
    [Header("Setting Animation")]
    public Ease scaleEase;
    public float scaleOnHover;
    public float scaleTransition;

    public float hoverPunchAngle;
    public float hoverTransition;


    [Space] [Header("Setting Level")] [SerializeField]
    private Button button;

    [SerializeField] private int level;
    public bool Lock = true;

    public bool IsLock
    {
        set => PlayerPrefs.SetInt("Level_" + level, value ? 1 : 0);
        get => PlayerPrefs.GetInt("Level_" + level, 0) == 1;
    }
    
    private void Start()
    {
        if (!Lock)
        {
            IsLock = true;
        }
        button.onClick.AddListener(StartLevel);
    }

    private void StartLevel()
    {
        if(!IsLock) return;
        GameController.Instance.PlayGame();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
        DOTween.Kill(2, true);
        transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.Kill(2, true);
        transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        transform.DOPunchRotation(Vector3.forward * -hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }

    public void AnimationUnClock()
    {
        transform.DOScale(1.5f, scaleTransition).SetEase(scaleEase).SetLoops(4, LoopType.Yoyo);
        transform.DOPunchRotation(Vector3.forward * 20f, hoverTransition, 20, 1).SetLoops(4, LoopType.Yoyo);
    }
}
