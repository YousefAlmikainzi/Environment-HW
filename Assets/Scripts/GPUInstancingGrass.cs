using UnityEngine;

public class GPUInstancingGrass : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int instances = 10;
    [SerializeField] float radius = 1.0f;
    [SerializeField] float YAxis = 1.0f;

    void Start()
    {
        Random.InitState(10);
        for (int i = 0; i < instances; i++)
        {
            GameObject instance = Instantiate(prefab, transform);
            float randomXPos = Random.Range(-radius, radius);
            float randomZPos = Random.Range(-radius, radius);

            instance.transform.localPosition = new Vector3(randomXPos, YAxis, randomZPos);
            instance.transform.SetParent(transform, true);
        }
    }
}
