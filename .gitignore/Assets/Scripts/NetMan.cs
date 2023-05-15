using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMan : NetworkManager
{
#region (Parametrs)
    #region (Component parametrs)
    private PlayerController playerObj;
    #endregion
#endregion
#region (Voids)
    #region (Default voids)
    void Update()
    {
        playerObj.ScopeMaft();
    }
    #endregion
    #region (Option voids)
    public void SetPlayer(PlayerController pl)
    {
        playerObj = pl;
    }
    public void SetNamePlayer()
    {
        playerObj.NewName();
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
#endregion
}
