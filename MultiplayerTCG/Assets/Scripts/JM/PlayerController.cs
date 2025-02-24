using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Deck _playerDeck;
    [SerializeField] Hand _hand;
    [SerializeField] BoardManager _board;

    [SerializeField] EnemyBoard _enemyBoard;

    [SerializeField] MensagerSender _mensagerSender;

    bool _isMyTurn;
    bool _energyUsed;
    bool _attacked;

    public delegate void OnPlayerFinishTurn();
    public static OnPlayerFinishTurn onPlayerFinishTurn;

    public bool IsMyTurn => _isMyTurn;

    public int AmountOfWins;

    JogadaController jogadaController;

    #region TurnStuff

    public void SetUpPlayer()

    {
        jogadaController = new JogadaController();  
        _playerDeck.StartGame();
        _energyUsed = false;
        _attacked = false;
    }

    public void StartTurn()
    {
        _isMyTurn = true;
        _energyUsed = false;
        _attacked = false;
        DrawCard();
        
        _hand.SetHandCardsUsability(_isMyTurn);
    }

    public void DrawCard()
    {
        _playerDeck.DrawCard(out CardData card);
        
        if (card == null)
        {
            return;
        }
        
        _hand.AddCard(card);
    }

    public void FinishTurn()
    {
        
        _isMyTurn = false;
        _hand.SetHandCardsUsability(_isMyTurn);
    }

    public void ExecuteTurnActions()
    {
        if (!_isMyTurn)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onPlayerFinishTurn?.Invoke();
            _mensagerSender.SendMensageToServer("FinishTurn" + ";");
        }
    }

    #endregion

    #region Cards

    public void CardUsedFromHand(Card cardUsed)
    {
        if (!IsMyTurn)
            return;

        TryUseCard(cardUsed);
    }

    public void TryUseCard(Card cardUsed)
    {
        CardData data = cardUsed.Data;
        bool isCardPlayed = false;

        if (data.GetType() == ScriptableObject.CreateInstance<PokemonData>().GetType())
        {
            PokemonData pokemon = (PokemonData)data;

            _board.PlayPokemon(pokemon, out bool isPokemonPlayed, out int boardID);

            CadastraJogada(data, boardID, -1, -1);

            Debug.Log("Pokemon " + data.cardName + " played");

            isCardPlayed = isPokemonPlayed;
        }

        if(isCardPlayed)
            _hand.RemoveCardFromHand(cardUsed);
    }
    #endregion

    #region Slots

    public void ReciveAttack(int damage) {
        _board.ReciveAttack(damage);
        _mensagerSender.SendMensageToServer("UpdateLife:" + 0 + "," + _board.Slots[0].HowMuchLife()+ ";");
    }

    public void SlotClicked(BoardSlot slotClicked)
    {
        if (slotClicked.IsSlotEmpty())
            return;

        if (!_energyUsed)
        {
            slotClicked.AddEnergy(out bool energyAddedSuccessfuly);
            CadastraJogada(slotClicked.CurrentPokemonData, slotClicked.SlotID, 1,-1);
            _mensagerSender.SendMensageToServer("EnergyAdded:" + slotClicked.SlotID + "," + slotClicked.HowMuckEnergy() + ";");
            _energyUsed = energyAddedSuccessfuly;
            return;
        }

        if(!_attacked && slotClicked.IsActiveSlot(out int damage) && slotClicked.HasEnergyToAttack()){
            CadastraJogada(slotClicked.CurrentPokemonData, slotClicked.SlotID, -1, damage);
            _mensagerSender.SendMensageToServer("ReciveAttack:"+damage + ";");
            _attacked = true;
        }
    }

    #endregion

    public void FaintedEnemyPokemon(int slotID)
    {
        AmountOfWins++;
        DrawCard();

        _enemyBoard.RemovePokemonFromSlot(slotID);

        if(AmountOfWins >= 3)
        {
            _mensagerSender.SendMensageToServer("BINGO:0" + ";");
            Win();
        }
    }

    public void Loose()
    {
        SceneManager.LoadScene("Loose");
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }

    public void PutPokemonOnEnemyBoard(int[] i)
    {
        _enemyBoard.PutPokemon(i[0], i[1]);
    }

    public void RemovePokemonOnEnemyBoard(int i)
    {
        _enemyBoard.RemovePokemonFromSlot(i);
    }
    public void EnemyAddedEnergy(int[] i)
    {
        _enemyBoard.SetEnergy(i[0], i[1]);
    }

    public void EnemyPokemonChangedLife(int[] i)
    {
        _enemyBoard.SetLife(i[0], i[1]);
    }

    public void CadastrarPartida()
    {
        _mensagerSender.SendMensageToServer("CadastrarPartida:" + JogadorVar.varInstance.baralho.cod + ";");
    }

    public void PartidaCadastrada()
    {
        _mensagerSender.SendMensageToServer("PartidaCadastrada:" + JogadorVar.varInstance.partidaID + ";");
    }

    public void CadastraJogada(CardData cardData, int localCarta, int energia, int dano)
    {
        Jogada novaJogada = new Jogada();

        novaJogada.item = cardData.itemID;

        novaJogada.baralho = JogadorVar.varInstance.baralho.cod;
        novaJogada.carta = cardData.ID;
        novaJogada.partida = JogadorVar.varInstance.partidaID;

        novaJogada.local_carta = localCarta;

        novaJogada.energia = energia;
        novaJogada.dano = dano;

        JogadaController jogadaController = new JogadaController();

        novaJogada = jogadaController.CreateJogada(novaJogada);

        if (dano != -1)
        {
            Acao acao = new Acao();

            acao.jogada = novaJogada.cod;
            acao.alvo = 0;
            acao.valor_efeito = dano;
            acao.efeito = "Causou " + dano + " no campo ativo do adversario";

            AcaoController acaoController = new AcaoController();
            acaoController.CreateAcao(acao);
        }

        if (dano == -1 && energia == -1)
        {
            Acao acao = new Acao();

            acao.jogada = novaJogada.cod;
            acao.alvo = localCarta;
            acao.valor_efeito = 0;
            acao.efeito = "Jogou pokemon " + cardData.ID + " no campo " + localCarta;

            AcaoController acaoController = new AcaoController();
            acaoController.CreateAcao(acao);
        }
    }
}
