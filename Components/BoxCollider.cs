using System.Collections.Generic;
using System.Diagnostics;
using AlexMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Collision {
    public Rectangle intersect;
    public Point centerpoint;
    public BoxCollider boxCollider;
    public Collision(Rectangle intersect, Point centerpoint, BoxCollider boxCollider) {
        this.intersect = intersect;
        this.centerpoint = centerpoint;
        this.boxCollider = boxCollider;
    }
}

public class BoxCollider: Component {

    public Point boundingbox;
    public BoxCollider(Point dimensions) {
        boundingbox = dimensions + new Point(1,1);
    }
    public override void Initialize() {
        Game1.colliders.Add(this);
    }
    public override void Update(GameTime gameTime) {}

    public Rectangle CalculateCollistionRect(BoxCollider boxc) {
        return new Rectangle((int)boxc.parent.transform.GlobalPosition.X, (int)boxc.parent.transform.GlobalPosition.Y, boxc.boundingbox.X, boxc.boundingbox.Y);
    }

    public Vector2 GetDisplacementAll(List<BoxCollider> colliders, Vector2 position, Vector2 displacement) {
        Vector2 origPos = parent.transform.GlobalPosition;
        parent.transform.GlobalPosition = position + displacement;
        foreach( BoxCollider b in colliders) {
            if (IsColliding(b)) {
                displacement = GetDisplacement(b, position, displacement);
            }
        }
        parent.transform.GlobalPosition = origPos;
        return displacement;
    }
    public bool IsColliding(BoxCollider other) {
        if (this.Equals(other)) return false;
        Vector2 pos = parent.transform.GlobalPosition;
        Rectangle otherrect = CalculateCollistionRect(other);
        Rectangle thisrect = CalculateCollistionRect(this);

        // bool x_collision = otherrect.Left <= pos.X && pos.X <= otherrect.Right;
        // bool y_collision = otherrect.Top <= pos.Y && pos.Y <= otherrect.Bottom;

        // bool x_collision = thisrect.Right >= otherrect.Left && thisrect.Left <= otherrect.Right; 
        // bool y_collision = thisrect.Top <= otherrect.Bottom && thisrect.Bottom >= otherrect.Top;

        // return x_collision && y_collision;
        return thisrect.Intersects(otherrect);
    }

    public List<Collision> CheckCollision(List<BoxCollider> colliders) {
        List<Collision> collisions = new List<Collision>();
        foreach( BoxCollider b in colliders) {
            if (IsColliding(b)) {
                Rectangle intersect = Rectangle.Intersect(ToRect(), b.ToRect());
                Collision collision = new Collision(intersect, intersect.Center, b);
                // if (intersect.Center.X == 0) Debug.WriteLine(intersect + " " + this.parent.transform.GlobalPosition + " " + b.parent.transform.GlobalPosition);
                collisions.Add(collision); 
                IsColliding(b);
            }
        }
        return collisions;
    }
    public static List<BoxCollider> CheckPointCollision(List<BoxCollider> colliders, Point point) {
        List<BoxCollider> collisions = new List<BoxCollider>();
        foreach( BoxCollider b in colliders) {
            if (b.IsCollidingPoint(point)) { collisions.Add(b); }
        }
        return collisions;
    }

    public bool IsCollidingPoint(Point point) {
        Rectangle checkrect = new Rectangle(Tools.Vector2Point(parent.transform.GlobalPosition), boundingbox);
        bool x_collision = checkrect.Left < point.X && point.X < checkrect.Right;
        bool y_collision = checkrect.Top < point.Y && point.Y < checkrect.Bottom;
        return x_collision && y_collision;
    }

    public Vector2 GetDisplacement(BoxCollider other, Vector2 position, Vector2 displacement) {
        Rectangle otherrect = CalculateCollistionRect(other);
        
        if (otherrect.Top < position.Y && position.Y < otherrect.Bottom) {
            // if current position within y bounds
            if (displacement.X < 0) displacement.X = otherrect.Right - position.X;
            else if (displacement.X > 0) displacement.X = otherrect.Left - position.X;
        }
        if (otherrect.Left < position.X && position.X < otherrect.Right) { 
            // if current position within x bounds
            if (displacement.Y > 0) displacement.Y = otherrect.Top - position.Y;
            else if (displacement.Y < 0) displacement.Y = otherrect.Bottom - position.Y;
        }
        return displacement;
    }

    // public bool IsSupportedBy(BoxCollider otherb) {
    //     Rectangle otherrect = CalculateCollistionRect(otherb);
    //     return otherrect.Left < parent.transform.GlobalPosition.X && 
    //         parent.transform.GlobalPosition.X < otherrect.Right && 
    //         parent.transform.GlobalPosition.Y == otherrect.Top;
    // }   // If collider is within x bounds of supporting collider and bottom is same height

    public void Outline(int thickness, Color color) {
        Rectangle orect = CalculateCollistionRect(this);
        Game1.lines.Add(new Line(new Vector2(orect.Left+1,orect.Top), new Vector2(boundingbox.X-2,0), thickness, color));
        Game1.lines.Add(new Line(new Vector2(orect.Right-1,orect.Top), new Vector2(0,boundingbox.Y-1), thickness, color));
        Game1.lines.Add(new Line(new Vector2(orect.Right-1,orect.Bottom-1), new Vector2(-boundingbox.X+1,0), thickness, color));
        Game1.lines.Add(new Line(new Vector2(orect.Left+1,orect.Top), new Vector2(0,boundingbox.Y-1), thickness, color));
    }

    public Rectangle ToRect() {
        return new Rectangle((int)parent.transform.GlobalPosition.X, (int)parent.transform.GlobalPosition.Y, boundingbox.X, boundingbox.Y);
    }


}