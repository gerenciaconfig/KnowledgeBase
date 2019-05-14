using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayStatsForm : MonoBehaviour {

    public TMP_Text tName;
    public TMP_Text tEmail;
    public TMP_Text tDate;
    public Toggle tGenderM;
    public Toggle tGenderF;   
    public TMP_Text tPassword;
    public TMP_Text tConfirmPassword;
    public TMP_Text tPin;
    public TMP_Text tConfirmPin;

    private string Gender;
    private DateTime date;


    public TMP_Text cName;
    public TMP_Text cDate;
    public Toggle cGenderM;
    public Toggle cGenderF;

    public Person personData;
    

    private string convertEmptyStringToNull(string content)
    {
        if (string.IsNullOrEmpty(content))
            return null;
        return content;
    }

    public void CreateElement()
    {     
        //Store common variables on object
        personData.name = convertEmptyStringToNull(tName.text);
        personData.email = convertEmptyStringToNull(tEmail.text);
        personData.password = convertEmptyStringToNull(tPassword.text);
        personData.cpassword = convertEmptyStringToNull(tConfirmPassword.text);
        personData.gender = convertEmptyStringToNull(Gender);
        personData.pin = int.Parse(convertEmptyStringToNull(tPin.text));
        personData.cpin = int.Parse(convertEmptyStringToNull(tConfirmPin.text));
       //personData.date = DateTime.Parse(convertEmptyStringToNull(tDate.text));

        //Migrate
        Migration m = personData.CreateMigration(!personData.isRegistered);

        //Saves migration
        m.Save();
    }

    public void CheckGender() {
        if (tGenderM.isOn == true)
        {
            Gender = "M";
        }if(tGenderF.isOn == true)
        {
            Gender = "F";
        }

    }

}
