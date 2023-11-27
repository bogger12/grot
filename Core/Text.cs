using System.Diagnostics;
using AlexMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public enum TextStartPoint {
    TopLeft, TopRight, BottomLeft, BottomRight, Center
}
public class Text: GameObject {
    string text;
    string fontname;
    SpriteFont font;
    TextStartPoint textStartPoint;
    Vector2 origin;
    
    public Text(string name, string text, string fontname, TextStartPoint textStartPoint): base(name) {
        this.text = text;
        this.fontname = fontname;
        this.textStartPoint = textStartPoint;
    }
    public override void Load() {
        font = Asset<SpriteFont>.GetAsset(Game1.fonts, fontname);
        SetStartPoint();
        Vector2 size = font.MeasureString(text);
        transform.dimensions = new Point((int)size.X, (int)size.Y);
    }

    public void SetStartPoint() {
        if (textStartPoint == TextStartPoint.Center) origin = font.MeasureString(text) / 2;
        else if (textStartPoint == TextStartPoint.TopLeft) origin = new Vector2(0,0);
        else if (textStartPoint == TextStartPoint.TopRight) origin = new Vector2(font.MeasureString(text).X,0);
        else if (textStartPoint == TextStartPoint.BottomLeft) origin = new Vector2(0,font.MeasureString(text).Y);
        else if (textStartPoint == TextStartPoint.BottomRight) origin = new Vector2(font.MeasureString(text).X,font.MeasureString(text).Y);
    }

    public override void Draw(SpriteBatch _spriteBatch, Camera camera) {
        _spriteBatch.DrawString(font, text, transform.GlobalPosition, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.5f);
    }

    public void UpdateText(string newtext) {
        text = newtext;
        SetStartPoint();
    }



}