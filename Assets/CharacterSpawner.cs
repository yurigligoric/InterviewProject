using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private string spherePoolItemName;

    // Start is called before the first frame update
    void Start()
    {
        ServiceManager.Instance.poolService.Spawn(spherePoolItemName, spawnPosition.position, Quaternion.identity);
    }
}
