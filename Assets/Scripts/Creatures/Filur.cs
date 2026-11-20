using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer), typeof(CapsuleCollider2D),  typeof(AudioSource))]
[RequireComponent(typeof(CircleCollider2D), typeof(OnTriggerEvent), typeof(Dialogue))]
public class Filur : MonoBehaviour
{
    [SerializeField] public bool isHappy;
    
    [Header("Sad")]
    [SerializeField] private TextAsset sadText;
    [SerializeField] private Sprite sadSprite;
    [SerializeField] private AudioResource sadAudio;
    
    [Header("Happy")]
    [SerializeField] private TextAsset happyText;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private AudioResource happyAudio;
    
    // Private
    private Dialogue dialogue;
    
    private AudioSource audioSource;
    private Animator animator;

    private void Awake()
    {
        dialogue = GetComponent<Dialogue>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        SetHappy(false);
    }

    public void StartDialogue()
    {
        dialogue.StartTalking(isHappy ? happyText : sadText);
    }

    public void SetHappy(bool happy)
    {
        isHappy = happy;
        if (isHappy)
        {
            audioSource.resource = happyAudio;
            dialogue.ChangePanelSprite(happySprite);
            animator.SetBool("isHappy", true);
        }
        else
        {
            audioSource.resource = sadAudio;
            dialogue.ChangePanelSprite(sadSprite);
            animator.SetBool("isHappy", false);
        }
    }
}
