using UnityEngine;

public class GameController : MonoBehaviour
{
    // Inserted in unity editor
    public Transform mapContainer;
    public GameObject spaceViewPrefab;
    public RandomSeeds seeds;

    // Our Reference to the Map
    MapModel map;

    // Start is called before the first frame update
    void Start()
    {
        seeds = new RandomSeeds();
        map = new MapModel(this);
    }

    // Adds a SpaceModel's controller and view.
    public SpaceController AddSpace(SpaceModel spaceModel)
    {
        DoubledCoords coords = spaceModel.GetDoubledCoords();
        var x = Utilities.HEX_SIZE * Mathf.Sqrt(3)/2 * coords.col;
        var y = Utilities.HEX_SIZE * 3/2f * coords.row;

        GameObject SpaceView = UnityEngine.Object.Instantiate(spaceViewPrefab, new Vector2(x, y), Quaternion.identity, mapContainer);
        SpaceView.GetComponent<SpaceController>().SetSpaceView(spaceModel, SpaceView, seeds);
        return SpaceView.GetComponent<SpaceController>();
    }
}

public class RandomSeeds
{
    public float elevation, baseTerrain, feature;
    public RandomSeeds()
    {
        elevation = UnityEngine.Random.value * 1000;
        baseTerrain = UnityEngine.Random.value * 1000;
        feature = UnityEngine.Random.value * 1000;
    }
}