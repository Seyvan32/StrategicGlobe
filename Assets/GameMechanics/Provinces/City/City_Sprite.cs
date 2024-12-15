using UnityEngine;

public class City_Sprite : MonoBehaviour
{
    private SpriteRenderer sprite_comp; // city logo
    [SerializeField] private bool isCapital;
    [SerializeField] private Sprite capital; //sprite of capital
    [SerializeField] private Sprite regular; //sprite of regular city

    private void Awake()
    {
        // Get the sprite renderer component of city
        sprite_comp = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start() 
    {
        // Set the sprite of city based on whether it is a capital or not
        if (isCapital)
        {
            sprite_comp.sprite = capital;
        }
        else
        {
            sprite_comp.sprite = regular;
        }
    }
}
