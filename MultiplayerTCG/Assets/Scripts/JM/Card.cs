using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CardData _data;

    public CardData Data => _data; 

    bool _canUse;

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] SpriteRenderer _spriteHolvered;

    [SerializeField] Text[] texts;

    public void InicializeCard(CardData data)
    {
        _data = data;
        _spriteRenderer.sprite = _data.cardArt;
        _spriteHolvered.sprite = _data.cardArt;

        texts[0].text = _data.cardName;
        texts[1].text = _data.cardName;

        if (_data.GetType() == ScriptableObject.CreateInstance<PokemonData>().GetType())
        {
            PokemonData pokemon = (PokemonData)data;
            texts[2].text = pokemon.MaxLife.ToString();
            texts[3].text = pokemon.MaxLife.ToString(); 
        }
    }

    public void SetUsability(bool usability)
    {
        _canUse = usability;
    }

    public void ShowHolveredImage(bool holvered)
    {
        _spriteRenderer.gameObject.SetActive(!holvered);
        _spriteHolvered.gameObject.SetActive(holvered);
    }    
}
