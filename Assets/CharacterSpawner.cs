using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private string spherePoolItemName;
    
    private GameObject _spawnedObj;
    public GameObject SpawnedSphereObj => _spawnedObj;
    
    // Start is called before the first frame update
    void Start()
    { 
        _spawnedObj = ServiceManager.Instance.poolService.Spawn(spherePoolItemName, spawnPosition.position, Quaternion.identity);
    }

    private void Update()
    {
        //Despawn
        if (Input.GetKeyDown(KeyCode.Q) && _spawnedObj != null)
        {
            ServiceManager.Instance.poolService.Despawn(_spawnedObj);
            _spawnedObj = null;
        }
        
        //spawn
        if (Input.GetKeyDown(KeyCode.E) && _spawnedObj == null)
        {
            _spawnedObj = ServiceManager.Instance.poolService.Spawn(spherePoolItemName, spawnPosition.position, Quaternion.identity);
        }
    }
}
