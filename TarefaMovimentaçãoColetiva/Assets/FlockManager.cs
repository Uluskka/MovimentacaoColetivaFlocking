using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //metodo para distribuir os peixes, aplicar seu numero e limitar o quanto eles podem nadar.
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    //metodo para configurar o cardume sua velocidade  e distancia em que ele faz sua rota.
    [Header("Configuraçao do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {    
        allFish = new GameObject[numFish];
        //Loop para todos os peixes
        for(int i = 0; i <numFish; i++)
        {
            //metodo para modificar a posicao, mais a movimentacao sendo o limite da movimentacao sendo positivo e o negativo aleatoria eixo sendo x,y e z.
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
                //instancia os peixes no array.
                allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
                allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = this.transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
    }
}
