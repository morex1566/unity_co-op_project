using UnityEngine;
using System.Collections;

public static class CoroutineUtility
{
    private static MonoBehaviour _coroutineHost;

    public static void Initialize()
    {
        GameObject hostObject = new GameObject("UserCoroutine");
        _coroutineHost = hostObject.AddComponent<UserCoroutine>();
        GameObject.DontDestroyOnLoad(hostObject);
    }

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return _coroutineHost.StartCoroutine(coroutine);
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
        _coroutineHost.StopCoroutine(coroutine);
    }

    private class UserCoroutine : MonoBehaviour { }
}