using UnityEngine;

public enum ScaleUnit { Inches, Mice, Eagles }

[CreateAssetMenu()]
public class WebsiteItem : ScriptableObject {
    public Sprite websitePNG;
    public string productName;
    [Range(0, 10)] public int avgReviews;
    public string description;
    public ScaleUnit scaleUnit;
    public bool wholeNumbers;
    public Vector2 minMax;
    public float idealAmount = 0;
}
