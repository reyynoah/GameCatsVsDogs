using UnityEngine;
using UnityEngine.UI; // Wajib ditambahin buat ngatur UI Text
using TMPro;
using UnityEngine.SceneManagement;

public class KucingController : MonoBehaviour
{
    [Header("Pergerakan Kucing")]
    public float kecepatanJalan = 5f;
    public float kekuatanLompat = 7f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteKucing;
    private bool isGrounded = false;

    [Header("Status Kucing")]
    public int nyawa = 3;
    private int maxNyawa = 3;
    public int ikanTerkumpul = 0;
    public bool punyaTulang = false;

    [Header("Pengaturan UI & Objek")]
    public GameObject[] ikonNyawa;
    public float batasJatuh = -2f;
    
    [Header("Menu Menang & Skor")]
    public GameObject panelMenang; 
    public TextMeshProUGUI teksSkor; // Slot buat masukin Teks UI Skor dari Hierarchy
    public Transform titikRespawn;

    // --- TAMBAHAN LANGKAH 3 (UNLOCK LEVEL) ---
    [Header("Buka Level Selanjutnya")]
    public string namaKunciBukaLevel; // Ketik "BukaLevel2" atau "BukaLevel3" di Inspector Unity nanti

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteKucing = GetComponent<SpriteRenderer>();
        UpdateUINyawa();
        
        // Update teks skor pertama kali pas game baru mulai
        if (teksSkor != null)
        {
            teksSkor.text = "Ikan: " + ikanTerkumpul;
        }
        
        // Pastikan game berjalan normal (waktu tidak stop)
        Time.timeScale = 1f; 
    }

    void Update()
    {
        float jalan = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(jalan * kecepatanJalan, rb.linearVelocity.y);

        if (jalan > 0)
            spriteKucing.flipX = false;
        else if (jalan < 0)
            spriteKucing.flipX = true;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, kekuatanLompat);
            isGrounded = false;
        }

        // Panggil fungsi Respawn saat jatuh
        if (transform.position.y < batasJatuh)
        {
            KucingJatuh();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tanah"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Anjing"))
        {
            nyawa--;
            UpdateUINyawa();
            
            float arahPental = (transform.position.x < collision.transform.position.x) ? -3f : 3f;
            rb.linearVelocity = new Vector2(arahPental, 5f);

            if (nyawa <= 0)
            {
                RestartGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ikan"))
        {
            ikanTerkumpul++;
            
            // TAMPILKAN SKOR KE LAYAR
            if (teksSkor != null)
            {
                teksSkor.text = "Ikan: " + ikanTerkumpul;
            }

            if (nyawa < maxNyawa)
            {
                nyawa++;
                UpdateUINyawa();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Tulang"))
        {
            punyaTulang = true;
            Debug.Log("Tulang didapat! Portal Terbuka!");
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Finish"))
        {
            if (punyaTulang)
            {
                Debug.Log("LEVEL SELESAI! MENANG!");
                
                // --- KODE LANGKAH 3 (MEMBERIKAN KUNCI KE PLAYERPREFS) ---
                // Menyimpan angka 1 ke nama kunci yang sudah kamu ketik di Inspector
                if (!string.IsNullOrEmpty(namaKunciBukaLevel))
                {
                    PlayerPrefs.SetInt(namaKunciBukaLevel, 1);
                    PlayerPrefs.Save();
                }
                // ---------------------------------------------------------

                panelMenang.SetActive(true); 
                Time.timeScale = 0f; 
            }
            else
            {
                Debug.Log("Dapetin tulang dulu woi!");
            }
        }
    }

    void UpdateUINyawa()
    {
        for (int i = 0; i < ikonNyawa.Length; i++)
        {
            if (i < nyawa)
                ikonNyawa[i].SetActive(true);
            else
                ikonNyawa[i].SetActive(false);
        }
    }

    void KucingJatuh()
    {
        nyawa--; 
        UpdateUINyawa(); 

        if (nyawa <= 0)
        {
            RestartGame();
        }
        else
        {
            if (titikRespawn != null)
            {
                transform.position = titikRespawn.position;
                rb.linearVelocity = Vector2.zero; 
            }
            else
            {
                Debug.LogWarning("Titik Respawn belum dimasukkan ke Inspector!");
                RestartGame(); 
            }
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TombolKembaliKeSampleScene()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("SampleScene"); 
    }
}