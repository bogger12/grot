
using Microsoft.Xna.Framework;

public class Spring: Component {

    public const float springConstantStandard = 0.1f;
    public const float dampeningCoefficientStandard = 6f; // takes 10% each frame
    public const float EPStandard = 0f;
    public float springConstant;
    public float dampeningCoefficient;
    public float EP;
    private Rigidbody rigidbody;
    private Transform transform;
    public Transform otherTransform;

    public Spring(Transform otherTransform, float springConstant=springConstantStandard, float dampeningCoefficient=dampeningCoefficientStandard, float EP=EPStandard) {
        this.otherTransform = otherTransform;
        this.springConstant = springConstant;
        this.dampeningCoefficient = dampeningCoefficient;
        this.EP = EP;
    }

    public override void Initialize() {
        transform = parent.GetComponent<Transform>();
        rigidbody = parent.GetComponent<Rigidbody>();
    }

    public override void Update(GameTime gameTime) {
        // Pull this towards other transform
        // Get Distance
        Vector2 distanceVector = (otherTransform.GlobalPosition+Tools.Point2Vector(otherTransform.origin))-(transform.GlobalPosition+Tools.Point2Vector(transform.origin));
        // Get Force
        float distance = distanceVector.Length()-EP;
        float F = springConstant * (distance > 0 ? distance: 0);
        distanceVector.Normalize();
        // Add Force to Rigidbody
        rigidbody.AddForce(F*distanceVector*(float)gameTime.ElapsedGameTime.TotalSeconds);
        rigidbody.AddForce(-dampeningCoefficient*rigidbody.velocity*(float)gameTime.ElapsedGameTime.TotalSeconds);
        // rigidbody.velocity *= dampeningCoefficient*(float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}