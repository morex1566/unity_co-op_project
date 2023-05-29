using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static readonly string Default = "Guest";
    public static string UserName = Default;
    public static string ID = Default;
    public static string Password = Default;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
