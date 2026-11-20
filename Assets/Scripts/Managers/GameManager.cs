using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onGameStarted, onGameWon, onEndGame;
    //[SerializeField] private UnityEvent onEuthanized;
    [Tooltip("The amount of foul creatures left and total. ex 3/10")]
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private GameObject foulCreatureContainer;

    private bool gameStarted = false;
    private bool hasWon = false;
    private int startFoulCreatureAmount;
    private int foulCreaturesEuthanized = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startFoulCreatureAmount = foulCreatureContainer.GetComponentsInChildren<FoulCreature>().Length;
        counterText.enabled = false;
    }
    

    public void EuthanizedFoulCreature()
    {
        foulCreaturesEuthanized++;
        counterText.text = $"{foulCreaturesEuthanized}/{startFoulCreatureAmount}";
        //onEuthanized?.Invoke();

        if (foulCreaturesEuthanized >= startFoulCreatureAmount)
        {
            WinGame();
        }
    }
    
    public void TryStartorEndGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            onGameStarted?.Invoke();
            ShowCounterText(true);
        }
        else if (hasWon)
        {
            onEndGame?.Invoke();
        }
        
    }
    
    private void ShowCounterText(bool show)
    {
        if (show) counterText.text = $"{foulCreaturesEuthanized}/{startFoulCreatureAmount}";
        counterText.enabled = show;
    }

    
    private void WinGame()
    {
        hasWon = true;
        onGameWon?.Invoke();
    }
}
