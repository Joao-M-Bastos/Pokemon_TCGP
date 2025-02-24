using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorVar : MonoBehaviour
{
    public static JogadorVar varInstance;

    public Jogador jogador;
    public Baralho baralho;
    public List<Carta> cartas;
    
    void Awake()
    {
        if(varInstance == null)
            varInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DefineJogador(Jogador jogador){
        this.jogador = jogador;
    }

    public int JogadorID()
    {
        return jogador.cod;
    }

    public void DefineBaralho(Baralho baralho)
    {
        this.baralho = baralho;
    }

    public void DefineCartas(List<Carta> cartas)
    {
        this.cartas = cartas;
    }

}
