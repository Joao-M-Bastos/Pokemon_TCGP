using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MensagerReceptor : MonoBehaviour
{
    [SerializeField] TurnManager _turnManager;
    [SerializeField] Text debbuger;

    public PlayerController _player;

    private Dictionary<string, Action<int[]>> commands;

    PartidaController partidaController;

    public void Start()
    {
        partidaController = new PartidaController();
        commands = new Dictionary<string, Action<int[]>>
        {
            { "StartGame", VerifyWhoStarts },
            { "CadastrarPartida", CadastrarPartida },
            { "PartidaCadastrada", GetPartidaID },
            { "FinishTurn", ChangeCurrentPlayer },
            { "ReciveAttack", ReciveAttack },
            { "PokemonFainted", GainPoint },
            { "RemoveEnemyPokemon", RemovePokemonFromSlot },
            { "PlayPokemonCard", EnemyPlayedPokemon },
            { "UpdateLife", EnemyPokemonChangedLife },
            { "EnergyAdded", EnemyAddedEnergy },
            { "BINGO", Loose }
        };
    }

    public void ReciveMensage(string response)
    {

        string[] partsGerais = response.Split(';');

        foreach (string part in partsGerais)
        {
            string[] parts = part.Split(':');
            string command = parts[0];
            string[] parametersString = parts.Length > 1 ? parts[1].Split(',') : new string[0];

            int[] parameters = new int[parametersString.Length];

            for (int i = 0; i < parametersString.Length; i++)
            {
                parameters[i] = int.Parse(parametersString[i]);

            }

            if (commands.TryGetValue(command, out Action<int[]> action))
            {
                action.Invoke(parameters);
            }
            else
            {
                Debug.LogWarning($"Comando não reconhecido: {command}");
            }
        }
    }

    public void ChangeCurrentPlayer(int[] parameters)
    {
        _turnManager.ChangePlayer();
    }

    void VerifyWhoStarts(int[] parameters)
    {


        if (parameters[0] == 0)
            _turnManager.StartGame(true);
        else
        {
            _player.CadastrarPartida();
            _turnManager.StartGame(false);
        }
    }

    void CadastrarPartida(int[] paramaters)
    {
        Partida partida = new Partida();

        partida.baralho_01 = paramaters[0];
        partida.baralho_02 = JogadorVar.varInstance.baralho.cod;

        partida = partidaController.CreatePartida(partida);

        int[] i = new int[1];
        i[0] = partida.cod;
;       GetPartidaID(i);

        _player.PartidaCadastrada();
    }

    public void GetPartidaID(int[] partidaCOD)
    {
        JogadorVar.varInstance.partidaID = partidaCOD[0];
    }

    void ReciveAttack(int[] damage)
    {
        _player.ReciveAttack(damage[0]);
    }

    void EnemyPlayedPokemon(int[] parameters)
    {
        _player.PutPokemonOnEnemyBoard(parameters);
    }

    void EnemyAddedEnergy(int[] parameters)
    {
        _player.EnemyAddedEnergy(parameters);
    }

    void EnemyPokemonChangedLife(int[] parameters)
    {
        _player.EnemyPokemonChangedLife(parameters);
    }

    public void GainPoint(int[] parameters)
    {
        _player.FaintedEnemyPokemon(parameters[0]);
    }
    public void RemovePokemonFromSlot(int[] parameters)
    {
        _player.RemovePokemonOnEnemyBoard(parameters[0]);
    }

    public void Loose(int[] parameters)
    {
        _player.Loose();
    }

}
