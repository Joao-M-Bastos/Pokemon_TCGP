using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField inputUsuario;
    public TMP_InputField inputSenha;
    public TMP_Text Mensagem;
    // Referência ao botão
    public Button submitButton;
    public Button criarJogador;
    public GameObject canvaCriarJogador;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
        criarJogador.onClick.AddListener(OnCriarJogadorClicked);
    }

    private void OnCriarJogadorClicked()
    {
        canvaCriarJogador.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnSubmitButtonClicked()
    {
        string inputUsuarioText = inputUsuario.text;
        string inputSenhaText = inputSenha.text;
        
        if (inputUsuarioText != "" && inputSenhaText != "")
        {
            VerficarJogador(inputUsuarioText, inputSenhaText);
        }
        else
        {
            Mensagem.text = "Preencha todos os campos!";
        }
    }

    private void VerficarJogador(string usuario, string senha)
    {
        try
        {
            JogadorController jogadorController = new JogadorController();
            Jogador jogador = new Jogador();
        
            jogador = jogadorController.CheckPlayer(usuario, senha);
            
            Mensagem.text = $"Jogador encontrado! COD: {jogador.cod} nome : {jogador.nome}";
        }
        catch (Exception e)
        {
            
            Mensagem.text = "Usuario ou senha incorretos!";
            
            // Console.WriteLine(e);
            // throw;
        }
        

    }
    
    
}