using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BetaLoader : MonoBehaviour
{
    public static string key = "";

    [SerializeField] private Text textId;
    [SerializeField] private Button skopiruj;
    [SerializeField] private Button potvrd;
    [SerializeField] private InputField input;

    // Start is called before the first frame update
    void Start()
    {
        var prefs = PlayerPrefs.GetString("wawrfkrapodsopa", "0_");
        var prefsRes = PlayerPrefs.GetString("wawrfkrapaxa", "0_");

        if (prefs == "0_")
        {
            var hwid = SystemInfo.deviceUniqueIdentifier;
            hwid = hwid.ToUpper();
            hwid += Random.Range(10000, 99999);
            hwid = hwid.Replace("D", "X");
            hwid += "VLADKO";
            key = hwid;
            PlayerPrefs.SetString("wawrfkrapodsopa", hwid);
        }
        else
        {
            key = prefs;
            var hwid = SystemInfo.deviceUniqueIdentifier;
            hwid = hwid.ToUpper();
            hwid = hwid.Replace("D", "X");
            if (!prefs.StartsWith(hwid))
            {
                Debug.Log("kokot hack");
                Application.Quit();
            }
        }
        
        textId.text = key;
        Debug.Log(CreateMD5(CreateMD5("B79C3B0222C703A3X069AB4F029E84083BF2792532657VLADKO")));
        if(prefsRes != "0_") input.text = prefsRes;
        TryConfirm();
        
        potvrd.onClick.AddListener(() => TryConfirm(false));
        skopiruj.onClick.AddListener(() => GUIUtility.systemCopyBuffer = key);
    }

    public void TryConfirm(bool safe = true)
    {
        if (!key.Contains("VLADKO") && !safe)
        {
            Application.Quit();
        }
        
        var hwid = SystemInfo.deviceUniqueIdentifier;
        hwid = hwid.ToUpper();
        hwid = hwid.Replace("D", "X");
        if (!key.StartsWith(hwid))
        {
            Debug.Log("kokot hack");
            Application.Quit();
        }
        
        var res = CreateMD5(CreateMD5(key));

        if (input.text == res)
        {
            if (input.text.Length > 5 && res.Length == 32)
            {
                PlayerPrefs.SetString("wawrfkrapaxa", res);
                SceneManager.LoadScene(2);
            }
            else
            {
                if (!safe) Application.Quit();
            }
        }
        else
        {
            if (!safe) Application.Quit();
        }
    }
    
    public static string CreateMD5(string input)
    {
        using MD5 md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);
            
        StringBuilder sb = new StringBuilder();
            
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
