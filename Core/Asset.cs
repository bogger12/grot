using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

public class Asset<T> {
    public string name;
    public T asset;

    public Asset(string name) {
        this.name = name;
    }

    public void LoadAsset(ContentManager Content) {
        asset = Content.Load<T>(name);
    }

    public static T GetAsset(List<Asset<T>> assets, string name) {
        foreach(Asset<T> a in assets) {
            if (a.name==name) return a.asset;
        }
        return default(T);
    }
}