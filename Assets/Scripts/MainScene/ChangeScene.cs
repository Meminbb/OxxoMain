using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    [SerializeField] GameObject EventButton;
    [SerializeField] GameObject Player;
    bool isCajero = false;
    bool isTrivia = false;
    bool isRitmo = false;
    void ChangeCajero(){
        SceneManager.LoadScene("Cajero");
    }

    void ChangeTrivia(){
        SceneManager.LoadScene("TriviaScene");
    }

    void ChangeRitmo(){
        SceneManager.LoadScene("RitmoScene");
    }

    public void pushButton(){
        if (isCajero){
            ChangeCajero();
        } else if (isTrivia){
            ChangeTrivia();
        } else if (isRitmo){
            ChangeRitmo();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        EventButton.SetActive(true);
        if (collision.CompareTag("Cajero")){
            isCajero = true;
        } else if (collision.CompareTag("Npc")){
            isTrivia = true;
        } else if (collision.CompareTag("Charco")){
            isRitmo = true;
        } else if (collision.CompareTag("Salida")){
            SceneManager.LoadScene("Menu");
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        EventButton.SetActive(false);
        isCajero = false;
        isTrivia = false;
        isRitmo = false;
    }
}
