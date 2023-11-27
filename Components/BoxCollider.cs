using System.Collections.Generic;
using System.Diagnostics;
using AlexMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class BoxCollider: Component {

    Point boundingbox;
    public BoxCollider(Point dimensions) {
        boundingbox = dimensions;
        Game1.colliders.Add(this);
    }
    public override void Initialize() {}
    public override void Update(GameTime gameTime) {}

    public Rectangle CalculateCollistionRect(BoxCollider other) {
        return new Rectangle((int)other.parent.transform.GlobalPosition.X-boundingbox.X, (int)other.parent.transform.GlobalPosition.Y-boundingbox.Y, other.boundingbox.X+boundingbox.X, other.boundingbox.Y+boundingbox.Y);
    }

    public Vector2 GetDisplacementAll(List<BoxCollider> colliders, Vector2 position, Vector2 displacement) {
        foreach( BoxCollider b in colliders) {
            Vector2 origPos = parent.transform.GlobalPosition;
            parent.transform.GlobalPosition = position + displacement;
            if (IsColliding(b)) {
                displacement = GetDisplacement(b, position, displacement);
            }
            parent.transform.GlobalPosition = origPos;
        }
        return displacement;
    }
    public bool IsColliding(BoxCollider other) {
        if (this.Equals(other)) return false;
        Vector2 pos = parent.transform.GlobalPosition;
        Rectangle otherrect = CalculateCollistionRect(other);

        bool x_collision = otherrect.Left < pos.X && pos.X < otherrect.Right;
        bool y_collision = otherrect.Top < pos.Y && pos.Y < otherrect.Bottom;

        return x_collision && y_collision;
    }

    public List<BoxCollider> CheckCollision(List<BoxCollider> colliders) {
        List<BoxCollider> collisions = new List<BoxCollider>();
        foreach( BoxCollider b in colliders) {
            if (b.IsColliding(this)) { collisions.Add(b); }
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

    public bool IsSupportedBy(BoxCollider otherb) {
        Rectangle otherrect = CalculateCollistionRect(otherb);
        return otherrect.Left < parent.transform.GlobalPosition.X && 
            parent.transform.GlobalPosition.X < otherrect.Right && 
            parent.transform.GlobalPosition.Y == otherrect.Top;
    }   // If collider is within x bounds of supporting collider and bottom is same height

    public void Outline(SpriteBatch spriteBatch, Camera camera, int thickness, Color color) {
        Vector2 renderPos = camera.GetRenderPosition(parent.transform.GlobalPosition);

        Vector2 topleft = renderPos;
        Vector2 topright = renderPos + new Vector2(boundingbox.X,0);
        Vector2 bottomleft = renderPos + new Vector2(0,boundingbox.Y);
        Vector2 bottomright = renderPos + new Vector2(boundingbox.X,boundingbox.Y);
        Tools.DrawLine(spriteBatch, topleft, topright, thickness, color);
        Tools.DrawLine(spriteBatch, topright, bottomright, thickness, color);
        Tools.DrawLine(spriteBatch, bottomright, bottomleft, thickness, color);
        Tools.DrawLine(spriteBatch, bottomleft, topleft, thickness, color);
    }


}