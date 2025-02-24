using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Deck : MonoBehaviour
{
    [SerializeField] PokemonData baseCard;
    [SerializeField] CardData[] unshuffledCards;
    List<CardData> cards = new List<CardData>();

    private CartaController _cardController;


    private void Awake()
    {
        _cardController = new CartaController();
        
    }

    public void StartGame()
    {
        
        ReciveDataFromDB();
        
        foreach (CardData card in unshuffledCards)
        {
            cards.Add(card);
        }

        //Shuffle();
    }

    public void ReciveDataFromDB()
    {
        List<Carta> cardsInDeck = _cardController.GetCartas(JogadorVar.varInstance.baralho.cod);

        int numOfCards = cardsInDeck.Count;

        unshuffledCards = new CardData[numOfCards];

        for (int i = 0; i < numOfCards; i++)
        {
            PokemonData pokemon = ScriptableObject.CreateInstance<PokemonData>();

            pokemon.PokemonPrefab = baseCard.PokemonPrefab;

            pokemon.ID = cardsInDeck[i].cod;

            pokemon.itemID = cardsInDeck[i].itemCod;

            pokemon.cardName = cardsInDeck[i].nome;
            pokemon.cardArt = baseCard.cardArt;
            
            pokemon.MaxLife = cardsInDeck[i].hp;
            pokemon.AttackName = cardsInDeck[i].ataqueNome;
            pokemon.AttackCost = cardsInDeck[i].ataqueCusto;
            pokemon.AttackDamage = cardsInDeck[i].ataqueDano;
            pokemon.RetreatCost = cardsInDeck[i].recuo;
            pokemon.Effect = cardsInDeck[i].efeito;
            pokemon.Stage = cardsInDeck[i].estagio;
            pokemon.Type = cardsInDeck[i].tipo;

            unshuffledCards[i] = pokemon;
        }
    }

    public void Shuffle()
    {
        //Embaralha as Cartas do deck
        //cards = cards.OrderBy(_ =>  new Random().Next()).ToList();
    }

    public void DrawCard(out CardData cardDrown)
    {
        
        if (cards.Count <= 0)
        {
            cardDrown = null;
            
            return;
        }
        int cardSelected;

        //Use only when using Shuffle(). Card pick is always from the top 
        //cardSelected = 0;

        //Does not need to Shuffle(). Card pick does not follow deck order
        //Debug.Log(Random.Range(0, cards.Count));

        cardSelected = Random.Range(0, cards.Count);
        cardDrown = cards[cardSelected];
        cards.RemoveAt(cardSelected);
    }
}
