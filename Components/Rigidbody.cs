using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlexMonoGame;
using Microsoft.Xna.Framework;
public class Rigidbody: Component {
    public bool doGravity;
    public Vector2 acceleration;
    public Vector2 velocity;
    public Vector2 position;
    public float angularvelocity;
    public float mass;
    public static float restitutionCoefficient = 0.8f;
    public BoxCollider collider;

    public Rigidbody(float mass) {
        acceleration = Vector2.Zero;
        velocity = Vector2.Zero;
        position = Vector2.Zero;
        angularvelocity = 0f;
        doGravity = true;
        this.mass = mass;
    }

    public override void Initialize() {
        collider = parent.GetComponent<BoxCollider>();
        position = parent.transform.GlobalPosition;
        Game1.rigidbodies.Add(this);
    }
    public override void Update(GameTime gameTime) {
        if (doGravity) velocity += new Vector2(0,Game1.globalValues.gravity);
        // Move(gameTime);
        // StepVelocity(gameTime);
        // UpdatePositionVelocity();
        // if (collider is not null) CalculateCollisions();
        // UpdatePosition();
    }

    // public void Move(GameTime gameTime) {
    //     if (collider is not null) {
    //         List<BoxCollider> colliding = collider.GetCollidingList(Game1.colliders, position, velocity);
    //         foreach(BoxCollider c in colliding) { // TODO: fix this horrible collision code
    //             if (this.GetParent() is DynamicObject a) {
    //                 if (c.GetParent() is DynamicObject b) {
    //                     Vector2 N = b.transform.GlobalPosition-a.transform.GlobalPosition;
    //                     N.Normalize(); // Normal Vector
                        
    //                     // Vector2 Vn = Vector2.Dot(N, velocity)*N;
    //                     // Vector2 Vt = velocity - Vn;
    //                     // Vector2 Vf = Vt - Vn*restitutionCoefficient;
    //                     // a.rigidbody.AddForce(a.rigidbody.mass*Vf/(float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);
    //                     Debug.WriteLine(N);
    //                 } 
    //                 // else if (c.GetParent() is StaticObject s) {
    //                 //     Vector2 direction = s.transform.GlobalPosition-a.transform.GlobalPosition;
    //                 //     direction.Normalize();

    //                 //     a.rigidbody.AddForce(-direction*GetCollisionDirection(direction)*a.rigidbody.mass*velocity*(float)gameTime.ElapsedGameTime.TotalSeconds);
    //                 // }
    //             }
    //         }
    //         Vector2 final_displacement = collider.GetDisplacementAll(Game1.colliders, position, velocity);
    //         if (final_displacement != velocity) velocity *= new Vector2(final_displacement.X==velocity.X?1:0, final_displacement.Y==velocity.Y?1:0);
    //         position += final_displacement;
    //     } else position += velocity;
    // }

    public void StepVelocity(GameTime gameTime) {
        velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        acceleration = Vector2.Zero;
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    public void UpdatePosition() {
        parent.transform.GlobalPosition = position;
    }

    public void CalculateCollisions(GameTime gameTime) {
        Game1.lines.Add(new Line(parent.transform.GlobalPosition, velocity, 1, Color.Green));

        List<Collision> colliding = collider.CheckCollision(Game1.colliders);
        foreach(Collision c in colliding) { // TODO: fix this horrible collision code
            if (this.GetParent() is DynamicObject a) {
                if (c.boxCollider.GetParent() is DynamicObject b) {
                    Game1.dcollisions++;
                    Vector2 N = -(b.transform.GlobalPosition-a.transform.GlobalPosition);
                    N.Normalize(); // Normal Vector
                    // AlignBoxes(a, b, -N);
                    // Vector2 totalV = a.rigidbody.velocity+b.rigidbody.velocity;

                    Vector2 Vn = Vector2.Dot(N, velocity)*N;
                    Vector2 Vt = velocity - Vn;
                    Vector2 Vf = Vt - Vn*restitutionCoefficient;
                    Vf *= new Vector2(-1,1);

                    if (Math.Abs(N.X)>Math.Abs(N.Y)) Vf.Y = 0;
                    else if (Math.Abs(N.X)<Math.Abs(N.Y)) Vf.X = 0;
                    else Vf = Vector2.Zero;

                    AlignBoxes(a, b, -N);
                    if (Vf.X!=0&&Vf.Y!=0) {
                        Debug.WriteLine(Vf);
                    }
                    // Vf *= OppositeDirection(a.rigidbody.velocity, b.rigidbody.velocity);

                    b.rigidbody.AddForce(a.rigidbody.mass*Vf/(float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);
                    a.rigidbody.AddForce(b.rigidbody.mass*-Vf/(float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);
                    
                    Debug.WriteLine("collision from " + parent.name + " " + a.transform.GlobalPosition + " to " + b.name + " " + b.transform.GlobalPosition);
                    Game1.lines.Add(new Line(a.transform.GlobalPosition, b.transform.GlobalPosition-a.transform.GlobalPosition, 1, Color.Blue)); // Draw line between collistion points
                } 
                else if (c.boxCollider.GetParent() is StaticObject s) {
                    Game1.scollisions++;
                    Vector2 N = s.transform.GlobalPosition-a.transform.GlobalPosition;
                    N.Normalize();

                    Vector2 Vn = Vector2.Dot(N, velocity)*N;
                    Vector2 Vt = velocity - Vn;
                    Vector2 Vf = Vt - Vn*restitutionCoefficient;
                    Vf *= new Vector2(1,1);

                    if (Math.Abs(N.X)>Math.Abs(N.Y)) Vf.Y = 0;
                    else if (Math.Abs(N.X)<Math.Abs(N.Y)) {
                        Vf.X = 0; // Supported by s
                        if (s.transform.GlobalPosition.Y>a.transform.GlobalPosition.Y && parent is Player p) p.isGrounded = true;
                    }
                    else Vf = Vector2.Zero;

                    AlignBoxes(a, s, N, true);

                    a.rigidbody.AddForce(a.rigidbody.mass*Vf/(float)gameTime.ElapsedGameTime.TotalSeconds, gameTime);


                    Game1.lines.Add(new Line(a.transform.GlobalPosition, s.transform.GlobalPosition-a.transform.GlobalPosition, 1, Color.AliceBlue)); // Draw line between collistion points

                }
            }
        }
    }

    public Vector2 GetCollisionDirection(Vector2 direction) {
        if (Math.Abs(direction.X)>Math.Abs(direction.Y)) {
            return new Vector2(1,0);
        } else if (Math.Abs(direction.X)<Math.Abs(direction.Y)) {
            return new Vector2(0,1);
        } else return Vector2.Zero;
    }

    // public Vector2 OppositeDirection(Vector2 a, Vector2 b) {
    //     Vector2 final = Vector2.Zero;
    //     if (a.X>a.Y==b.X>b.Y) final.X = 1;
    //     if (a.Y>a.X==b.Y>b.X) final.Y = 1;
    //     return final;
    // }

    public void AlignBoxes(DynamicObject a, GameObject b, Vector2 N, bool resetVelocity=false) {
        if (Math.Abs(Math.Abs(N.X)-Math.Abs(N.Y))<0.1f) {
            return;
        }
        if (Math.Abs(N.X)<Math.Abs(N.Y) && N.Y>0 == velocity.Y>0) { // Vertical Side
            if (N.Y<0) position.Y = b.transform.GlobalPosition.Y+b.GetComponent<BoxCollider>().boundingbox.Y;
            else position.Y = b.transform.GlobalPosition.Y-a.collider.boundingbox.Y+1;
            if (resetVelocity && (N.Y<0==velocity.Y<0)) velocity.Y = 0; // only when down
        }
        else if (Math.Abs(N.X)>Math.Abs(N.Y) && N.X>0 == velocity.X>0) { // Horizontal Side
            if (N.X>0) position.X = b.transform.GlobalPosition.X-a.collider.boundingbox.X+1;
            else position.X = b.transform.GlobalPosition.X+b.GetComponent<BoxCollider>().boundingbox.X;
            if (resetVelocity && (N.X>0==velocity.X>0)) velocity.X = 0;
        }
    }

    public void AddForce(float force, float angle, GameTime gameTime) {
        acceleration += (Tools.PolarToCartesian(force, angle) / mass);
        if (Math.Abs(acceleration.X) < 0.001) acceleration.X = 0;
        if (Math.Abs(acceleration.Y) < 0.001) acceleration.Y = 0;
    }
    public void AddForce(Vector2 force, GameTime gameTime) {
        acceleration += force / mass;
        if (Math.Abs(acceleration.X) < 0.001) acceleration.X = 0;
        if (Math.Abs(acceleration.Y) < 0.001) acceleration.Y = 0;
    }
}