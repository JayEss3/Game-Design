using UnityEngine;
using MySql.Data.MySqlClient;
using System;

public static class Database
{
    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection("SERVER=kp-dev.cdrigqqp3tnk.us-east-1.rds.amazonaws.com;DATABASE=untitledFPS;UID=jnbKJASHDn89132n;PASSWORD=Zx7cg#1j$n9;");
    }
    public static bool AccountExists(string username)
    {
        var query = $"SELECT username FROM untitledFPS.accounts WHERE username='{username}';";
        var tempConn = GetConnection();
        int rowCount = 0;
        try
        {
            tempConn.Open();
            var comm = tempConn.CreateCommand();
            comm.CommandText = query;
            var reader = comm.ExecuteReader();
            while (reader.Read())
                rowCount++;
        }
        catch (Exception e)
        {
            Debug.LogError($"An Error as occured when attempting to check if username exists.\n{e}");
        }
        finally
        {
            tempConn.Close();
        }
        return rowCount != 0;
    }
    public static bool CorrectCredentials(string username, string password)
    {
        var query = $"SELECT username FROM untitledFPS.accounts WHERE username='{username}' AND password='{password}';";
        var tempConn = GetConnection();
        int rowCount = 0;
        try
        {
            tempConn.Open();
            var comm = tempConn.CreateCommand();
            comm.CommandText = query;
            var reader = comm.ExecuteReader();
            while (reader.Read())
                rowCount++;
        }
        catch (Exception e)
        {
            Debug.LogError($"An Error as occured when attempting to check if the credentials are correct.\n{e}");
        }
        finally
        {
            tempConn.Close();
        }
        return rowCount != 0;
    }
}
