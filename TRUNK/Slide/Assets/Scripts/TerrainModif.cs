using UnityEngine;
using System.Collections;

public class TerrainModif : MonoBehaviour {

    public Terrain _Terrain;

    private TerrainData _TerrainData;

	// Use this for initialization
	void Awake () {
        _TerrainData = _Terrain.terrainData;
        //EditTerrain();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}



    public void EditTerrain()
    {
        int _Whidth = 10;
        int _Height = 10;

        float[,] Heights = _TerrainData.GetHeights(50, 50, _Whidth, _Height);

        for (int y = 0; y < _Height; y++)
        {

            for (int x = 0; x < _Whidth; x++)
            {

                

                Heights[x, y] = 1 / 250;
            }

        }
        _TerrainData.SetHeights(50, 50, Heights);

    }


}
