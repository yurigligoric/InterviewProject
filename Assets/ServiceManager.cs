using Services;
using UnityEngine;

public class ServiceManager : MonoBehaviour
{
    public PoolService poolService;
    
    public static ServiceManager Instance;
    
    private void Awake()
    {
        Instance = this;
        poolService.PreloadAllPools();
    }
}
