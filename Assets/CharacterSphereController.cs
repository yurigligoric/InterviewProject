using Services;
using Tools.Pool;
using UnityEngine;

public class CharacterSphereController : MonoBehaviour, IPoolInit, IPoolOnSpawn, IPoolOnDespawn
{
    [SerializeField] private Rigidbody characterRigidbody;
    [SerializeField] private float moveSpeed;
    
    // Update is called once per frame
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
        
        characterRigidbody.AddForce(netForce);
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
}
