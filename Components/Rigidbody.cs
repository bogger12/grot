using System.Diagnostics;
using AlexMonoGame;
using Microsoft.Xna.Framework;
public class Rigidbody: Component {
    public bool doGravity;
    public Vector2 velocity;
    public Vector2 position;
    public float angularvelocity;
    public float mass;
    public BoxCollider collider;

    public Rigidbody(float mass) {
        velocity = new Vector2(0,0);
        position = new Vector2(0,0);
        angularvelocity = 0f;
        doGravity = true;
        this.mass = mass;
    }

    public override void Initialize() {
        collider = parent.GetComponent<BoxCollider>();
        position = parent.transform.GlobalPosition;
    }
    public override void Update(GameTime gameTime) {
        if (doGravity) velocity += new Vector2(0,Game1.globalValues.gravity*(float)gameTime.ElapsedGameTime.TotalSeconds);
        Move(velocity);
        UpdatePosition();
    }

    public void Move(Vector2 displacement) {
        if (collider is not null) {
            Vector2 final_displacement = collider.GetDisplacementAll(Game1.colliders, position, displacement);
            if (final_displacement != velocity) velocity *= new Vector2(final_displacement.X==displacement.X?1:0, final_displacement.Y==displacement.Y?1:0);
            position += final_displacement;
        } else position += displacement;
    }
    public void UpdatePosition() {
        parent.transform.GlobalPosition = position;
    }

    public void AddForce(float force, float angle) {
        velocity += Tools.PolarToCartesian(force, angle) / mass;
    }
    public void AddForce(Vector2 force) {
        velocity += force / mass;
    }
}