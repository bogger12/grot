using Microsoft.Xna.Framework;

public abstract class Component {
    protected GameObject parent;
    
    public abstract void Initialize();
    public abstract void Update(GameTime gameTime);

    public void SetParent(GameObject parent) { this.parent = parent; }
    public GameObject GetParent() { return parent; }
}