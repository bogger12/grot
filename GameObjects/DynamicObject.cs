using Microsoft.Xna.Framework;

public class DynamicObject: GameObject {

    public BoxCollider collider;
    public Sprite sprite;
    public Rigidbody rigidbody;

    public DynamicObject(string name): base(name) { }

    public override void Initialize()  {
        base.Initialize();
        collider = GetComponent<BoxCollider>();
        sprite = GetComponent<Sprite>();
        rigidbody = GetComponent<Rigidbody>();
    }

}