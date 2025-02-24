
using UnityEngine;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;


public class CartaController : NewConnectDB
{
    
    
    public void CreateCarta(Carta carta)
    {
        string sqlColluns = " NOME, " +
                            " IMAGEM, " +
                            " HP," +
                            " ATAQUE_NOME," +
                            " ATAQUE_CUSTO," +
                            " ATAQUE_DANO," +
                            " ATAQUE_EFEITO," +
                            " RECUO," +
                            " EFEITO," +
                            " ESTAGIO," +
                            " TIPO";
        
        string sqlValues =  "@NOME, " +
                            "@IMAGEM, " +
                            "@HP," +
                            "@ATAQUE_NOME," +
                            "@ATAQUE_CUSTO," +
                            "@ATAQUE_DANO," +
                            "@ATAQUE_EFEITO," +
                            "@RECUO," +
                            "@EFEITO," +
                            "@ESTAGIO," +
                            "@TIPO";
        
        string sql = $"INSERT INTO CARTA ({sqlColluns}) " +
                     $"VALUES ({sqlValues})";

        cmd = new SqlCommand(sql, conectar());
        
        par = new SqlParameter("@NOME", carta.nome);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@IMAGEM", carta.imagem);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@HP", carta.hp);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ATAQUE_NOME", carta.ataqueNome);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ATAQUE_CUSTO", carta.ataqueCusto);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ATAQUE_DANO", carta.ataqueDano);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ATAQUE_EFEITO", carta.ataqueEfeito);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@RECUO", carta.recuo);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@EFEITO", carta.efeito);
        par.SqlDbType = System.Data.SqlDbType.VarChar;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@ESTAGIO", carta.estagio);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);
        par = new SqlParameter("@TIPO", carta.tipo);
        par.SqlDbType = System.Data.SqlDbType.Int;
        cmd.Parameters.Add(par);


        
        reader = cmd.ExecuteReader();
        
    }
    
    
    public List<Carta> GetCartas(int baralhoCod){
        List<Carta> cartas = new List<Carta>();
        string sqlColluns = " C.COD, " +
                            " C.NOME, " +
                            " C.IMAGEM, " +
                            " C.HP," +
                            " C.ATAQUE_NOME," +
                            " C.ATAQUE_CUSTO," +
                            " C.ATAQUE_DANO," +
                            " C.ATAQUE_EFEITO," +
                            " C.RECUO," +
                            " C.EFEITO," +
                            " C.ESTAGIO," +
                            " C.TIPO," +
                            " I.COD as COD_ITEM," +
                            " T.NOME AS NOME_TIPO";
        
        string sql = $"SELECT {sqlColluns} FROM CARTA AS C JOIN ITEM I ON I.CARTA = C.COD JOIN BARALHO B ON I.BARALHO = B.COD JOIN TIPO T ON T.COD = C.TIPO WHERE B.COD = @BARALHOCOD";
        
        cmd = new SqlCommand(sql, conectar());
        
        par = new SqlParameter("@BARALHOCOD", baralhoCod);
        par.SqlDbType = SqlDbType.Int;
        cmd.Parameters.Add(par);

        SqlDataReader reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            Carta carta = new Carta();
            
            carta.cod = Convert.ToInt32(reader["COD"]);

            carta.itemCod = Convert.ToInt32(reader["COD_ITEM"]);

            carta.nome = reader["NOME"].ToString();
            carta.imagem = reader["IMAGEM"].ToString();
            carta.hp = Convert.ToInt32(reader["HP"]);
            carta.ataqueNome = reader["ATAQUE_NOME"].ToString();
            carta.ataqueCusto = Convert.ToInt32(reader["ATAQUE_CUSTO"]);
            carta.ataqueDano = Convert.ToInt32(reader["ATAQUE_DANO"]);
            carta.ataqueEfeito = reader["ATAQUE_EFEITO"].ToString();
            carta.recuo = Convert.ToInt32(reader["RECUO"]);
            carta.efeito = reader["EFEITO"].ToString();
            carta.estagio = Convert.ToInt32(reader["ESTAGIO"]);
            carta.tipo = Convert.ToInt32(reader["TIPO"]);
            
            cartas.Add(carta);
        }

        reader.Close();
        return cartas;
    } 

    public Carta GetCard(int cardcod)
    {
        string sqlColluns = " COD, " +
                            " NOME, " +
                            " IMAGEM, " +
                            " HP," +
                            " ATAQUE_NOME," +
                            " ATAQUE_CUSTO," +
                            " ATAQUE_DANO," +
                            " ATAQUE_EFEITO," +
                            " RECUO," +
                            " EFEITO," +
                            " ESTAGIO," +
                            " TIPO";

        string sql = $"SELECT {sqlColluns} FROM CARTA WHERE COD = @CARTACOD";

        cmd = new SqlCommand(sql, conectar());

        par = new SqlParameter("@CARTACOD", cardcod);
        par.SqlDbType = SqlDbType.Int;
        cmd.Parameters.Add(par);

        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Carta carta = new Carta();

            carta.cod = Convert.ToInt32(reader["COD"]);
            carta.nome = reader["NOME"].ToString();
            carta.imagem = reader["IMAGEM"].ToString();
            carta.hp = Convert.ToInt32(reader["HP"]);
            carta.ataqueNome = reader["ATAQUE_NOME"].ToString();
            carta.ataqueCusto = Convert.ToInt32(reader["ATAQUE_CUSTO"]);
            carta.ataqueDano = Convert.ToInt32(reader["ATAQUE_DANO"]);
            carta.ataqueEfeito = reader["ATAQUE_EFEITO"].ToString();
            carta.recuo = Convert.ToInt32(reader["RECUO"]);
            carta.efeito = reader["EFEITO"].ToString();
            carta.estagio = Convert.ToInt32(reader["ESTAGIO"]);
            carta.tipo = Convert.ToInt32(reader["TIPO"]);

            return carta;
        }

        
        return null;
    }
    
}
