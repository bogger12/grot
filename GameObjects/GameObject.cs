using System.Collections.Generic;
using AlexMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class GameObject {
    public string name;
    public Transform transform;
    public List<Component> components;
    public bool active;

    public GameObject(string name) {
        this.name = name;
        this.active = false;
        components = new List<Component>();
    }
    public virtual void Initialize() {
        transform = GetComponent<Transform>();
        foreach(Component c in components) { c.Initialize(); }
    }

    public virtual void Load() {
        Sprite s;
        if ((s = GetComponent<Sprite>()) is not null) s.Load();
    }

    public virtual void Update(GameTime gameTime) {
        foreach(Component c in components) { c.Update(gameTime); }
    }

    public virtual void Draw(SpriteBatch _spriteBatch, Camera camera) {
        Sprite s;
        if ((s = GetComponent<Sprite>()) is not null) s.Draw(_spriteBatch, camera);
    }

    public T GetComponent<T>() where T: Component {
        foreach( Component c in components) {
            if (c is T) return c as T;
        } 
        return null;
    }

    public Component AddComponent(Component c) {
        if (c is Component) {
            components.Add(c);
            c.SetParent(this);
            return c;
        }
        else return null;
    }
    public Component RemoveComponent<T>() {
        foreach( Component c in components) {
            if (c is T) {
                components.Remove(c);
                return c;
            }
        } 
        return null;
    }

    public void SetParent(GameObject parent) {
        GetComponent<Transform>().SetParentTransform(parent.GetComponent<Transform>());
    }
    public void RemoveParent() {
        GetComponent<Transform>().SetParent(null);
    }
}