using System.Diagnostics;
using AlexMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Sprite: Component {
    public Texture2D spriteTexture;
    public Rectangle ?sourceRect;
    public string texturename;

    public Sprite(string texturename, Rectangle ?sourceRect) { 
        this.texturename = texturename; 
        this.sourceRect = sourceRect;
    }

    public override void Initialize() {}
    public override void Update(GameTime gameTime) {}
    public void Load() {
        spriteTexture = Asset<Texture2D>.GetAsset(Game1.textures, texturename);
    }

    public void Draw(SpriteBatch _spriteBatch, Camera camera) {
        Vector2 renderPos = camera.GetRenderPosition(parent.transform.GlobalPosition);
        if (spriteTexture is not null) {
            _spriteBatch.Draw(
                spriteTexture,
                new Rectangle(
                    (int)renderPos.X, 
                    (int)renderPos.Y, 
                    parent.transform.dimensions.X, 
                    parent.transform.dimensions.Y
                ), 
                sourceRect,
                Color.White
            );
        }
    }
}