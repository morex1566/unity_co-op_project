using UnityEngine;
using UnityEngine.UI;

public class LevelTimeline : MonoBehaviour
{
    [SerializeField] private Slider timeline;

    private void Start()
    {
        setTimelineLimit();
    }

    // Update is called once per frame
    void Update()
    {
        timeline.value += Time.deltaTime;
    }

    private void setTimelineLimit()
    {
        float lastTime = GameManager.Instance._levelObstacleSpawner.LastObstacleTime() / 10f;

        timeline.maxValue = lastTime;
    }
}
