using UnityEngine;

public class PieceHighlighter : MonoBehaviour
{
    public bool highlighted = false;
    private SpriteRenderer sr;

    public Vector2 Position;

    private PeiceMoveDisplayer _moveDisplayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        _moveDisplayer = PeiceMoveDisplayer.Instance;
    }

    void Update()
    {
        sr.enabled = highlighted;
    }

    private void OnMouseDown()
    {
        print($"Clicked on {Position}");
        if (!highlighted)
            _moveDisplayer.UpdateSelectedPosition(Position);
        else
            _moveDisplayer.Capture(Position);
    }
}
