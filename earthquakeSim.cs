
using UnityEngine;

public class earthquakeSim : MonoBehaviour
{
    private int width = 256;
    public float scale = 20f;
    private int height = 256;
    public int amplitude = 20;
    private float offset1,offset2 = 0;
    [Range(-10.0f,10.0f)]
    public float frequency1,frequency2 = 10;
    Terrain ground;
    Rigidbody rb;
    public float amp_x = 10f;
    public float amp_z = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ground = GetComponent<Terrain>();
        ground.terrainData = GenTerrain(ground.terrainData);
    }

    // Update is called once per frame
    void Update()
    {
        
        ground.terrainData = GenTerrain(ground.terrainData);
        offset1 += Time.deltaTime * frequency1;
        offset2 += Time.deltaTime * frequency2;
        Vector3 oscillate = new Vector3((calculateHeights(10,10)-0.5f)*amp_x, 0, (calculateHeights(10, 10) - 0.5f) * amp_z);
        transform.position = oscillate;
    }
    TerrainData GenTerrain(TerrainData terraindata)
    {
        terraindata.heightmapResolution = width + 1;
        terraindata.size = new Vector3(width, amplitude, height);
        terraindata.SetHeights(0, 0, generate_heights());
        return terraindata;
    }
    float[,] generate_heights()
    {
        float[,] heights = new float[width, height];
        for (int i = 0; i<height;i++)
        {
            for (int j = 0; j<width;j++)
            {
                heights[i, j] = calculateHeights(i,j);
            }
        }
        return heights;
    }
    float calculateHeights(int x,int y)
    {
        float xCoord = (float)x / width * scale + offset1;
        float yCoord = (float)y / height *scale + offset2;
        
        return Mathf.PerlinNoise(xCoord, yCoord);

    }
}
