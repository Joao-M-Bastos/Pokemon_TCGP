using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class EscolhaBaralhos : MonoBehaviour
{
    private BaralhoController baralhoController;
    private List<Baralho> baralhos;
    private JogadorVar jogadorVar;
    public GameObject buttonPrefab; // Arraste um botão de exemplo para este campo no Inspector
    public Transform buttonContainer; // Arraste o container (painel) onde os botões serão criados

    void Start()
    {
        jogadorVar = FindObjectOfType<JogadorVar>();
        baralhoController = new BaralhoController();
        baralhos = new List<Baralho>();    

        baralhos = baralhoController.GetBaralhos(jogadorVar.JogadorID());

        // Crie botões dinamicamente
        CreateDeckButtons(baralhos);
    }

    void CreateDeckButtons(List<Baralho> decks)
    {
        foreach (Baralho deck in decks)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            
            LayoutElement layoutElement = newButton.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = newButton.AddComponent<LayoutElement>();
            }
            
            layoutElement.preferredHeight = 30;
            layoutElement.preferredWidth = 30;
            
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = deck.nome;
            newButton.GetComponent<Button>().onClick.AddListener(() => OnDeckButtonClicked(deck));
        }
    }

    void OnDeckButtonClicked(Baralho deck)
    {
        Debug.Log("Deck selecionado: " + deck.nome);
        CartaController cartaController = new CartaController();
        List<Carta> cartas = new List<Carta>();
        
        cartas = cartaController.GetCartas(deck.cod);
        
        jogadorVar.DefineBaralho(deck);
        jogadorVar.cartas = cartas;

        SceneManager.LoadScene("SampleScene");
    }
    
}