using System;
using System.Text;
// using System.Data.SqlClient;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
// using System.Md5;



public class NewConnectDB
{
    public SqlParameter par;
    public SqlCommand cmd;
    public SqlDataReader reader;

    
    public SqlConnection conectar()
    {
        try
        {
            // Debug.Log("Conectado!!!!!!!!!!!!!!!!!!!!!");
            // byte[] conexao = Encoding.UTF8.GetBytes("Data Source=localhost;" +
            //                  "Initial Catalog=POKEMON_2024_02;" +
            //                  "User ID=sa;" +
            //                  "Password=SuaSenhaForte123!");
            //
            // SqlConnection conn = new SqlConnection(Encoding.GetEncoding(1252).GetString(conexao));

            Debug.Log("Conectado!!!!!!!!!!!!!!!!!!!!!");
            String conexao = "Data Source=127.0.0.1;" +
                             "Initial Catalog=POKEMON_2024_02;" +
                             "User ID=sa;" +
                             "Password=SuaSenhaForte123!;";
            
            SqlConnection conn = new SqlConnection(ConverToUTF8(conexao));

            conn.Open();
            
            return conn;
        }catch(Exception ex)
        {
            Debug.LogError("Erro ao conectar ao banco de dados: " + ex.Message);
            string filePath = "logfile.txt";
            SaveErrorToFile(ex, filePath);
        }
        return null;
    }


    public string ConverToUTF8(string input){
        Encoding windows1252 = Encoding.GetEncoding(1252);
        Encoding utf8 = Encoding.UTF8;
        byte[] windows1252Bytes = windows1252.GetBytes(input);
        byte[] utf8Bytes = Encoding.Convert(windows1252, utf8, windows1252Bytes);
        
        return utf8.GetString(utf8Bytes);
    }


    public static string HashMd5(string input){
        MD5 md5Hash = MD5.Create();

        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();

    }
    
    private void SaveErrorToFile(Exception ex, string filePath)
    {
        try
        {
            // Criar ou abrir o arquivo para escrita
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Data e Hora: " + DateTime.Now.ToString());
                writer.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
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
