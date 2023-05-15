using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallcolour : MonoBehaviour
{
#region (Parametrs)
    #region (Textures)
    public Texture Fall;
    public Texture Defaute;
    #endregion
    #region (Component parametrs)
    Renderer rd;
    #endregion
#endregion
#region (Voids)
    #region (Default voids)
    void Start()
    {
        rd = GetComponent<Renderer>();
    }
    void Update()
    {
        FallPlayer();
    }
    #endregion
    #region (Option voids)
    void FallPlayer()
    {
        if(tag == "FallPlayer")
        {
            rd.material.SetTexture("_MainTex", Fall);
        } 
        else if(tag == "Player")
        {
            rd.material.SetTexture("_MainTex", Defaute);
        }
    }
    #endregion
#endregion
}
