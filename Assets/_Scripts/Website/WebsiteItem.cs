using UnityEngine;

public enum ScaleUnit { Mouse, Eagle, Frog }

[CreateAssetMenu()]
public class WebsiteItem : ScriptableObject {
    public Sprite websitePNG;
    public string productName;
    [Range(0, 10)] public int avgReviews;
    public string description;
    public ScaleUnit scaleUnit;
}
