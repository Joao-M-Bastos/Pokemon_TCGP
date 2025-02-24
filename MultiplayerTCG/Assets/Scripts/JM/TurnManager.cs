using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{

    public PlayerController _player;

    

    private void OnEnable()
    {
        PlayerController.onPlayerFinishTurn += ChangePlayer;
    }
    private void OnDisable()
    {
        PlayerController.onPlayerFinishTurn -= ChangePlayer;
    }

    private void Start()
    {
        _player.SetUpPlayer();
    }

    public void StartGame(bool iStart)
    {
        if(iStart)
            _player.StartTurn();
        else
            _player.FinishTurn();
    }


    public void Update()
    {
        _player.ExecuteTurnActions();
    }

    public void ChangePlayer()
    {
        if (_player.IsMyTurn)
            _player.FinishTurn();
        else
            _player.StartTurn();
    }
}
