#define DEBUG

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Line {
    public Vector2 origin;
    public Vector2 vector;
    public int thickness;
    public Color color;
    public Line(Vector2 origin, Vector2 vector, int thickness, Color color) {
        this.origin = origin;
        this.vector = vector;
        this.thickness = thickness;
        this.color = color;
    }
    public Line(Point startPos, Point endPos, int thickness, Color color) {
        this.origin = Tools.Point2Vector(startPos);
        this.vector = Tools.Point2Vector(endPos-startPos);
        this.thickness = thickness;
        this.color = color;
    }
}


public static class Tools {
    public static Texture2D lineTex;
    public static Texture2D CreateLineTexture(SpriteBatch spriteBatch, Color color) {
        // Create a texture as wide as the distance between two points and as high as the desired thickness of the line.
        Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        // Fill texture with given color.
        Color[] data = new Color[] {color};
        texture.SetData(data);
        return texture;
    }
    public static void DrawLine(SpriteBatch spriteBatch, Line line, Camera camera) {
        if (lineTex is null) lineTex = CreateLineTexture(spriteBatch, Color.White);
        float distance = line.vector.Length();

        // Rotate about the beginning middle of the line.
        float rotation = (float)Math.Atan2(line.vector.Y, line.vector.X);
        Vector2 origin = new Vector2(0, line.thickness / 2);
        
        line.origin = camera.GetRenderPosition(line.origin); // Important to position element relative to camera
        Rectangle destrect = new Rectangle(Vector2Point(line.origin), new Point((int)distance, line.thickness));
        // Debug.WriteLine(destrect);
        spriteBatch.Draw(lineTex, destrect, null, line.color, rotation, origin, SpriteEffects.None, 0f);
    }

    public static Point Vector2Point(Vector2 input) {
        return new Point((int)input.X, (int)input.Y);
    }
    public static Vector2 Point2Vector(Point input) {
        return new Vector2(input.X, input.Y);
    }

    public static string Vector2String(Vector2 vector2, int decimals) {
        return string.Format("({0:F"+decimals+"},{1:F"+decimals+"})", vector2.X, vector2.Y);
    }

    public static Vector2 Vector2Int(Vector2 vector2) {
        return new Vector2((int)vector2.X, (int)vector2.Y);
    }

    public static Vector2 PolarToCartesian(float distance, float angle) {
        float x = (float)Math.Cos(angle)*distance;
        float y = (float)Math.Sin(angle)*distance;
        return new Vector2(x,y);
    }

    public static float VectorAngle(Vector2 v1, Vector2 v2) {
        return (float)Math.Atan2(v2.Y-v1.Y,v2.X-v1.X);
    }

    public static T LoadJSONFIle<T>(string filepath) {
        string jsonString = File.ReadAllText(filepath);
        return JsonSerializer.Deserialize<T>(jsonString);
    }


}