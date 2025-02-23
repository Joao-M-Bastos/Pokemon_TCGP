using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public JogadorVar jogadorVar;

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
            
            jogadorVar.DefineJogador(jogador);
            
            Mensagem.text = $"Jogador encontrado! COD: {jogador.cod} nome : {jogador.nome}";
            SceneManager.LoadScene("EscolhaBaralho");
        }
        catch (Exception e)
        {
            
            Mensagem.text = $"Usuario ou senha incorretos! ERRO: {e.Message}";
            
            string filePath = "logfile.txt";
            SaveErrorToFile(e, filePath);
            
            // Console.WriteLine(e);
            // throw;
        }
        

    }
    private void SaveErrorToFile(Exception ex, string filePath)
    {
        try
        {
            // Criar ou abrir o arquivo para escrita
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Data e Hora: " + DateTime.Now.ToString());
                writer.WriteLine("Mensagem de Erro: " + ex.Message);
                writer.WriteLine("Stack Trace: " + ex.StackTrace);
                writer.WriteLine("--------------------------------------------------");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Não foi possível salvar o erro no arquivo. Detalhes: " + e.Message);
        }
    }
    
}