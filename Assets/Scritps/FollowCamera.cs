using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform playerTransform;
    public Vector3 offset;
    void Start()
    {
        //Buscamos el componente transform por medio del Tag Player usando el mÃ©todo FindWithTag
        playerTransform = GameObject.FindWithTag("Player").transform; 
    }

    
    void LateUpdate()
    {
        //Copia las coordenadas del jugador + unos valores adicionales que permiten posicionarla mejor (offset)
        transform.position = playerTransform.position + offset;
        //2D
        //transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}