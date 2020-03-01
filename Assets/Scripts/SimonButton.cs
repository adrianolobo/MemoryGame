using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    public Sprite SpriteLight;
    public Sprite SpriteDark;
    bool isEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onAllowUserInput += Enable;
        GameEvents.current.onBlockUserInput += Disable;
        this.SetDark();
    }

    void OnMouseUp()
    {
        if (!isEnabled) return;
        this.SetDark();
        GameEvents.current.ButtonPressed(this);
    }

    void OnMouseDown()
    {
        if (!isEnabled) return;
        this.SetLight();
    }

    void Disable()
    {
        isEnabled = false;
    }

    void Enable()
    {
        isEnabled = true;  
    }

    public void SetLight()
    {
        this.SetSprite(SpriteLight);
    }
    
    public void SetDark()
    {
        this.SetSprite(SpriteDark);
    }

    void SetSprite(Sprite sprite)
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
