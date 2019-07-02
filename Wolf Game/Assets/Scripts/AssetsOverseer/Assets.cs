using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class Assets : MonoBehaviour
{
    // Sprites for tiles
    public Sprite MountainFrost;
    public Sprite Mountain;
    public Sprite Desert;
    public Sprite DesertHills;
    public Sprite Grass;
    public Sprite GrassDeepForest;
    public Sprite GrassForest;
    public Sprite GrassHill;
    public Sprite GrassHillDeepForest;
    public Sprite GrassHillForest;
    public Sprite Plains;
    public Sprite PlainsDeepForest;
    public Sprite PlainsForest;
    public Sprite PlainsHill;
    public Sprite PlainsHillDeepForest;
    public Sprite PlainsHillForest;
    public Sprite Snow;
    public Sprite SnowHills;
    public Sprite Tundra;
    public Sprite TundraForest;
    public Sprite TundraHill;
    public Sprite TundraHillForest;
    public Sprite Water;
    public Sprite WaterIceberg;

    // Colors for tile selectors;
    public Color SelectedColor;
    public Color MoveableColor;
    public Color AttackableColor;
    public Color MoveableHighlightedColor;
    public Color AttackableHighlightedColor;
    public Color PathColor;
    public Color NoColor;

    public Dictionary<UnitTypeModel.UnitTypes, Sprite> Units;
    public Dictionary<UnitBackgrounds, Sprite> UnitBackGrounds;

    public void Start()
    {
        var extraSprites = GetComponent<AssetsPositions>();

        UnitBackGrounds = new Dictionary<UnitBackgrounds, Sprite>
        {
            [UnitBackgrounds.Normal] = extraSprites.NormalBackground,
            [UnitBackgrounds.Worker] = extraSprites.WorkerBackground,
            [UnitBackgrounds.Shield] = extraSprites.ShieldBackGround
        };

        Units = new Dictionary<UnitTypeModel.UnitTypes, Sprite>
        {
            [UnitTypeModel.UnitTypes.Wolf] = extraSprites.Wolf,
            [UnitTypeModel.UnitTypes.BattleSpider] = extraSprites.BattleSpider,
            [UnitTypeModel.UnitTypes.WorkerSpider] = extraSprites.WorkerSpider,
            [UnitTypeModel.UnitTypes.ScoutEagle] = extraSprites.ScoutEagle,
        };
    }

    public enum UnitBackgrounds
    {
        Normal, Worker, Leader, Party, Shield
    }
}
