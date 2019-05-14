using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Person : ScriptableObject
{
    public new string name;
    public string email;
    public DateTime date;
    public string password;
    public string cpassword;
    public int pin;
    public int cpin;
    public string gender;

    public int person_id;

    public int id;


    

    public bool isRegistered
    {
        get
        {
            return id >= 0;
        }
    }

   
    /// <summary>
    /// Creates a migration for playstats
    /// </summary>
    /// <param name="create"></param>
    /// Create or Update data
    /// <returns></returns>
    public Migration CreateMigration(bool create = true)
    {
        Migration migration = new Migration();
        migration.request_object = this;
        migration.json = JsonUtility.ToJson(this);
        migration.json = "{\"dependant\":" + migration.json + "}";
        if (create)
        {
            migration.request = "POST";
            migration.request_adress = "localhost:3000/dependants";
        }
        else
        {
            migration.request = "PUT";
            migration.request_adress = "localhost:3000/dependants/" + id.ToString();

        }
        migration.Save();

        return migration;
    }
}
