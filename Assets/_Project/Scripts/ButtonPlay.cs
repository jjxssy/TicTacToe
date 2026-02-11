using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour
{
    public void ClickPlay()
    {
        SceneManager.LoadSceneAsync("Level01");
    }
   
}
