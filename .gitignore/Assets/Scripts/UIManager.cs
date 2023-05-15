using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
#region (Parametrs)
    #region (GameObject)
    [SerializeField] private GameObject panelWin;
    #endregion
    #region (TextTMPro)
    [SerializeField] private TextMeshProUGUI nameWiner;
    public TextMeshProUGUI Scope;
    #endregion
    #region (Ints)
    public int ScopeInt;
    #endregion
    #region (Floats)
    public float n = 5;
    #endregion
    #region (Bools)
    public bool stop = false;
    #endregion
#endregion
#region (Voids)
    #region (Default voids)
    void Update()
    {
        ScopeUse();
    }
    #endregion
    #region (Options voids)
    void ScopeUse()
    {
        Scope.text = ScopeInt.ToString();
        if(stop == true){
          n -= Time.deltaTime;
        }
        if(n <=0){
            panelWin.SetActive(false);
            stop = false;
            n=5f; 
        }
    }
    public void SetPlayerNameWinner(string NamePlayer)
    {
        nameWiner.text = NamePlayer;
        panelWin.SetActive(true);
        stop = true;

    }
    #endregion
#endregion
}
