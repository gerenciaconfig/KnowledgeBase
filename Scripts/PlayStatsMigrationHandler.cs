using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayStatsMigrationHandler : MonoBehaviour {


    /// <summary>
    /// Stored Session variables
    /// </summary>
    public Dictionary<string, int> Session;

    /// <summary>
    /// Migrations that should still go to Playtats
    /// </summary>
    public List<Migration> migrationsToGo;

    #region Singleton

    public static PlayStatsMigrationHandler instance;

    private void initiateSingleton()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region Predicates

    //TODO override it on Disney's playstats migration handler
    /// <summary>
    /// Checks if PlayStats is able to realize any migration
    /// </summary>
    private bool canMigrate{

        get
        {
            return true;
        }
    }

    /// <summary>
    /// Returns if Playstats has given a token
    /// </summary>
    public bool hasToken
    {
        get
        {
            return string.IsNullOrEmpty(token) == false;
        }
    }

    /// <summary>
    /// Playstats Acess Token
    /// </summary>
    public string token;

    /// <summary>
    /// Checks if PlayStats has Migrations to Realize
    /// </summary>
    private bool hasMigrations
    {
        get
        {
            return migrationsToGo.Count > 0;
        }
    }

    /// <summary>
    /// override this to say if PlayStats should Migrate
    /// </summary>
    protected virtual bool shouldMigrate
    {
        get
        {
            return hasMigrations && canMigrate;
        }
    }

    #endregion

    #region MonoBehaviour

    public IEnumerator Start()
    {
        initiateSingleton();

        yield return null;
        DeserializeSession();
        DeserializeMigrations();
        while(true)
        {
            if (shouldMigrate) {
                yield return StartCoroutine(Migrate(migrationsToGo[0]));
            }
            yield return null;
        }
    }

    #endregion

    //TODO Get Migration from persistence
    /// <summary>
    /// Gets all migrations from persistence and puts it in migrations to go list
    /// </summary>
    private void DeserializeMigrations()
    {
        migrationsToGo = new List<Migration>();
    }

    //TODO Get data from persistence
    /// <summary>
    /// Starts the Session variables stored in the persistence
    /// </summary>
    private void DeserializeSession()
    {

    }
    
    //TODO
    /// <summary>
    /// Stores the session variable and id in the persistence and into the Dictionary
    /// </summary>
    public void UpdateSession(string sessionKey,string object_id)
    {

    }


    /// <summary>
    /// Request API to receive JSON 
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator RequestServerMigration(Migration migration)
    {
        using (var www = new UnityWebRequest(migration.request_adress, migration.request))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(migration.json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            if(hasToken)www.SetRequestHeader("authorization", token);
            yield return www.SendWebRequest();
            token = www.GetResponseHeader("authorization");
            migration.OnMigrationEnd(www.downloadHandler.text, www.isNetworkError || www.isHttpError);
        }
    }

    /// <summary>
    /// Realizes the migration for PlayStats API
    /// </summary>
    /// <param name="migration"></param>
    private IEnumerator Migrate(Migration migration)
    {
        SessionUpdateMigration(migration);
        yield return StartCoroutine(RequestServerMigration(migration));
    }

    //TODO change session variables to object id in migration's strings
    /// <summary>
    /// Change the migration with the session fields updated;
    /// </summary>
    private void SessionUpdateMigration(Migration migration)
    {
        
    }
}
