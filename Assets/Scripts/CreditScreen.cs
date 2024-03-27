using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScreen : MonoBehaviour
{
    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }
}
