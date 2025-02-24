using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] BoardSlot[] slots;
    [SerializeField] MensagerSender _mensagerSender;

    public BoardSlot[] Slots => slots;

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotID(i);
        }
    }

    public void PlayPokemon(PokemonData data, out bool isPokemonPlayed, out int boardID)
    {
        isPokemonPlayed = false;
        boardID = -1;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsSlotEmpty())
            {
                slots[i].AddPokemonToSlot(data);
                boardID = i;
                isPokemonPlayed=true;
                _mensagerSender.SendMensageToServer("PlayPokemonCard:" + boardID + "," + data.ID + ";");
                return;
            }
        }
    }

    public void ReciveAttack(int damage)
    {
        ReciveAttack(damage, 0);
    }

    public void ReciveAttack(int damage,int slotToHit)
    {
        slots[slotToHit].PokemonTakeDamage(damage, out bool pokemonDied);

        if (pokemonDied)
            HandleDeath(slotToHit);
    }

    public void HandleDeath(int slot)
    {
        _mensagerSender.SendMensageToServer("PokemonFainted:" + slot + ";");

        if (slot == 0)
        {
            HasPokemonInAnySlot(out int slotWithPokemonID);

            if (slotWithPokemonID == -1)
                return;

            ChangePokemonInSlots(0, slotWithPokemonID);
        }
    }

    private void ChangePokemonInSlots(int a, int b)
    {
        PokemonData dataB = slots[b].CurrentPokemonData;
        PokemonScrpt scptB = slots[b].CurrentPokemonInSlot;

        if (!slots[a].IsSlotEmpty())
        {
            slots[b].AddPokemonToSlot(slots[a].CurrentPokemonData, slots[a].CurrentPokemonInSlot);
            _mensagerSender.SendMensageToServer("PlayPokemonCard:" + b + "," + slots[b].CurrentPokemonData.ID + ";");
            _mensagerSender.SendMensageToServer("EnergyAdded:" + b + "," + slots[b].HowMuckEnergy() + ";");
        }
        else
        {
            _mensagerSender.SendMensageToServer("RemoveEnemyPokemon: " + b + ";");
            slots[b].RemovePokemon();
        }

        if (dataB != null)
        {
            slots[a].AddPokemonToSlot(dataB, scptB);
            _mensagerSender.SendMensageToServer("PlayPokemonCard:" + a + "," + slots[a].CurrentPokemonData.ID + ";");
            _mensagerSender.SendMensageToServer("EnergyAdded:" + a + "," + slots[a].HowMuckEnergy() + ";");
        }
        else
        {
            _mensagerSender.SendMensageToServer("RemoveEnemyPokemon: " + b + ";");
            slots[a].RemovePokemon();
        }
    }

    public void HasPokemonInAnySlot(out int slotWithPokemonID)
    {
        slotWithPokemonID = -1;
        for (int i = 1;i < slots.Length;i++)
        {
            if (!slots[i].IsSlotEmpty())
            {
                slotWithPokemonID = i;
                return;
            }
        }
    }
}
