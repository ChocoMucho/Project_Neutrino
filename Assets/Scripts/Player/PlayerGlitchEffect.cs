using UnityEngine;

public class PlayerGlitchEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material glitchMaterial;

    [SerializeField] private string propertyName = "_GlitchAmount";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        glitchMaterial = spriteRenderer.material;
        SetGlitch(false);
    }

    public void SetGlitch(bool adjst)
    {
        if(adjst)
            glitchMaterial.SetFloat(propertyName, 100f);
        else
            glitchMaterial.SetFloat(propertyName, 0f);
    }
}
