using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SpawnAt : MonoBehaviour
{
    [SerializeField] Terrain ourTerrain;
    [SerializeField] GameObject spawnThis;
    [SerializeField] float yOffset = 0f;

    void Start()
    {
        spawnObject(0.0f,0.0f);
    }
    private void spawnObject(float x, float z)
    {
        Vector3 objPos = new Vector3(x, 0.0f, z);
        float yPos = ourTerrain.SampleHeight(objPos) + ourTerrain.transform.position.y;
        objPos.y = yPos + yOffset;
        Instantiate(spawnThis, objPos, Quaternion.identity);
    }
}
