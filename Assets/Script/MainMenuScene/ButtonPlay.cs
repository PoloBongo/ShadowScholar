using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonPlay : MonoBehaviour
{
    public Button playButton;      // R�f�rence au bouton "Jouer"
    public Slider loadingSlider;   // R�f�rence au slider de progression

    private void Start()
    {
        // Ajoutez un listener au bouton pour lancer la m�thode LoadSceneAsync
        playButton.onClick.AddListener(() => StartCoroutine(LoadSceneAsync("SceneChargement")));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Activer le slider avant de commencer le chargement
        loadingSlider.gameObject.SetActive(true);

        // D�marrer le chargement asynchrone de la sc�ne
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Emp�cher la sc�ne de se charger imm�diatement apr�s �tre pr�te
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;

        // Tant que le chargement n'est pas termin�
        while (!operation.isDone)
        {
            // Simuler la progression du slider jusqu'� ce que le chargement r�el atteigne 90%
            if (operation.progress < 0.9f)
            {
                fakeProgress = Mathf.MoveTowards(fakeProgress, operation.progress, Time.deltaTime);
                loadingSlider.value = fakeProgress;
            }
            else
            {
                // Si le chargement r�el est termin�, simuler le passage � 100%
                loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, 1f, Time.deltaTime);

                if (loadingSlider.value >= 1f)
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null; // Attendre la frame suivante
        }
    }
}
