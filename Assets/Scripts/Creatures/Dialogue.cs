using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Dialogue : MonoBehaviour
{
    [SerializeField] private UnityEvent onDialogueStart, onDialogueNewLine, onDialogueEnd;
    [Tooltip("The dialogue TextMeshPro")]
    [SerializeField] private TextMeshProUGUI textUI; // The 
    [Tooltip("The dialogue panel")]
    [SerializeField] private Image backgroundPanel;
    
    private Canvas dialogueCanvas;
    private Queue<string> upcomingLines;
    private bool isTalking = false;
    private bool isTyping = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueCanvas = textUI.canvas; 
        textUI.canvas.enabled = false;
    }

    public void StartTalking(TextAsset manuscript)
    {
        string[] lines = manuscript.text.Split('\n');

        if (lines.Length <= 0)
        {
            Debug.Log("No lines found: " + manuscript.text);
        }

        isTalking = true;
        upcomingLines = new Queue<string>(lines);
        
        onDialogueStart?.Invoke();
        dialogueCanvas.enabled = true;
        StartCoroutine(TypeSentence(upcomingLines.Dequeue()));
        onDialogueNewLine?.Invoke();
    }

    public void ProgressDialogue(InputAction.CallbackContext context)
    {
        if (context.started && isTalking &&  !isTyping)
        {
            if (upcomingLines.Count > 0)
            {
                StartCoroutine(TypeSentence(upcomingLines.Dequeue()));
                onDialogueNewLine?.Invoke();
            }
            else
            {
                dialogueCanvas.enabled = false;
                isTalking = false;
                onDialogueEnd?.Invoke();
            }
        }
    }

    [Tooltip("Changes panels sprite")]
    public void ChangePanelSprite(Sprite sprite)
    {
        if (backgroundPanel)
        {
            backgroundPanel.sprite = sprite;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textUI.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textUI.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
    }
}
