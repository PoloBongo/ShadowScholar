using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonPlay : MonoBehaviour
{
    public Button playButton;      // Référence au bouton "Jouer"
    public Slider loadingSlider;   // Référence au slider de progression

    private void Start()
    {
        // Ajoutez un listener au bouton pour lancer la méthode LoadSceneAsync
        playButton.onClick.AddListener(() => StartCoroutine(LoadSceneAsync("SceneChargement")));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Activer le slider avant de commencer le chargement
        loadingSlider.gameObject.SetActive(true);

        // Démarrer le chargement asynchrone de la scène
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Empêcher la scène de se charger immédiatement après être prête
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;

        // Tant que le chargement n'est pas terminé
        while (!operation.isDone)
        {
            // Simuler la progression du slider jusqu'à ce que le chargement réel atteigne 90%
            if (operation.progress < 0.9f)
            {
                fakeProgress = Mathf.MoveTowards(fakeProgress, operation.progress, Time.deltaTime);
                loadingSlider.value = fakeProgress;
            }
            else
            {
                // Si le chargement réel est terminé, simuler le passage à 100%
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
