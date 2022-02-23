using Services;

namespace Tools.Pool
{
    public interface IPoolInit
    {
        /// <summary>
        /// Only called once during preload
        /// </summary>
        void Init();
    }
    
    public interface IPoolOnSpawn
    {
        /// <summary>
        /// Called at the same frame of spawn
        /// </summary>
        /// <param name="poolService"></param>
        void OnSpawn(PoolService poolService);
    }

    public interface IPoolOnDespawn
    {
        /// <summary>
        /// Called at the same frame of despawn
        /// </summary>
        void OnDespawn();
    }

}