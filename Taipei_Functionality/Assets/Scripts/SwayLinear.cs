using UnityEngine;

public class SwayLinear : MonoBehaviour
{
    public float m_swayFrequency;
    public float m_swayAmplitude;

    public bool autoupdate;

    Mesh ms;
    float swaytime;
    float heightMin;       //minimum height w.r.t refrence plane (now world origin)
    float heightMax;
    float swayHeightFactor;     //to scale vibration amplitude w.r.t height
    float omega;
    Vector3[] originalVert;

    void Start() {
        swaytime = 0f;
        omega = 2*m_swayFrequency*Mathf.PI;
        
        ms = GetComponent<MeshFilter>().mesh;
        Vector3[] vert = ms.vertices;
        originalVert = ms.vertices;

        //to find the minimum height of building
        heightMin = transform.TransformPoint(vert[0]).y;        //convert mesh point from local to world space 
        heightMax = transform.TransformPoint(vert[0]).y;
        for(int i=0; i< vert.Length; i++) {
            float height = transform.TransformPoint(vert[i]).y;

            if(height <= heightMin) {
                heightMin = height;
            }

            if(height >= heightMax) {
                heightMax = height;
            }
        }
    }

    void Update() {
        swaytime += Time.deltaTime;     //Update time with each frame

        if(autoupdate) {
            omega = 2*m_swayFrequency*Mathf.PI;
        }

        SwayMovement(omega*swaytime);
    }

    void SwayMovement(float swayPhase) {
        //For sway along x axis
        float vibration = m_swayAmplitude * Mathf.Sin(swayPhase);

        Vector3[] vert = ms.vertices;

        for(int i =0; i<vert.Length; i++) {
            Vector3 height = transform.TransformPoint(originalVert[i]);     //convert mesh point height from local to world space
            swayHeightFactor = (height.y - heightMin)/(heightMax - heightMin);

            vert[i] = transform.InverseTransformPoint(height + new Vector3(swayHeightFactor*vibration, 0f, 0f));
        }

        ms.vertices = vert;
        ms.RecalculateBounds();
        ms.RecalculateNormals();
    }
}
