using UnityEngine;
using UnityEngine.UI;

public class KunciLevel : MonoBehaviour
{
    public string namaKunci; // Nanti tinggal kita ketik di Unity

    void Start()
    {
        // Kalau kuncinya udah dapet (nilainya 1), tombolnya otomatis bisa diklik
        if (PlayerPrefs.GetInt(namaKunci, 0) == 1)
        {
            GetComponent<Button>().interactable = true;
        }
    }
}