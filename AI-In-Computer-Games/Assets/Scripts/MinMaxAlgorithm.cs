using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinMaxAlgorithm : MonoBehaviour
{
    /* Left is always attacking,right is healing*/

    public int attackingCounter=0;
    public int healingCounter=0;

    private int absolute = 0;  
    public bool controlVariable=false;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Unit playerUnit;
    Unit enemyUnit;

    public float playerUnitattackingPoint, enemyUnitattackingPoint;
    public float playerUnitSituationValue, enemyUnitSituationValue,gameSituationValue;

    private float randomNumberforPlayer, randomNumberforEnemy,randomNumberforGeneral;

    private float temporary;
    private float temporaryNumber=1;

    private float[,] temp;

    public float[] enemyFirstMinMax;
    public float[] playerSecondMinMax;
    public float[] enemyThirdMinMax;


    // Start is called before the first frame update
    void Start()
    {
        

        GameObject playerGameObject = playerPrefab;
        playerUnit = playerGameObject.GetComponent<Unit>();

        GameObject enemyGameObject = enemyPrefab;
        enemyUnit = enemyGameObject.GetComponent<Unit>();

        enemyFirstMinMax = new float[10];
        playerSecondMinMax = new float[10];
        enemyThirdMinMax = new float[10];
    }

    // Update is called once per frame
    void Update()
    {

        absolute = Mathf.Abs(attackingCounter - healingCounter);
        GameObject playerGameObject  = GameObject.FindGameObjectWithTag("minMaxPlayer");
        playerUnit = playerGameObject.GetComponent<Unit>();

        GameObject enemyGameObject = GameObject.FindGameObjectWithTag("minMaxEnemy");
        playerUnit = enemyGameObject.GetComponent<Unit>();

        //Random.seed = (int)System.DateTime.Now.Ticks;
        //randomNumber = Random.Range(0, 5);
        // (damage+rand(0,5) * level) *0.5f +current health *0.5
    }

    public float SituationCalculationForPlayer()
    {
        playerUnitattackingPoint = playerUnit.damage + playerUnit.unitLevel; // between 20-30
        playerUnitSituationValue = (playerUnitattackingPoint * 0.5f) + ((playerUnit.currentHP) * 0.5f);


       /* randomNumberforPlayer = Random.Range(1, 5);

        playerUnitattackingPoint = (Mathf.Pow((playerUnit.damage + randomNumberforPlayer),playerUnit.unitLevel)); //between 20 - 30
        //Debug.Log("random sayý player:" + randomNumberforPlayer);
       // Debug.Log("Player attacking point:" + playerUnitattackingPoint);
        playerUnitSituationValue = (playerUnitattackingPoint * 0.5f) + ((playerUnit.currentHP) * 0.5f ); */

       // Debug.Log("Player:" + enemyUnitSituationValue);
        return playerUnitSituationValue;
    }



    public float SituationCalculationForEnemy()
    {
        enemyUnitattackingPoint = enemyUnit.damage+enemyUnit.unitLevel; // between 20-30
        enemyUnitSituationValue = (enemyUnitattackingPoint * 0.5f) + ((enemyUnit.currentHP) * 0.5f);

        /* randomNumberforEnemy = Random.Range(1, 5);

        enemyUnitattackingPoint = (Mathf.Pow((enemyUnit.damage + randomNumberforEnemy), enemyUnit.unitLevel)); // between 20-30
        //Debug.Log("random sayý enemy:" + randomNumberforEnemy);
       // Debug.Log("Enemy attacking point:" + enemyUnitattackingPoint);
        enemyUnitSituationValue = (enemyUnitattackingPoint * 0.5f) + ((enemyUnit.currentHP) * 0.5f); */

        //Debug.Log("Enemy:" + enemyUnitSituationValue);
        return enemyUnitSituationValue;
    }

    public float SituationCalculatorForGeneral (int damage,int level,int health)
    {
        temporary = damage + level; //between 20 - 30
        temporary = (temporary * 0.5f) + ((health) * 0.5f);

        /* randomNumberforGeneral = Random.Range(1, 5);
        

        temporary = (Mathf.Pow((damage + randomNumberforGeneral), level)); //between 20 - 30
        temporary = (temporary * 0.5f) + ((health) * 0.5f); */

        return temporary;
    }
    public float SituationCalculationForGame ()
    {
        gameSituationValue = ( SituationCalculationForEnemy() / SituationCalculationForPlayer());
       // Debug.Log("Game Situation Value" + gameSituationValue);
        return gameSituationValue;
    }
    public bool MinMaxCalc()
    {
        /* Left is always attacking,right is healing*/


        enemyFirstMinMax[0] = SituationCalculationForGame(); // getting current situation stats, do not need any modification
        //Debug.Log("(0,1)=" + enemyFirstMinMax[0]);
        playerSecondMinMax[1] = SituationCalculationForEnemy()/SituationCalculatorForGeneral(playerUnit.damage,playerUnit.unitLevel,TakeHit(enemyUnit.damage,enemyUnit.unitLevel,playerUnit.currentHP));
        playerSecondMinMax[2] = SituationCalculatorForGeneral(enemyUnit.damage, enemyUnit.unitLevel, SelfHealing(enemyUnit.currentHP,enemyUnit.healPoint)) / SituationCalculationForPlayer();
        enemyThirdMinMax[1] =  SituationCalculatorForGeneral(enemyUnit.damage, enemyUnit.unitLevel, TakeHit(playerUnit.damage,playerUnit.unitLevel,enemyUnit.currentHP) ) / SituationCalculatorForGeneral(playerUnit.damage, playerUnit.unitLevel, TakeHit(enemyUnit.damage,enemyUnit.unitLevel,playerUnit.currentHP));
        enemyThirdMinMax[2] = SituationCalculationForEnemy()/ SituationCalculatorForGeneral(playerUnit.damage, playerUnit.unitLevel, SelfHealing(TakeHit(enemyUnit.damage, enemyUnit.unitLevel, playerUnit.currentHP),playerUnit.healPoint));
        enemyThirdMinMax[3] = SituationCalculatorForGeneral(enemyUnit.damage, enemyUnit.unitLevel, TakeHit(playerUnit.damage,playerUnit.unitLevel, SelfHealing(enemyUnit.currentHP, enemyUnit.healPoint)) )  / SituationCalculationForPlayer();
        enemyThirdMinMax[4] = SituationCalculatorForGeneral(enemyUnit.damage, enemyUnit.unitLevel, SelfHealing(enemyUnit.currentHP,enemyUnit.healPoint)) / SituationCalculatorForGeneral(playerUnit.damage, playerUnit.unitLevel, SelfHealing(playerUnit.currentHP,playerUnit.healPoint));

        for(int i=1;i<5;i++)
         {
             Debug.Log(i+".Node:"+100*enemyThirdMinMax[i]);
         }  

        #region Health
         if(absolute>=2)
        {
            controlVariable = true;
            absolute = 0;
        }

         else
        {
            controlVariable = false;
        }
        #endregion Health

        if (enemyThirdMinMax[1] < enemyThirdMinMax[2])
            playerSecondMinMax[1] = enemyThirdMinMax[1];
        else
            playerSecondMinMax[1] = enemyThirdMinMax[2];



        if (enemyThirdMinMax[3] < enemyThirdMinMax[4])
            playerSecondMinMax[2] = enemyThirdMinMax[3];
        else
            playerSecondMinMax[2] = enemyThirdMinMax[4];



        if ((playerSecondMinMax[1] > playerSecondMinMax[2] ) )/* &&  !controlVariable  ) */
        {
            enemyFirstMinMax[0] = playerSecondMinMax[1];
            Debug.Log("Initial move:"+enemyFirstMinMax[0]*100);
            attackingCounter++;
            return true;
        }
        else
        {
            enemyFirstMinMax[0] = playerSecondMinMax[2];
            Debug.Log("Initial move:" + enemyFirstMinMax[0]*100);
            healingCounter++;
            return false;
        }
       
    }


    public int TakeHit(int opponentDamagePoint,int opponentUnitLevel,int selfHealthPoint)
    {
        opponentDamagePoint = opponentDamagePoint + opponentUnitLevel;
        selfHealthPoint -= opponentDamagePoint;

        /* opponentDamagePoint = Random.Range(opponentDamagePoint - 3 + opponentUnitLevel, opponentDamagePoint + 3 + opponentUnitLevel);
        selfHealthPoint -= opponentDamagePoint; */

        return selfHealthPoint; 

       /* if (unitHealthPoint <= 0)
            return true;
        else
            return false; */

    }

    public int SelfHealing(int currentHealth,int healthPoint) 
    {
         healthPoint = healthPoint + 5;
       currentHealth += healthPoint; 

        /* healthPoint = Random.Range(healthPoint - 5, healthPoint + 5);
         currentHealth += healthPoint; */

        if (currentHealth >= playerUnit.maxHP)
            currentHealth = playerUnit.maxHP;

        return currentHealth;
    }

    public void PrintMinMaxTree()
    {

        MinMaxCalc();
    }

   

}
