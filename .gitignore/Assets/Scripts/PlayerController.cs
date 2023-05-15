using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class PlayerController : NetworkBehaviour 
{
#region (Parametrs)
    #region (Strings)
    [SerializeField] private string namePlayer = "New Player";
    #endregion
    #region (TextTMPro)
    private TextMeshPro namePlayerText;
    private TextMeshPro scopeText;
    [SerializeField] private TMP_InputField inputFieldNamePlayer;
    #endregion
    #region (Component parametrs)
    Rigidbody rg;
    CapsuleCollider cc;
    [SerializeField] private UIManager UIM;
    private NetMan netMan; 
    #endregion
    #region (GameObjects)
    public GameObject Tex;
    public GameObject Camera;
    #endregion
    #region (Vectors)
    Vector3 directionVector;
    #endregion
    #region (Floats)
    float speed = 10f;

    float TimeToJerk = 1f;
    float ReJerking = 0;
    public float Force;

    float ReImpulse = 0f;
    public float TimeFall;
    
    float ReFall = -1f;
    float RePlayering = 0;
    #endregion
    #region (Ints)
    public int Scope;
    public int ScopeWin = 3;
    #endregion
#endregion
#region (Voids)
    #region (Default voids)
    void Start()
    {
        if(!hasAuthority)
        {
            Camera.SetActive(false);
        }
        if(hasAuthority)
        {
            transform.position = new Vector3 (Random.Range(-18f,18f),0.3f ,Random.Range(-18f,18f));
        }
        rg = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        netMan = GameObject.FindObjectOfType<NetMan>();
        inputFieldNamePlayer = GameObject.FindObjectOfType<TMP_InputField>();
        namePlayerText = transform.GetChild(0).GetComponent<TextMeshPro>();
        scopeText = transform.GetChild(1).GetComponent<TextMeshPro>();
        UIM = GameObject.FindObjectOfType<UIManager>();
        Tex.tag ="Player";
        if(isClient && isLocalPlayer)
        {
           SetInputManager();
        }
    }
    void Update()
    {
        if(UIM.stop == false)
        {  
            namePlayerText.text = namePlayer;
            scopeText.text = Scope.ToString();
            if(hasAuthority)
            {
                UIM.ScopeInt = Scope;
                Move();
                Jerk();
                ScopeMaft();
            }
        Falls();
        Win();
        }
        if(UIM.n-0.1f <= 0 && UIM.stop == true)
        {
                Scope = 0;
                transform.position = new Vector3 (Random.Range(-18f,18f),0.3f ,Random.Range(-18f,18f));
            }
    }
    #endregion
    #region (Options voids)
        #region (Local voids)
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        speed = 5f* Time.deltaTime;
        transform.Translate(new Vector3(h * speed,0,v * speed));
    }
    void Jerk()
    {
        float ImpulseTime = 0.5f;
        if(Input.GetMouseButton(0)&&ReJerking<=0)
        {
            if(Input.GetKey(KeyCode.W))
            {
                directionVector = transform.forward;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                directionVector = -transform.forward;
            }
            else if(Input.GetKey(KeyCode.A))
            {
                directionVector = transform.right;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                directionVector = -transform.right;
            }      
            rg.AddForce(directionVector*Force,ForceMode.Impulse);
            ReJerking = TimeToJerk;
            ReImpulse = ImpulseTime;
        }
        else
        {
            ReJerking -= Time.deltaTime;
            ReImpulse -= Time.deltaTime;
        }
        if(ReImpulse>0)
        {
            cc.height = Force*2/3f;
            cc.center = new Vector3(0,1.16f,-Force/3f);
        }
        else
        {
            cc.height = 0f;
            cc.center = new Vector3(0,1.16f,0f);
        }
    }
    void Falls()
    {
        if(ReFall < 0 && tag == "FallPlayer")
        {
            Tex.tag = "FallPlayer";
            ReFall = TimeFall + 0.2f;
            RePlayering = TimeFall;
        }
        else if( ReFall > 0 && RePlayering < 0 && tag == "FallPlayer")
        {
           tag = "Player";
           Tex.tag = "Player";
        }
        else
        {
            ReFall -= Time.deltaTime;
            RePlayering -= Time.deltaTime;
        }
    }
    void OnTriggerEnter(Collider other) 
    {
        if(hasAuthority)
        {
            if(other.tag == "Player" && ReImpulse > 0 )
            {
                Scope++;
                other.tag = "FallPlayer";
            }
        }
        
    }
    #endregion
        #region (Net voids)
    public void SetInputManager()
    {
        netMan.SetPlayer(this);
    }
    public void NewName()
    {
        if(hasAuthority)
        {
            if(isServer)
            {
                ChangeName(inputFieldNamePlayer.text);
                inputFieldNamePlayer.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                CmdChangeName(inputFieldNamePlayer.text);
                inputFieldNamePlayer.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    public void ITag()
    {
        if(hasAuthority)
        {
            if(isServer)
            {
                ChangeTag(tag);
            }
            else
            {
                CmdChangeTag(tag);
            }
        }
    }
    public void ScopeMaft()
    {
            if(isServer)
            {
                ChangeScope(Scope);

            }
            else
            {
                CmdChangeScope(Scope);      
            }
    }
    public void Win()
    {
        if(Scope >=ScopeWin)
        {
            UIM.SetPlayerNameWinner(namePlayer);
        }  
    }
    #endregion
    #endregion
#endregion
#region (Syncing)
    #region (Scope)    
    [SyncVar(hook = nameof(SyncScope))]
    int _SyncScope;

    void SyncScope(int oldValue, int newValue)
    {
        Scope = newValue;
    }

    [Server]

    public void ChangeScope(int newValue)
    {
        _SyncScope = newValue;
    }

    [Command]

    public void CmdChangeScope(int newValue)
    {
        ChangeScope(newValue);
    }
    #endregion
    #region (Name)
    [SyncVar(hook = nameof(SyncName))]
    string _SyncName;

    void SyncName(string oldValue, string newValue)
    {
        namePlayer = newValue;
    }
    [Server]
    public void ChangeName(string newValue)
    {
        _SyncName = newValue;
    }

    [Command]
    public void CmdChangeName(string newValue)
    {
        ChangeName(newValue);
    }
    #endregion
    #region (Tag)
    [SyncVar(hook = nameof(SyncTag))]
    string _SyncTag;

    void SyncTag(string oldValue, string newValue)
    {
        tag = newValue;
    }
    [Server]
    public void ChangeTag(string newValue)
    {
        _SyncName = newValue;
    }

    [Command]
    public void CmdChangeTag(string newValue)
    {
        ChangeTag(newValue);
    }
    #endregion
#endregion
}
