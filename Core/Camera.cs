
using AlexMonoGame;
using Microsoft.Xna.Framework;

public class Camera: GameObject {
    
    public Spring spring;
    public Camera(string name): base(name) {

    }

    public override void Initialize() {
        spring = GetComponent<Spring>();
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {   
        // do something
        base.Update(gameTime);
    }

    public Vector2 GetRenderPosition(Vector2 pos) {
        return pos-transform.GlobalPosition;
    }
}