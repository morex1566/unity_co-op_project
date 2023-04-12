using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    public void OnPauseTimer();
    public void OnResumeTimer();
    public void OnStartTimerFor(float time);
    public float OnGetTime();
 
    public void AddGameObjectsToLevel(List<GameObject> objs);
    public void AddGameObjectToLevel(GameObject obj);
    public GameObject GetLevel();
    Vector3 GetCenterPointAtLevel();
}

public interface ILevelPlatformSpawner : ILevel
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

public interface ILevelObstacleSpawner : ILevel
{
    GameObject FragileObstaclePrefab { get; }
    GameObject StaticObstaclePrefab { get; }
    float SyncSpeed { get; }
    float PlatformWidth { get; }
    float FragileObstacleSize { get; }
    float StaticObstacleSize { get; }
    float LevelStartPos { get; }

}

public interface ILevelManager : ILevel
{
    
}