// File: GeneticOptimizer.cs
// Date: 2013 6 17
// Author: Brien Smith-Martinez
// Summary:
// Contains an implementation of the Junction Solutions GeneticOptimizer,
// a genetic algorithm for finding production schedules.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

using TestSimpleRNG;


// Job Scheduling Problem description:
//
// ** Assumptions about Jobs:
// * Precedence and overlap
// Jobs have precedence relationships. This means one job cannot be started until another job is complete.
// However, overlap is possible: a job may be started when its predecessor is only partially complete
// * Temporal restrictions
// Jobs may be constrained such that they can only be done at a certain time.
//
// ** Assumptions about Resources:
// * Temporal restrictions
// Resources may be constrained that limit their use to a certain time or time period
// * Job-Resource dependencies
// Resoruces may be constrained depending on the type of work they are assigned to
// 
// ** Assumptions about Objectives:
// * Minimal makespan
// Reduce the time it takes to complete the jobs without violating constraints
// * Minimize cost
// There are other penalties... todo: define penalties and costs.

namespace Junction
{
    /// <summary>
    /// Third GA attempt. Includes the following improvements:
    /// - Include Usage Modes (only used to represent resource usage atm)
    /// - Better OOP
    /// Didn't just replace the last one because I want to have them both available for comparison.
    /// </summary>
    namespace GeneticOptimizer
    {
        // Genetic Operator Flags:
        public enum RealCrossoverOp { Uniform, MeanWithNoise }
        public enum SurvivalSelectionOp { ReplaceWorst, Elitist, Generational, Struggle }
        public enum ParentSelectionOp { Tournament, FitnessProportional }

        public class GA
        {
            public Func<int[], double[], int[], double> FitnessFunction { get; set; }
            public ScheduleGenome[] population;
            private ScheduleGenome[] offspring;
            public ScheduleGenome elite;
            static private Random _rand;
            static private SimpleRNG _srng;
            // Generic GA parameters:
            private int _seed;
            private int _length;
            private int _tl;
            private int _mModes;
            private int _popsize;
            private int _offsize;
            private double _mutationRate;
            private double _deathRate;
            // Genetic Operator Flags:
            public ParentSelectionOp parentSelection { get; set; }
            public RealCrossoverOp realCrossover { get; set; }
            public SurvivalSelectionOp survivalSelection { get; set; }
            // Problem specific parameters:
            private double _delayMean;
            double _delayRate;
            //double _delayVar;
            public GA(int seed, int length, int tl, int modes, int popsize, int offsize, double mutationRate, double deathRate, double delayRate, double delayMean)
            {
                _seed = seed;
                _rand = new Random(_seed);
                _srng = new SimpleRNG((uint)_seed);
                _length = length;
                _tl = tl;
                _mModes = modes;
                _popsize = popsize;
                _offsize = offsize;
                _mutationRate = mutationRate;
                _deathRate = deathRate;
                _delayRate = delayRate;
                _delayMean = delayMean;
                population = new ScheduleGenome[_popsize];
                offspring = new ScheduleGenome[_offsize];
                // Default operator options:
                realCrossover = RealCrossoverOp.MeanWithNoise;
                survivalSelection = SurvivalSelectionOp.Elitist;
                parentSelection = ParentSelectionOp.Tournament;

                elite = new ScheduleGenome(_length, tl, modes, mutationRate, delayRate, delayMean);
                for (int i = 0; i < popsize; i++)
                {
                    population[i] = new ScheduleGenome(length, tl, modes, mutationRate, _delayRate, _delayMean);
                    population[i].realCrossover = realCrossover;
                }
                for (int i = 0; i < offsize; i++)
                {
                    offspring[i] = new ScheduleGenome(length, tl, modes, mutationRate, _delayRate, _delayMean);
                    offspring[i].realCrossover = realCrossover;
                }

            }
            public void SeedPopulation(int[] genes, double[] times)
            {
                population[0] = new ScheduleGenome(_length, _tl, _mModes, _mutationRate, genes, times);
            }
            public double AverageFitness()
            {
                double avg = 0;
                for (int i = 0; i < _popsize; i++)
                {
                    avg += population[i].fitness;
                }
                avg = avg / _popsize;
                return avg;
            }
            public void GenRand()
            {
                population[0].GenRand(); //TestSimpleRNG.SimpleRNG.GetExponential();
            }

            // Copies best individual in population to elite.
            // Assumes population has been evaluated. Use with caution.
            public void FindElite()
            {
                elite.fitness = FitnessFunction(elite.Genes, elite.Times, elite.Modes);
                for (int i = 0; i < _popsize; i++)
                {
                    if (population[i].fitness > elite.fitness)
                    {
                        elite.Copy(population[i]);
                    }
                }
            }

            public void EvaluatePopulation()
            {
                elite.fitness = FitnessFunction(elite.Genes, elite.Times, elite.Modes);
                for (int i = 0; i < _popsize; i++)
                {
                    population[i].fitness = FitnessFunction(population[i].Genes, population[i].Times, population[i].Modes);
                    if (population[i].fitness > elite.fitness)
                    {
                        elite.Copy(population[i]);
                    }
                }

            }

            public void GenerateOffspring()
            {
                if (survivalSelection == SurvivalSelectionOp.ReplaceWorst)
                {
                    _offsize = (int)(_popsize * _deathRate);
                }
                for (int i = 0; i < _offsize; i += 2)
                {
                    // Select two parents
                    int p1 = SelectParent();
                    int p2 = SelectParent();

                    // Crossover to create two offspring
                    population[p1].Crossover(ref population[p2], ref offspring[i]);
                    population[p2].Crossover(ref population[p1], ref offspring[i + 1]);

                    // Mutation chance
                    offspring[i].Mutate();
                    Debug.Assert(offspring[i].IsValid(), "Invalid mutation");
                    offspring[i + 1].Mutate();
                    Debug.Assert(offspring[i + 1].IsValid(), "Invalid mutation");

                }
                int threadcount = 20;
                int ck = 0;
                while (ck < _offsize)
                {
                    ManualResetEvent[] doneEvents = new ManualResetEvent[threadcount];
                    for (int k = 0; k < threadcount; k++)
                    {
                        if (ck < _offsize)
                        {
                            doneEvents[k] = new ManualResetEvent(false);
                            FitnessEvaluationThread f = new FitnessEvaluationThread(ck, this, doneEvents[k]);
                            ThreadPool.QueueUserWorkItem(f.Evaluate);
                            ck++;
                        }
                    }
                    //WaitHandle.WaitAll(doneEvents);
                    for (int i = 0; i < threadcount; i++)
                    {
                        if (doneEvents[i] != null)
                        {
                            doneEvents[i].WaitOne();
                        }
                    }
                }

            }
            public class FitnessEvaluationThread
            {
                public Func<int[], double[], double> FitnessFunction { get; set; }
                GA _reftoGO;
                // public double Fitness { get { return _fitness; } }
                // private double _fitness;
                private int _i;
                private ManualResetEvent _doneEvent;
                public FitnessEvaluationThread(int i, GA reftoGO, ManualResetEvent doneEvent)
                {
                    _reftoGO = reftoGO;
                    _i = i;
                    _doneEvent = doneEvent;
                }
                public void Evaluate(Object threadContext)
                {
                    _reftoGO.offspring[_i].fitness = _reftoGO.FitnessFunction(_reftoGO.offspring[_i].Genes, _reftoGO.offspring[_i].Times, _reftoGO.offspring[_i].Modes);
                    _doneEvent.Set();
                }

            }
            private int SelectParent()
            {
                int p = 0;
                switch (parentSelection)
                {
                    case ParentSelectionOp.FitnessProportional:
                        double totalFitness = 0;
                        for (int i = 0; i < _popsize; i++)
                        {
                            totalFitness += population[i].fitness;
                        }
                        double r = _rand.NextDouble() * totalFitness;
                        double runningTotal = population[p].fitness;
                        while (runningTotal > r)
                        {
                            p++;
                            runningTotal += population[p].fitness;
                        }
                        break;
                    case ParentSelectionOp.Tournament:
                        int k = _popsize / 10;
                        p = _rand.Next(_popsize);
                        double bestfitness = population[p].fitness;
                        for (int i = 0; i < k; i++)
                        {
                            int px = _rand.Next(_popsize);
                            if (population[px].fitness > bestfitness)
                            {
                                bestfitness = population[px].fitness;
                                p = px;
                            }
                        }
                        break;
                }

                return p;
            }

            public void SurvivalSelection()
            {
                switch (survivalSelection)
                {
                    case SurvivalSelectionOp.ReplaceWorst:
                        // Replace worst _deathRate * populationSize with offspring.
                        Array.Sort(population, new NewComp());
                        int cutpoint = (int)(_popsize * (1.0 - _deathRate));
                        for (int i = cutpoint; i < _popsize; i++)
                        {
                            population[i].Copy(offspring[i - cutpoint]);
                        }
                        break;
                    case SurvivalSelectionOp.Elitist:
                        // Elitist survival selection:
                        List<ScheduleGenome> combo = new List<ScheduleGenome>();
                        combo.AddRange(population);
                        combo.AddRange(offspring);
                        combo.Sort(new NewComp());
                        for (int i = 0; i < _popsize; i++)
                        {
                            population[i].Copy(combo[i]);
                        }
                        break;
                    case SurvivalSelectionOp.Generational:
                        // Generational survival selection:
                        for (int i = 0; i < _popsize; i++)
                        {
                            population[i].Copy(offspring[i]);
                        }
                        //Array.Sort(population, new NewComp());
                        break;
                    case SurvivalSelectionOp.Struggle:
                        // Struggle survival selection:
                        // Replace Most-Similar if new is better
                        int replaced = 0;
                        for (int i = 0; i < _popsize; i++)
                        {
                            double d = 999999;
                            int replacementIndex = -1;
                            for (int j = 0; j < _popsize; j++)
                            {
                                // Find most similar
                                double newd = 0;
                                if (offspring[i].fitness > population[j].fitness)
                                {
                                    newd = offspring[i].Distance(population[j]);
                                    if (newd < d)
                                    {
                                        d = newd;
                                        replacementIndex = j;
                                    }
                                }
                            }
                            if (replacementIndex > -1)
                            {
                                population[replacementIndex].Copy(offspring[i]);
                                replaced++;
                            }
                        }
                        Debug.Write(Environment.NewLine + "Replaced: " + replaced);
                        break;
                }
                /*
                // Percent replacement:
                Array.Sort(population, new NewComp());
                Array.Sort(offspring, new NewComp());
                int cutpoint = (int)(_popsize * _deathRate);
                for (int i = cutpoint; i < _popsize; i++)
                {
                    population[i].Copy(offspring[i - cutpoint]);
                }
                */

            }

            public class ScheduleGenome
            {
                public int[] Genes;
                public double[] Times;
                public int[] Modes;
                public int maxModes;
                private double _mutationRate;
                public double fitness;
                public int _timesLength;
                public int _length;
                public RealCrossoverOp realCrossover;


                public ScheduleGenome(int length, int tl, int mModes, double mut, int[] geneSeeds, double[] timeSeeds)
                {
                    _length = length;
                    Genes = new int[length];
                    Times = new double[tl];
                    Modes = new int[tl];
                    maxModes = mModes;
                    _timesLength = tl;
                    _mutationRate = mut;
                    for (int i = 0; i < length; i++)
                    {
                        Genes[i] = geneSeeds[i];
                    }
                    for (int i = 0; i < tl; i++)
                    {
                        Times[i] = timeSeeds[i];
                    }
                    fitness = -1;
                }

                public ScheduleGenome(int length, int tl, int mModes, double mut, double delayRate, double delayMean)
                {
                    Genes = new int[length];
                    Times = new double[tl];
                    Modes = new int[tl];
                    maxModes = mModes;
                    _length = length;
                    _timesLength = tl;
                    _mutationRate = mut;
                    List<int> randarray = new List<int>();
                    for (int i = 0; i < length; i++)
                    {
                        randarray.Add(i);
                    }
                    for (int i = 0; i < length; i++)
                    {
                        int r = _rand.Next(0, length - i);
                        Genes[i] = randarray[r];
                        randarray.RemoveAt(r);
                    }
                    fitness = -1;
                    for (int i = 0; i < tl; i++)
                    {
                        if (_rand.NextDouble() < delayRate)
                        {
                            Times[i] = SimpleRNG.GetExponential(delayMean);
                        }
                        else
                        {
                            Times[i] = 0.0;
                        }
                        Modes[i] = _rand.Next(maxModes);
                    }
                }

                public void Copy(ScheduleGenome c)
                {
                    fitness = c.fitness;
                    for (int i = 0; i < Genes.Length; i++)
                    {
                        Genes[i] = c.Genes[i];
                        if (i < _timesLength)
                        {
                            Times[i] = c.Times[i];
                            Modes[i] = c.Modes[i];
                        }
                    }
                }

                public double Distance(ScheduleGenome c)
                {
                    double d = 0;
                    for (int i = 0; i < _timesLength; i++)
                    {
                        d += Math.Pow(Times[i] - c.Times[i], 2.0);
                    }
                    return Math.Sqrt(d);
                }

                public void GenRand()
                {
                    Debug.Write(Environment.NewLine + _rand.Next(Genes.Length));
                }
                public void DTCrossover(ref ScheduleGenome p2, ref ScheduleGenome o1) //out ScheduleGenome o2)
                {
                    switch (realCrossover)
                    {
                        case RealCrossoverOp.MeanWithNoise:
                            // Mean-with-noise Crossover:
                            for (int i = 0; i < _timesLength; i++)
                            {
                                double mean = Times[i] + p2.Times[i];
                                mean = mean / 2.0;
                                o1.Times[i] = SimpleRNG.GetNormal(mean, 0.5);
                                if (o1.Times[i] < 0.0)
                                {
                                    o1.Times[i] = 0.0;
                                }
                            }
                            break;
                        case RealCrossoverOp.Uniform:
                            // Uniform Crossover:
                            int cutpoint = _rand.Next(_timesLength + 1);
                            for (int i = 0; i < cutpoint; i++)
                            {
                                o1.Times[i] = Times[i];
                            }
                            for (int i = cutpoint; i < _timesLength; i++)
                            {
                                o1.Times[i] = p2.Times[i];
                            }
                            break;
                    }
                }

                public void Crossover(ref ScheduleGenome p2, ref ScheduleGenome o1)
                {
                    int cutpoint = _rand.Next(_length);

                    //Debug.Write(Environment.NewLine + "Distance between " + p1 + " - " + p2 + ": " + population[p1].Distance(population[p2]));

                    for (int i = 0; i < _length; i++)
                    {
                        o1.Genes[i] = Genes[i];
                    }

                    Debug.Assert(o1.IsValid(), "Invalid creature before crossover");

                    List<int> remainder = new List<int>(p2.Genes);
                    for (int i = 0; i < cutpoint; i++)
                    {
                        remainder.Remove(Genes[i]);
                    }
                    for (int i = cutpoint; i < _length; i++)
                    {
                        o1.Genes[i] = remainder[i - cutpoint];
                    }

                    Debug.Assert(o1.IsValid(), "Invalid creature after crossover");

                    DTCrossover(ref p2, ref o1);
                    CombinationCrossover(ref p2, ref o1);
                }
                public void CombinationCrossover(ref ScheduleGenome p2, ref ScheduleGenome o1)
                {
                    int cutpoint = _rand.Next(_timesLength);

                    for (int i = 0; i < cutpoint; i++)
                    {
                        o1.Modes[i] = p2.Modes[i];
                    }
                    for (int i = cutpoint; i < _timesLength; i++)
                    {
                        o1.Modes[i] = Modes[i];
                    }
                }
                public void Mutate()
                {
                    for (int i = 0; i < _length; i++)
                    {
                        if (_rand.NextDouble() < _mutationRate)
                        {
                            int r = _rand.Next(_length);
                            int temp = Genes[i];
                            Genes[i] = Genes[r];
                            Genes[r] = temp;
                            // Mutate the delay time
                            r = _rand.Next(_timesLength);
                            double mutatedDelay = 0;
                            //mutatedDelay = SimpleRNG.GetExponential(_delayMean);
                            mutatedDelay = SimpleRNG.GetNormal(Times[r], 1.0);
                            //mutatedDelay = _rand.NextDouble() * _delayMean;
                            if (mutatedDelay < 0.0)
                            {
                                mutatedDelay = 0.0;
                            }
                            Times[r] = mutatedDelay;
                        }
                    }
                    //Mutate the Mode vector:
                    for (int i = 0; i < _timesLength; i++)
                    {
                        if (_rand.NextDouble() < _mutationRate)
                        {
                            Modes[i] = _rand.Next(maxModes);
                        }
                    }

                }
                public bool IsValid()
                {
                    List<int> vals = new List<int>();
                    bool r = true;
                    foreach (int i in Genes)
                    {
                        if (vals.Contains(i))
                        {
                            r = false;
                            break;
                        }
                        vals.Add(i);
                    }
                    return r;
                }

            }

            public sealed class NewComp : IComparer<ScheduleGenome>
            {
                public int Compare(ScheduleGenome x, ScheduleGenome y)
                {
                    return (y.fitness.CompareTo(x.fitness));
                }
            }

        }
    }
}