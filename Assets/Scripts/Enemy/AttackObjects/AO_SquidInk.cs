using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_SquidInk : AO_Persistent
{
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    [SerializeField] private float inkSpeed;
    [SerializeField] private AnimationCurve inkAlpha;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponentInChildren<Collider2D>();
        lifeTime = Random.Range(1, lifeTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, Time.time / 1000f);    // fix z fighting
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, inkSpeed * Time.deltaTime);
        Color newColor = _renderer.color;
        //float alpha = Mathf.MoveTowards(newColor.a, targetAlpha, 0.05f * Time.deltaTime);
        newColor.a = inkAlpha.Evaluate(timer/lifeTime);
        _renderer.color = newColor;
        _collider.enabled = newColor.a > .4f;
    }
}
