using UnityEngine;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

public class JogadaController : NewConnectDB
{
    public Jogada CreateJogada(Jogada jogada)
    {
        string subsql = "SELECT COD FROM ITEM WHERE COD = @ITEM";

        string sql = "INSERT INTO JOGADA (ITEM,BARALHO,CARTA,PARTIDA,LOCAL_CARTA,DANO,ENERGIA) " +
                     $"VALUES (({subsql}), @BARALHO, @CARTA, @PARTIDA, @LOCAL_CARTA, @DANO, @ENERGIA);" +
                     "SELECT SCOPE_IDENTITY() AS COD; ";
        
        cmd = new SqlCommand(sql, conectar());

        

        par = new SqlParameter("@ITEM", jogada.item);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@BARALHO", jogada.baralho);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@CARTA", jogada.carta);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@PARTIDA", jogada.partida);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@LOCAL_CARTA", jogada.local_carta);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@DANO", jogada.dano);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ENERGIA", jogada.energia);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        
        Debug.Log(sql);

        Debug.Log(jogada.partida);

        reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            // Jogada jogada = new Jogada();

            jogada.cod = Int32.Parse(reader.GetValue(reader.GetOrdinal("COD")).ToString());
            return jogada;
        }
        return null;

    }
}
