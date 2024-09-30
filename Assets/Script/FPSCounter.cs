using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;  // R�f�rence a l'objet TextMeshPro pour afficher les FPS
    private int frames = 0;
    private float time = 0.0f;
    private float refreshTime = 1.0f;  // Intervalle de temps pour le rafra�chissement des FPS affich�s

    void Update()
    {
        frames++;
        time += Time.deltaTime;

        if (time >= refreshTime)
        {
            int fps = Mathf.RoundToInt(frames / time);
            fpsText.text = "FPS: " + fps;
            frames = 0;
            time = 0.0f;
        }
    }
}
