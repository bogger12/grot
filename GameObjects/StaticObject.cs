using Microsoft.Xna.Framework;

public abstract class StaticObject: GameObject {

    public BoxCollider collider;
    public Sprite sprite;

    public StaticObject(string name): base(name) { }

    public override void Initialize()  {
        base.Initialize();
        collider = GetComponent<BoxCollider>();
        sprite = GetComponent<Sprite>();
    }
}