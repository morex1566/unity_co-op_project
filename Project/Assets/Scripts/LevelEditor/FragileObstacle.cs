using UnityEngine;

public class FragileObstacle : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private IGameManagerPlatformSpawner _gameManager;

    private float _mapSpeed;

    [SerializeField] Transform arrow;
    [SerializeField] Material leftMat;
    int randomNum;
    public string direction;


    // Start is called before the first frame update
    void Start()
    {
        randomNum = Random.Range(0, 20);
        if (randomNum <10)
            direction = "Right";
        else
        {
            direction = "Left";
            arrow.GetComponent<MeshRenderer>().material = leftMat;
        }
        _gameManager = GameManager.Instance as IGameManagerPlatformSpawner;
        _mapSpeed = _gameManager.MapSpeed;
        
        Debug.Log(direction);
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
