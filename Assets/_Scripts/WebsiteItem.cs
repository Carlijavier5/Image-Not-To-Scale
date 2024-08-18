using UnityEngine;

public enum ScaleUnit { Mouse, Eagle, Frog }

public class WebsiteItem : ScriptableObject {
    public Sprite websitePNG;
    [Range(0, 10)] public int avgReviews;
    public string description;
    public ScaleUnit scaleUnit;
}
