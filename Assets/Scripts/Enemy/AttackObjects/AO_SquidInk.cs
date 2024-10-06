using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_SquidInk : AO_Persistent
{
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Color newColor = _renderer.color;
        float alpha = Mathf.Lerp(1, 0, timer / lifeTime);
        newColor.a = alpha;
        _renderer.color = newColor;
        _collider.enabled = alpha > .4f;
    }
}
