using Services;
using Tools.Pool;
using UnityEngine;

public class CharacterSphereController : MonoBehaviour, IPoolInit, IPoolOnSpawn, IPoolOnDespawn
{
    [SerializeField] private float moveSpeed;
    Rigidbody characterRigidbody;
    //public GameObject popUpBox;
    
    
    // Update is called once per frame

    void Awake()
    {

    
    }
    
    void Start()
    {
        characterRigidbody = GameObject.Find("CharacterSphere(Clone)").GetComponent<Rigidbody>();
    }


    void Update()
    {
        var netForce = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            netForce += (Vector3.forward * moveSpeed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            netForce += (Vector3.back * moveSpeed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            netForce += (Vector3.left * moveSpeed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            netForce += (Vector3.right * moveSpeed * Time.deltaTime);
        }
        
        if (Input.anyKey == false)
        {
            netForce = Vector3.zero;
        }

        if (characterRigidbody != null){
            characterRigidbody.AddForce(netForce);
        }
    }

    public void Init()
    {
        Debug.Log("Character Initialized");
    }

    public void OnSpawn(PoolService poolService)
    {
        Debug.Log("Character Spawned");
    }

    public void OnDespawn()
    {
        Debug.Log("Character Despawned");
    }

   
   
   
   void OnCollisionStay(Collision col)
    {
        
        
            PopUpSystem pop = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopUpSystem>();
            if(col.gameObject.tag== "wall1")
            {
                
                pop.PopUp(1);
            }
            if(col.gameObject.tag== "wall2")
            {
                
                pop.PopUp(2);
            }
            if(col.gameObject.tag== "wall3")
            {
                
                pop.PopUp(3);
            }
            if(col.gameObject.tag== "wall4")
            {
                        
                pop.PopUp(4);
            }
        

    }


    
}
