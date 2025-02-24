using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySlots : MonoBehaviour
{
    PokemonData CurrentPokemonData;
    GameObject pokemon;

    [SerializeField] Text energyQuantity;
    [SerializeField] Text hp;
    [SerializeField] Text nameText;
    [SerializeField] GameObject energyCanvas;
    [SerializeField] GameObject hpCanvas;
    [SerializeField] GameObject nameCanvas;

    int SlotID;

    public void UpdateTextEnergy(int value)
    {
        energyCanvas.SetActive(value >= 0);

        energyQuantity.text = value.ToString();
    }

    public void UpdateTextLife(int value)
    {
        hpCanvas.SetActive(value >= 0);

        hp.text = value.ToString();
    }

    public void UpdateTextName(string name)
    {
        nameCanvas.SetActive(name != "");

        nameText.text = name;
    }
    public void AddPokemonToSlot(PokemonData pokemonData)
    {
        CurrentPokemonData = pokemonData;

        pokemon = Instantiate(CurrentPokemonData.EnemyPrefab, transform);

        SpriteRenderer _spriteRenderer = pokemon.GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = pokemonData.cardArt;
    }

    public void SetSlotID(int id)
    {
        SlotID = id;
    }

    public bool IsSlotEmpty()
    {
        return CurrentPokemonData == null;
    }

    public void RemovePokemon()
    {
        CurrentPokemonData = null;

        UpdateTextEnergy(-1);
        UpdateTextLife(-1);
        UpdateTextName("");

        Destroy(pokemon);
    }
}
