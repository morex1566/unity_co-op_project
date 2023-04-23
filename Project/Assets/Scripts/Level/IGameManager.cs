using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    public void OnPauseTimer();
    public void OnResumeTimer();
    public void OnStartTimerFor(TimePer unit, float time);
    public float OnGetTime(TimePer unit);
 
    public void AddGameObjectsToLevel(List<GameObject> objs);
    public void AddGameObjectToLevel(GameObject obj);
    public GameObject GetLevel();
    public Vector3 GetCenterPointAtLevel();
    MapData MapData { get; }
    public float GetDistance();
}

public interface IGameManagerPlatformSpawner : IGameManager
{
    // INFO 사용되는 Prefab들...
    GameObject PlatformPrefab { get; }
    float MapSpeed { get; }
    float PlatformWidth { get; }
    float PlatformLength { get; }
    float PlatformHeight { get; }
    float LevelStartPos { get; }
    int PlatformLayerCount { get; }
}

public interface IGameManagerObstacleSpawner : IGameManager
{
    GameObject FragileObstaclePrefab { get; }
    GameObject StaticObstaclePrefab { get; }
    float MapSpeed { get; }
    float SyncSpeed { get; }
    float PlatformWidth { get; }
    float FragileObstacleSize { get; }
    float StaticObstacleSize { get; }
    float LevelStartPos { get; }

}