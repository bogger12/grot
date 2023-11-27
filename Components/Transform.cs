using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Transform: Component {
    private Transform parentTransform;
    public Vector2 GlobalPosition {
        get { return (parentTransform is null) ? position : parentTransform.GlobalPosition + position; }
        set { position = (parentTransform is null) ? value : value - parentTransform.GlobalPosition; }
    }
    private Vector2 position;
    public Point dimensions;
    public float rotation;
    public Point origin;

    public Transform() {
        this.position = new Vector2(0,0);
        this.dimensions = Point.Zero;
        this.rotation = 0f;
        this.origin = new Point(0, 0);
    }
    public Transform(Vector2 position) {
        this.position = position;
        this.dimensions = Point.Zero;
        this.rotation = 0f;
        this.origin = new Point(0, 0);
    }
    public Transform(Vector2 position, Point dimensions, float rotation) {
        this.position = position;
        this.dimensions = dimensions;
        this.rotation = rotation;
        this.origin = new Point(dimensions.X/2, dimensions.Y/2);
    }
    public override void Initialize() {}
    public override void Update(GameTime gameTime) {}

    public void SetParentTransform(Transform newparent) {
        parentTransform = newparent;
    }
    public Transform GetParentTransform() { return parentTransform; }

    public void SetLocalPosition(Vector2 position) {
        this.position = position;
    }
    
}