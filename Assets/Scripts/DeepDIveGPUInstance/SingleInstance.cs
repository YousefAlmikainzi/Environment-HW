using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InstancedCrossGrass : MonoBehaviour
{
    [SerializeField] Mesh mesh;              // quad mesh
    [SerializeField] Material material;      // VertexGrassMovement material

    [SerializeField] int instances = 1000;   // requested clumps
    [SerializeField] float radius = 10.0f;
    [SerializeField] float yAxis = 0.0f;
    [SerializeField] Vector3 scale = Vector3.one;

    [SerializeField] float minDistance = 1.0f;          // min distance between clump centers
    [SerializeField] int maxAttemptsPerInstance = 20;   // how hard we try before giving up

    Matrix4x4[] matricesA;
    Matrix4x4[] matricesB;
    int instanceCount; // actually placed count

    void OnEnable()
    {
        if (mesh == null || material == null)
        {
            Debug.LogError("Assign Mesh and Material on " + name);
            return;
        }

        material.enableInstancing = true;

        // positions we successfully placed
        List<Vector3> positions = new List<Vector3>();

        Random.InitState(10);

        // rotations for cross
        Quaternion rotA = Quaternion.identity;
        Quaternion rotB = Quaternion.Euler(0f, 90f, 0f);

        float minDistSq = minDistance * minDistance;

        for (int i = 0; i < instances; i++)
        {
            bool placed = false;

            for (int attempt = 0; attempt < maxAttemptsPerInstance; attempt++)
            {
                // random point in a circle
                Vector2 rand2D = Random.insideUnitCircle * radius;
                Vector3 candidate = new Vector3(rand2D.x, yAxis, rand2D.y);

                bool overlaps = false;

                // check against all previous positions (XZ only)
                for (int j = 0; j < positions.Count; j++)
                {
                    Vector3 p = positions[j];
                    float dx = candidate.x - p.x;
                    float dz = candidate.z - p.z;
                    float distSq = dx * dx + dz * dz;

                    if (distSq < minDistSq)
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (!overlaps)
                {
                    positions.Add(candidate);
                    placed = true;
                    break;
                }
            }

            // couldn’t place this one after N attempts → assume area is full enough
            if (!placed)
            {
                break;
            }
        }

        instanceCount = positions.Count;

        matricesA = new Matrix4x4[instanceCount];
        matricesB = new Matrix4x4[instanceCount];

        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 pos = positions[i];

            matricesA[i] = Matrix4x4.TRS(pos, rotA, scale);
            matricesB[i] = Matrix4x4.TRS(pos, rotB, scale);
        }
    }

    void Update()
    {
        if (mesh == null || material == null || matricesA == null || matricesB == null)
            return;

        if (instanceCount == 0)
            return;

        // first quad per clump (no shadows)
        Graphics.DrawMeshInstanced(
            mesh,
            0,
            material,
            matricesA,
            instanceCount,
            null,
            ShadowCastingMode.Off,
            false,
            0,
            null,
            LightProbeUsage.Off
        );

        // second quad rotated 90° (no shadows)
        Graphics.DrawMeshInstanced(
            mesh,
            0,
            material,
            matricesB,
            instanceCount,
            null,
            ShadowCastingMode.Off,
            false,
            0,
            null,
            LightProbeUsage.Off
        );
    }
}
