using UnityEngine.SceneManagement;

namespace Common
{
    public class SceneReloader
    {
        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}