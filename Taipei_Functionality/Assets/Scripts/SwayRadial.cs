using UnityEngine;

public class SwayRadial : MonoBehaviour
{
    public float m_swayFrequency;
    public float m_swayAmplitude;

    //public GameObject m_referenceObj;

    Mesh mesh;
    float swaytime;
    float r_min;
    float r_max;
    Vector3[] originalVert;

    void Start() {
        swaytime = 0f;

        mesh = GetComponent<MeshFilter>().mesh;
        originalVert = mesh.vertices;

        r_min = transform.TransformPoint(originalVert[0]).y;
        r_max = transform.TransformPoint(originalVert[0]).y;
        for(int i = 0; i < originalVert.Length; i++) {
            float radius = transform.TransformPoint(originalVert[i]).y;
            if(radius <= r_min) {
                r_min = radius;
            }
            if(radius >= r_max) {
                r_max = radius;
            }
        }
    }

    void Update() {
        swaytime += Time.deltaTime;

        float omega = 2*m_swayFrequency*Mathf.PI;

        SwayMovement(omega*swaytime);
    }

    void SwayMovement(float swayPhase) {
        float theta = Mathf.Deg2Rad*m_swayAmplitude * Mathf.Sin(swayPhase);     //angular sway equation in radian

        Vector3[] vert = mesh.vertices;
        for(int i =0; i < originalVert.Length; i++) {
            Vector3 originalWorldPos = transform.TransformPoint(originalVert[i]);
            float radius = transform.TransformPoint(originalVert[i]).y;
            float swayHeightFactor = (radius - r_min) / (r_max - r_min);    //scale eefect of angular sway w.r.t height from reference plane

            vert[i] = transform.InverseTransformPoint(new Vector3(radius*Mathf.Sin(swayHeightFactor*theta) + originalWorldPos.x, radius *Mathf.Cos(theta*swayHeightFactor), originalWorldPos.z));
        }

        mesh.vertices = vert;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
