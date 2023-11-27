using Microsoft.Xna.Framework;

public abstract class DynamicObject: GameObject {

    public BoxCollider collider;
    public Sprite sprite;

    public DynamicObject(string name): base(name) { }

    public override void Initialize()  {
        base.Initialize();
        collider = GetComponent<BoxCollider>();
        sprite = GetComponent<Sprite>();
    }

}