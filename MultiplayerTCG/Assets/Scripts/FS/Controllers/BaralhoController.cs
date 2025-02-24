
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;


public class BaralhoController : NewConnectDB
{
    public void CreateBaralho(Baralho baralho, out Baralho baralhoRetorno)
    {
        string sql = "INSERT INTO BARALHO (NOME,ATIVO, JOGADOR) " +
                     "VALUES (@NOME,@ATIVO, @JOGADOR)";

        cmd = new SqlCommand(sql, conectar());
        par = new SqlParameter("@NOME", baralho.nome);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ATIVO", 1);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@JOGADOR", baralho.jogador);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);

        
        reader = cmd.ExecuteReader();

        List<Baralho> baralhos = GetBaralhos(baralho.jogador);
        baralhoRetorno = baralhos[0];
    }

    public List<Baralho> GetBaralhos(int PlayerID)
    {
        List<Baralho> baralhos = new List<Baralho>();
        string sql = "SELECT COD, NOME, JOGADOR FROM BARALHO WHERE JOGADOR=@ID AND ATIVO=@ATIVO";
        SqlCommand cmd = new SqlCommand(sql, conectar());

        SqlParameter par = new SqlParameter("@ID", PlayerID);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ATIVO", 1);
        par.SqlDbType = System.Data.SqlDbType.Char;
        cmd.Parameters.Add(par);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Baralho baralho = new Baralho();
            baralho.cod = Int32.Parse(reader.GetValue(reader.GetOrdinal("COD")).ToString());
            baralho.nome = reader["NOME"].ToString();
            baralho.jogador = Convert.ToInt32(reader["JOGADOR"]);
            baralhos.Add(baralho);
        }

        reader.Close();
        return baralhos;
    }
}
