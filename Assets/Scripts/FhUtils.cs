using UnityEngine;
using UnityEngine.SceneManagement;

public static class FhUtils
{
    #region dllimport

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();
#endif

    #endregion

    #region api

    public static void LevelRestartInLevel()
    {
        PlayerPrefs.SetInt("MenuTrigger_1", 1);
        SceneManager.LoadScene("Level");
    }
        
    public static bool IsOnMobile()
    {
        bool isMobile = true;
#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();
#endif
        return isMobile;
    }
        
    /// <summary>
    /// Convert money count in integer format to string. Example: 10000 = 10,000$
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="_Money">Money.</param>
    public static string MoneyString(int _Money)
    {
        return MoneyStringCore(_Money);
    }

    #endregion

    #region nonpublic methods

    private static string MoneyStringCore(int _Money)
    {
        string strNum1, strNum2;
        
        string moneyStr = _Money.ToString("D");
        static string Normalize(int _V) => _V.ToString("D");
        
        if (moneyStr.Length <= 3)
        {
            return moneyStr + "C";
        }
        if (moneyStr.Length > 3 && moneyStr.Length <= 6)
        {
            int num1 = Mathf.FloorToInt(_Money * 0.001f);
            int num2 = _Money - num1 * 1000;
        
            if (num2 < 10)
                strNum2 = "00" + num2.ToString("D");
            else if (num2 < 100)
                strNum2 = "0" + num2.ToString("D");
            else
                strNum2 = "" + num2.ToString("D");
        
            strNum1 = num1.ToString("D");
            return strNum1 + "," + strNum2 + "C";
        }
        if (moneyStr.Length > 6 && moneyStr.Length <= 9)
        {
            int num1 = Mathf.FloorToInt(_Money * 0.000001f );
            int num2 = _Money - num1 * 1000000;
            int num2_1 = Mathf.FloorToInt(num2 * 0.001f);
        
            if (num2_1 < 10)
                strNum2 = "00" + num2_1.ToString("D");
            else if (num2_1 < 100)
                strNum2 = "0" + num2_1.ToString("D");
            else
                strNum2 = "" + num2_1.ToString("D");
        
            int num3 = _Money - num1 * 1000000 - num2_1 * 1000;
        
            string strNum3;
            if (num3 < 10)
                strNum3 = "00" + num3.ToString("D");
            else if (num3 < 100)
                strNum3 = "0" + num3.ToString("D");
            else
                strNum3 = "" + num3.ToString("D");
        
            strNum1 = num1.ToString("D");
            return strNum1 + "," + strNum2 + "," + strNum3 + "C";
        }
        return "";
    }

    #endregion
}