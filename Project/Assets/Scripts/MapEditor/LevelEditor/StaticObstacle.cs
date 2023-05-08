using UnityEngine;

public class StaticObstacle : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private IGameManagerPlatformSpawner _gameManager;

    private float _mapSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance as IGameManagerPlatformSpawner;
        _mapSpeed = _gameManager.MapSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = transform.position;
        nextPos.z += _mapSpeed * Time.deltaTime;

        transform.position = nextPos;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
