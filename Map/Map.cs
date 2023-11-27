
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using AlexMonoGame;


public struct CollisionZone {
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int H { get; set; }
}
public class Layer {
    public string name { get; set; }
    public string _eid { get; set; }
    public int offsetX { get; set; }
    public int offsetY { get; set; }
    public int gridCellWidth { get; set; }
    public int gridCellHeight { get; set; }
    public int gridCellsX { get; set; }
    public int gridCellsY { get; set; }
    public string tileset { get; set; }
    public List<int> data { get; set; }
    public int exportMode { get; set; }
    public int arrayMode { get; set; }
    public List<Entity> entities { get; set; }
}
public class Entity {
    public string name { get; set; }
    public int id { get; set; }
    public string _eid { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int originX { get; set; }
    public int originY { get; set; }
}
public class MapWrapper {
    public int width { get; set; }
    public int height { get; set; }
    public int offsetX { get; set; }
    public int offsetY { get; set; }
    public List<Layer> layers { get; set; }
}

public class Map {
    public const int pixelScale = 1; // keep at 1
    public string filepath;
    public MapWrapper map;

    public Map(string filepath) {
        this.filepath = filepath;
    }

    public void Initialize(Player player) {
        map = Tools.LoadJSONFIle<MapWrapper>(filepath);
        // Debug.WriteLine(JsonSerializer.Serialize<List<Tile>>(map.MapTiles));

        Layer tileLayer = map.layers[1];
        for (int i=0; i < tileLayer.gridCellsY; i++) {
            for (int j=0; j < tileLayer.gridCellsX; j++) {
                int tile_num = tileLayer.data[i*tileLayer.gridCellsX+j];
                Rectangle sourceRect = new Rectangle();
                sourceRect.Size = new Point(tileLayer.gridCellWidth, tileLayer.gridCellHeight);
                sourceRect.Y = 0;
                if (tile_num==-1) {
                    continue;
                } else if (tile_num==1) sourceRect.X = tileLayer.gridCellWidth*1;
                else if (tile_num==2) sourceRect.X = tileLayer.gridCellWidth*2;
                else if (tile_num==3) sourceRect.X = tileLayer.gridCellWidth*3;

                TileObject tile = new TileObject("tile");
                Sprite tilesprite = new Sprite(tileLayer.tileset, sourceRect);
                tile.AddComponent(tilesprite);
                tile.AddComponent(new Transform(new Vector2(j*tileLayer.gridCellWidth*pixelScale,i*tileLayer.gridCellWidth*pixelScale), new Point(tileLayer.gridCellWidth*pixelScale,tileLayer.gridCellHeight*pixelScale), 0f));
                tile.AddComponent(new BoxCollider(new Point(tileLayer.gridCellHeight*pixelScale, tileLayer.gridCellHeight*pixelScale)));
                tile.Initialize();
                Game1.tiles.Add(tile);

            }
        }
        foreach(Entity e in map.layers[0].entities) {
            if (e.name=="CollisionZone") {
                CollisionZoneObject czone = new CollisionZoneObject(e._eid);
                czone.AddComponent(new Transform(new Vector2(e.x*pixelScale,e.y*pixelScale), new Point(e.width*pixelScale,e.height*pixelScale), 0f));
                czone.AddComponent(new BoxCollider(new Point(e.width*pixelScale, e.height*pixelScale)));
                czone.Initialize();
            }
            if (e.name=="PlayerSpawn") {
                player.rigidbody.position = new Vector2(e.x, e.y);
            }
        }
    }

}