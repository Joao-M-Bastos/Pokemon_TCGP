using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardSlot : MonoBehaviour
{
    PokemonData currentPokemonData;
    [SerializeField] PokemonScrpt currentPokemonInSlot;

    public PokemonData CurrentPokemonData => currentPokemonData;
    public PokemonScrpt CurrentPokemonInSlot => currentPokemonInSlot;

    [SerializeField] Text energyQuantity;
    [SerializeField] GameObject energyCanvas;
    

    public int SlotID;

     void UpdateTextEnergy()
    {
        energyCanvas.SetActive(currentPokemonInSlot != null);
        if(currentPokemonInSlot != null)
        {
            energyQuantity.text = currentPokemonInSlot.Energy.ToString();
        }

    }

    public int HowMuckEnergy()
    {
        return currentPokemonInSlot.Energy;
    }

    public int HowMuchLife()
    {
        return currentPokemonInSlot.CurrentLife;
    }

    public void AddPokemonToSlot(PokemonData pokemonData)
    {
        if(pokemonData == null)
        {
            RemovePokemon();
            return;
        }

        currentPokemonData = pokemonData;

        GameObject newPokemon = Instantiate(currentPokemonData.PokemonPrefab, transform);
        currentPokemonInSlot = newPokemon.GetComponent<PokemonScrpt>();

        currentPokemonInSlot.Inicialize(pokemonData);
        UpdateTextEnergy();
    }

    public void AddPokemonToSlot(PokemonData pokemonData, PokemonScrpt existingPokemon)
    {

        if (pokemonData == null || existingPokemon == null)
        {
            RemovePokemon();
            return;
        }

        currentPokemonData = pokemonData;

        currentPokemonInSlot = existingPokemon;
        currentPokemonInSlot.transform.position = transform.position;
        currentPokemonInSlot.transform.SetParent(transform);

        UpdateTextEnergy();
    }

    public void RemovePokemon()
    {
        currentPokemonData = null;
        currentPokemonInSlot = null;

        UpdateTextEnergy();
    }

    public bool IsSlotEmpty()
    {
        return currentPokemonInSlot == null;
    }

    public bool CanAddPokemon(int evolutionID)
    {
        if (evolutionID == 0)
            return IsSlotEmpty();

        return currentPokemonData.PokemonID == evolutionID;
    }

    public void PokemonTakeDamage(int amountOfDamage, out bool pokemonDied)
    {
        currentPokemonInSlot.TakeDamage(amountOfDamage, out bool died);
        pokemonDied = died;

        if (pokemonDied)
        {
            RemovePokemon();
        }
    }

    public void AddEnergy(out bool added)
    {
        added = false;
        if (currentPokemonInSlot != null)
        {
            currentPokemonInSlot.AddEnergy();
            UpdateTextEnergy();
            added = true;
        }
    }

    public bool HasEnergyToAttack()
    {
        return currentPokemonInSlot.Energy >= currentPokemonData.AttackCost;
    }

    public bool IsActiveSlot(out int damage)
    {
        damage = currentPokemonInSlot.Data.AttackDamage;
        return SlotID == 0;
    }

    public void SetSlotID(int id)
    {
        SlotID = id;
    }
}
