using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Migration {

    /// <summary>
    /// Element the should be sent to the PlayStats API
    /// </summary>
    public string json;

    public UnityEngine.Object request_object;

    /// <summary>
    /// PUT GET POST DEL
    /// </summary>
    public string request;
    
    /// <summary>
    /// URL that will be sent
    /// </summary>
    public string request_adress;


    /// <summary>
    /// Callback that should be called when the migration is succedded. Use it to store session variables
    /// </summary>
    public void OnMigrationSucceded()
    {
        PlayStatsMigrationHandler.instance.migrationsToGo.Remove(this);
    }

    /// <summary>
    /// Callback that should be called when the migration fails. Use it correct stuff
    /// </summary>
    public void OnMigrationFail()
    {

    }

    //TODO check json succed or fail
    /// <summary>
    /// Called when the PlayStats Migration Handler ends the migration
    /// </summary>
    /// <param name="json"></param>
    public void OnMigrationEnd(string json,bool error)
    {
        if(!error)
        {
            JsonUtility.FromJsonOverwrite(json, request_object);
            OnMigrationSucceded();
        }
        else
        {
            Debug.Log(json);
            OnMigrationFail();
        }
    }

    //TODO persist migration
    /// <summary>
    /// Saves the migration to be used by Playstats Migration
    /// </summary>
    public void Save()
    {
        PlayStatsMigrationHandler.instance.migrationsToGo.Add(this);
    }

    public override string ToString()
    {
        return request + " " + request_adress + "\n" + json;
    }

}
