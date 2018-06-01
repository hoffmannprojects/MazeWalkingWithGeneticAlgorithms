using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    #region Properties
    public static float TimeElapsed { get; private set; } 
    #endregion

    [SerializeField] private GameObject botPrefab;
    [SerializeField] private int populationSize = 50;
    [SerializeField] private float trialTime = 5;

    private List<GameObject> population = new List<GameObject>();
    private int generation = 1;

    private GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start ()
    {
        TimeElapsed = 0f;

		for(var i = 0; i < populationSize; i++)
        {
            Vector3 startingPosition = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));
            GameObject bot = Instantiate(botPrefab, startingPosition, this.transform.rotation);

            bot.GetComponent<Brain>().Init();
            population.Add(bot);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        TimeElapsed += Time.deltaTime;
        if(TimeElapsed >= trialTime)
        {
            BreedNewPopulation();
            TimeElapsed = 0;
        }
	}

    private void BreedNewPopulation ()
    {
        // Population sorted by fittest last.
        // Fitness weighted by TimeSpentWalking * 5 and TimeAlive * 1.
        List<GameObject> sortedPopulation = population.OrderBy(o => (o.GetComponent<Brain>().TimeSpentWalking * 5+ o.GetComponent<Brain>().TimeAlive)).ToList();
        population.Clear();

        // Breed second half of list (fittest).
        for(var i = (int) (sortedPopulation.Count / 2.0f) - 1; i < sortedPopulation.Count -1; i++)
        {
            population.Add(Breed(sortedPopulation[i], sortedPopulation[i + 1]));
            population.Add(Breed(sortedPopulation[i + 1], sortedPopulation[i]));
        }

        // Destroy all parents and previous population.
        for(var i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }
        generation++;
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        var startingPosition = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));

        GameObject offspring = Instantiate(botPrefab, startingPosition, Quaternion.identity);
        Brain brain = offspring.GetComponent<Brain>();

        if(Random.Range(0, 100) == 1)
        // Mutate 1 in 100.
        {
            brain.Init();
            brain.Dna.Mutate();
        }
        else
        // Normal combination of parent Dna.
        {
            brain.Init();
            brain.Dna.Combine(parent1.GetComponent<Brain>().Dna, parent2.GetComponent<Brain>().Dna);
        }
        return offspring;
    }

    private void OnGUI ()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", TimeElapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }
}
