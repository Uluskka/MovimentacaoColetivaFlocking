using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //metodo de movimentacao.
    public FlockManager myManager; //puxa os metodos necessarios.
    //velociade do peixe
    public float speed;
    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //Metodo para deixa aleatoridade do Speed.
        speed = Random.Range(myManager.minSpeed,
            myManager.maxSpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2); //informa o limite que os peixes tem para continuar sua rota.
        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), 
                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if(Random.Range(0, 100)< 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);

    }
     void ApplyRules() //metodo para aplicar regras.
    {
        //array dos peixes
        GameObject[] gos;
        gos = myManager.allFish;
        Vector3 vcenter = Vector3.zero; //calculo central.
        Vector3 vavoid = Vector3.zero;  
        float gSpeed = 0.01f; //variavel para usar a velocidades de suas rotacoes do grupo.
        float nDistance; // calcula a distancia dos outros peixes.
        int groupSize = 0; //ajusta o agrupamento do cardume e seu tamanho.

        //loop.
        foreach(GameObject go in gos)
        {
            //metodo para calcular a distancia e a central para cada um dos peixes.
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= myManager.neighbourDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;


                    if(nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position); //informa que a posicao de cada um dos peixes nao pode ser igual.
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }

            }
        }
        //metodo para apalicar as informacoes de rotacao e movimentacao.
        if (groupSize > 0)
        {
            //metodo para zerar o valor do centro e divide pelo tamanho do grupo.
            vcenter = vcenter / groupSize + (myManager.goalPos - this.transform.position);
            //velocidade para dividir o grupo.
            speed = gSpeed / groupSize; //metodo para fazer a direcao ser a soma do valor até o centro mais o valor de desvio menos a posiçao atual.

            //informa a direcao calculada pelo vcenter para que possa seguir uma nova rota a ser aplicada.
            Vector3 direction = (vcenter + vavoid) - transform.position;
            //se informa que a direcao é diferente de zero ele muda a rotacao do peixe para nao bater.
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime);
        }


    }

}
