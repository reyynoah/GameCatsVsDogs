using UnityEngine;
using UnityEngine.SceneManagement; // Ini wajib biar bisa pindah scene

public class MenuManager : MonoBehaviour
{
    // Fungsi ini yang bakal dipanggil pas tombol diklik
    public void PindahKeScene(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }

    // Fungsi tambahan buat tombol "Keluar" nanti
    public void KeluarGame()
    {
        Debug.Log("Game Keluar!");
        Application.Quit();
    }
}