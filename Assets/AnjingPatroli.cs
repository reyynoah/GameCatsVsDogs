using UnityEngine;

public class AnjingPatroli : MonoBehaviour
{
    public float kecepatan = 2f;
    public float jarakPatroli = 3f;
    
    private float titikAwalX;
    private int arah = 1; // 1 = kanan, -1 = kiri
    private SpriteRenderer spriteAnjing;
    private Rigidbody2D rb;

    void Start()
    {
        titikAwalX = transform.position.x;
        spriteAnjing = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Pake linearVelocity biar jalannya stabil ngikutin fisika Unity terbaru
        rb.linearVelocity = new Vector2(arah * kecepatan, rb.linearVelocity.y);

        // Kalau anjing udah kejauhan ke kanan, suruh putar balik ke kiri
        if (transform.position.x >= titikAwalX + jarakPatroli)
        {
            arah = -1;
            spriteAnjing.flipX = true; // Ganti jadi false kalau gambarnya kebalik
        }
        // Kalau anjing udah kejauhan ke kiri, suruh putar balik ke kanan
        else if (transform.position.x <= titikAwalX - jarakPatroli)
        {
            arah = 1;
            spriteAnjing.flipX = false; // Ganti jadi true kalau gambarnya kebalik
        }
    }
}