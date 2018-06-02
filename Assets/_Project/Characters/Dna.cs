using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dna {

    #region Properties
    public List<int> Genes
    {
        get { return genes; }
    }
    #endregion

    List<int> genes = new List<int>();
    int dnaLength = 0;
    int maxValues = 0;

    #region Constructors
    public Dna (int length, int values)
    {
        dnaLength = length;
        maxValues = values;
        SetRandom();
    }
    #endregion

    #region Public Methods

    public void SetRandom ()
    {
        genes.Clear();
        for (var i = 0; i < dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    public void SetInt (int position, int value)
    {
        genes[position] = value;
    }

    public void Combine (Dna parent1, Dna parent2)
    {
        for(var i = 0; i < dnaLength; i++)
        {
            if(i < dnaLength / 2.0f)
            {
                int chromosome = parent1.genes[i];
                genes[i] = chromosome;
            }
            else
            {
                int chromosome = parent2.genes[i];
                genes[i] = chromosome;
            }
        }
    }

    public void Mutate ()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }
    #endregion
}
