using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyBoard : MonoBehaviour
{
    [SerializeField] EnemySlots[] slots;

    [SerializeField] PokemonData data;

    private CartaController _cardController;

    private void Awake()
    {
        _cardController = new CartaController();
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotID(i);
        }
    }

    public void PutPokemon(int boardID, int pokemonID)
    {

        try
        {
            Carta newCarta = _cardController.GetCard(pokemonID);


            data.MaxLife = newCarta.hp;
            data.name = newCarta.nome;

            slots[boardID].AddPokemonToSlot(data);

            SetLife(boardID, data.MaxLife);
            SetEnergy(boardID, 0);
            SetName(boardID, data.name);
        }
        catch (Exception e)
        {

            Debug.Log($"Carta não encontrada! ERRO: {e.Message}");

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

    public void RemovePokemonFromSlot(int slotID)
    {
        slots[slotID].RemovePokemon();
    }

    public void SetLife(int slot, int i)
    {
        slots[slot].UpdateTextLife(i);
    }

    public void SetEnergy(int slot, int i)
    {
        slots[slot].UpdateTextEnergy(i);
    }

    public void SetName(int slot, string name)
    {
        slots[slot].UpdateTextName(name);
    }
}
