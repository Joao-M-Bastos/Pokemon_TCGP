using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CriarConta : MonoBehaviour
{
    [FormerlySerializedAs("inputEmail")] public TMP_InputField inputNome;
    public TMP_InputField inputUsuario;
    public TMP_InputField inputSenha;
    public TMP_Text Mensagem;

    
    public Button voltarButton;
    public Button criarJogador;
    public GameObject canvaLogin;

    void Start()
    {
        criarJogador.onClick.AddListener(OnCriarJogadorClicked);
        voltarButton.onClick.AddListener(VoltaTelaLogin);
    }
    
    private void OnCriarJogadorClicked()
    {
        string inputNomeText = inputNome.text;
        string inputUsuarioText = inputUsuario.text;
        string inputSenhaText = inputSenha.text;
        if (inputNomeText != "" && inputUsuarioText != "" && inputSenhaText != "")
        {
            CriarJogador(inputNomeText, inputUsuarioText, inputSenhaText); 
        }
        else
        {
            Mensagem.text = "Preencha todos os campos!";
        }
        
    }

    private void VoltaTelaLogin()
    {
        canvaLogin.SetActive(true);
        gameObject.SetActive(false);
    }

    private void CriarJogador(string nome, string usuario, string senha)
    {
        try
        {
            JogadorController jogadorController = new JogadorController();
            Jogador jogador = new Jogador
            {
                nome = nome,
                usuario = usuario,
                senha = senha
            };
        
            jogadorController.CreatePlayer(jogador);
            
            Mensagem.text = $"Jogador criado com sucesso!";
            VoltaTelaLogin();
        }
        catch (Exception e)
        {
            
            Mensagem.text = "Usuario já cadastrado, tente outro usuario!";

        }
        

    }
    
    
}