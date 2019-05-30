using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    // Inserted in unity editor
    public Transform mapContainer;
    public GameObject spaceViewPrefab;
    public Assets assets;
    public GameObject mouseOverText;

    public RandomSeeds seeds;

    // Our Reference to the Map
    private MapModel map;

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
        SpaceView.GetComponent<SpaceController>().SetSpaceView(spaceModel, SpaceView, this);
        return SpaceView.GetComponent<SpaceController>();
    }
}