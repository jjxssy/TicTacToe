using UnityEngine;
using TMPro;

public class TextWave : MonoBehaviour
{
    public TMP_Text textMesh;
    public float waveSpeed = 2f;
    public float waveHeight = 10f;
    public float waveFrequency = 0.01f; // how much x affects the wave

    private TMP_TextInfo textInfo;
    private Vector3[][] baseVertices; // cached original vertices per mesh

    void Awake()
    {
        if (!textMesh) textMesh = GetComponent<TMP_Text>();
    }

    void LateUpdate()
    {
        if (!textMesh) return;

        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        // Cache base vertices (fresh after mesh rebuilds)
        if (baseVertices == null || baseVertices.Length != textInfo.meshInfo.Length)
        {
            baseVertices = new Vector3[textInfo.meshInfo.Length][];
        }

        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            var src = textInfo.meshInfo[m].vertices;
            if (src == null) continue;

            if (baseVertices[m] == null || baseVertices[m].Length != src.Length)
                baseVertices[m] = new Vector3[src.Length];

            System.Array.Copy(src, baseVertices[m], src.Length);
        }

        // Apply wave relative to base vertices
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;

            var verts = textInfo.meshInfo[matIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var basePos = baseVertices[matIndex][vertIndex + j];
                float wave = Mathf.Sin(Time.time * waveSpeed + basePos.x * waveFrequency) * waveHeight;
                verts[vertIndex + j] = basePos + new Vector3(0, wave, 0);
            }
        }

        // Push updated geometry back to TMP
        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            textInfo.meshInfo[m].mesh.vertices = textInfo.meshInfo[m].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[m].mesh, m);
        }
    }
}
