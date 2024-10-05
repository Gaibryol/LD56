using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_SquidInk : AO_Persistent
{
    private SpriteRenderer _renderer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Color newColor = _renderer.color;
        newColor.a = Mathf.Lerp(1, 0, timer / lifeTime);
        _renderer.color = newColor;
    }
}
