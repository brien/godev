// File: GeneticOptimizer.cs
// Date: 2013 6 17
// Author: Brien Smith-Martinez
// Summary:
// Contains an implementation of the Junction Solutions GeneticOptimizer,
// a genetic algorithm for finding production schedules.
//
// History:
// Version: 0.2
// Date: 2013 8 22 17:08
// After rebasing the project, rewriting parts in C# that were in VB,
// and other improvements, all features (seem to) work as intended.

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
        // Genetic Operator Flag type definitions:
        public enum RealCrossoverOp { Uniform, MeanWithNoise }
        public enum SurvivalSelectionOp { ReplaceWorst, Elitist, Generational, Struggle }
        public enum ParentSelectionOp { Tournament, FitnessProportional }

        public class GA
        {
            public Func<int[], double[], int[], double> FitnessFunction { get; set; }
            public ScheduleGenome[] population;
            private ScheduleGenome[] offspring;
            public ScheduleGenome elite;
            //static private Random _rand;
            // Generic GA parameters:
            private int _seed;
            private int _popsize;
            private int _offsize;
            private double _deathRate;
            // Genetic Operator Flags:
            public ParentSelectionOp parentSelection { get; set; }
            public RealCrossoverOp realCrossover { get; set; }
            public SurvivalSelectionOp survivalSelection { get; set; }
            public GA(int seed, int popsize, int offsize, double deathRate)
            {
                _seed = seed;
                //_rand = new Random(_seed);
                SimpleRNG.SetSeed((uint)_seed);
                _popsize = popsize;
                _offsize = offsize;
                _deathRate = deathRate;
                population = new ScheduleGenome[_popsize];
                offspring = new ScheduleGenome[_offsize];
                // Default operator options:
                realCrossover = RealCrossoverOp.MeanWithNoise;
                survivalSelection = SurvivalSelectionOp.Elitist;
                parentSelection = ParentSelectionOp.Tournament;
            }
            public void IntializePopulations(ScheduleGenome genomeDefinition)
            {
                // ScheduleGenome._rand = _rand;
                elite = new ScheduleGenome(genomeDefinition);
                elite.realCrossover = realCrossover;
                elite.RandomInit();
                for (int i = 0; i < _popsize; i++)
                {
                    population[i] = new ScheduleGenome(elite);
                    population[i].RandomInit();
                    //population[i] = new ScheduleGenome(length, tl, modes, mutationRate, delayRate, delayMean);
                    //population[i].realCrossover = realCrossover;
                }
                for (int i = 0; i < _offsize; i++)
                {
                    offspring[i] = new ScheduleGenome(elite);
                    offspring[i].RandomInit();
                    //offspring[i] = new ScheduleGenome(length, tl, modes, mutationRate, delayRate, delayMean);
                    //offspring[i].realCrossover = realCrossover;
                }
            }
            // todo: broken, fix.
            public void SeedPopulation(int[] genes, double[] times, int[] modes, int mModes, double mutationRate)
            {
                //population[0] = new ScheduleGenome(genes.Length, times.Length, mModes, mutationRate, genes, times, modes);
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

            // Copies best individual in population to elite.
            // Assumes population has been evaluated. Use with caution.
            public void FindElite()
            {
                elite.fitness = FitnessFunction(elite.JobGenes, elite.TimeGenes, elite.ModeGenes);
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
                elite.fitness = FitnessFunction(elite.JobGenes, elite.TimeGenes, elite.ModeGenes);
                for (int i = 0; i < _popsize; i++)
                {
                    population[i].fitness = FitnessFunction(population[i].JobGenes, population[i].TimeGenes, population[i].ModeGenes);
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
                    _reftoGO.offspring[_i].fitness = _reftoGO.FitnessFunction(_reftoGO.offspring[_i].JobGenes, _reftoGO.offspring[_i].TimeGenes, _reftoGO.offspring[_i].ModeGenes);
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
                        //double r = _rand.NextDouble() * totalFitness;
                        double r = SimpleRNG.GetUniform() * totalFitness;
                        double runningTotal = population[p].fitness;
                        while (runningTotal > r)
                        {
                            p++;
                            runningTotal += population[p].fitness;
                        }
                        break;
                    case ParentSelectionOp.Tournament:
                        int k = _popsize / 10;
                        //p = _rand.Next(_popsize);
                        p = (int)(SimpleRNG.GetUniform() * _popsize);
                        double bestfitness = population[p].fitness;
                        for (int i = 0; i < k; i++)
                        {
                            int px = SimpleRNG.Next(0, _popsize);
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



            public sealed class NewComp : IComparer<ScheduleGenome>
            {
                public int Compare(ScheduleGenome x, ScheduleGenome y)
                {
                    return (y.fitness.CompareTo(x.fitness));
                }
            }

        }

        public class ScheduleGenome
        {

            public int[] JobGenes;
            public double[] TimeGenes;
            public int[] ModeGenes;

            public int _jobsLength;
            public int _timesLength;
            public int _modesLength;
            public int _numberOfModes;

            private double _mutationRate;
            public double _delayRate;
            public double _delayMean;
            public RealCrossoverOp realCrossover;

            public double fitness;

            // This constructor only allocates the gene arrays. Does not represent a valid solution.
            public ScheduleGenome(int jobsLength, int timesLength, int modesLength, int numberOfModes, double mut, double delayRate, double delayMean)
            {
                _mutationRate = mut;
                _delayRate = delayRate;
                _delayMean = delayMean;

                _jobsLength = jobsLength;
                _timesLength = timesLength;
                _modesLength = modesLength;
                _numberOfModes = numberOfModes;

                JobGenes = new int[jobsLength];
                TimeGenes = new double[timesLength];
                ModeGenes = new int[timesLength];

                for (int i = 0; i < _jobsLength; i++)
                {
                    JobGenes[i] = 0;
                }
                for (int i = 0; i < _timesLength; i++)
                {
                    TimeGenes[i] = 0;
                }
                for (int i = 0; i < _modesLength; i++)
                {
                    ModeGenes[i] = 0;
                }
                fitness = -1;

            }

            public ScheduleGenome(ScheduleGenome g)
            {
                JobGenes = new int[g._jobsLength];
                TimeGenes = new double[g._timesLength];
                ModeGenes = new int[g._modesLength];

                _delayRate = g._delayRate;
                _delayMean = g._delayMean;

                _jobsLength = g._jobsLength;
                _timesLength = g._timesLength;
                _modesLength = g._modesLength;
                _numberOfModes = g._numberOfModes;
                _mutationRate = g._mutationRate;

                realCrossover = g.realCrossover;

                for (int i = 0; i < g._jobsLength; i++)
                {
                    JobGenes[i] = g.JobGenes[i];
                }
                for (int i = 0; i < g._timesLength; i++)
                {
                    TimeGenes[i] = g.TimeGenes[i];
                }
                for (int i = 0; i < g._modesLength; i++)
                {
                    ModeGenes[i] = g.ModeGenes[i];
                }
                fitness = -1;
            }

            public ScheduleGenome(int jobsLength, int timesLength, int modesLength, int numberOfModes, double mut, int[] geneSeeds, double[] timeSeeds, int[] modeSeeds)
            {
                JobGenes = new int[jobsLength];
                TimeGenes = new double[timesLength];
                ModeGenes = new int[timesLength];


                _jobsLength = jobsLength;
                _timesLength = timesLength;
                _modesLength = modesLength;
                _numberOfModes = numberOfModes;
                _mutationRate = mut;
                for (int i = 0; i < jobsLength; i++)
                {
                    JobGenes[i] = geneSeeds[i];
                }
                for (int i = 0; i < timesLength; i++)
                {
                    TimeGenes[i] = timeSeeds[i];
                }
                for (int i = 0; i < modesLength; i++)
                {
                    ModeGenes[i] = modeSeeds[i];
                }
                fitness = -1;
            }

            public void RandomInit()
            {
                List<int> randarray = new List<int>();
                for (int i = 0; i < _jobsLength; i++)
                {
                    randarray.Add(i);
                }
                for (int i = 0; i < _jobsLength; i++)
                {
                    int r = SimpleRNG.Next(0, _jobsLength - i);
                    JobGenes[i] = randarray[r];
                    randarray.RemoveAt(r);
                }
                for (int i = 0; i < _timesLength; i++)
                {
                    if (SimpleRNG.GetUniform() < _delayRate)
                    {
                        TimeGenes[i] = SimpleRNG.GetExponential(_delayMean);
                    }
                    else
                    {
                        TimeGenes[i] = 0.0;
                    }
                    ModeGenes[i] = SimpleRNG.Next(0, _numberOfModes);
                }

                fitness = -1;

            }

            public void Copy(ScheduleGenome c)
            {
                fitness = c.fitness;
                for (int i = 0; i < JobGenes.Length; i++)
                {
                    JobGenes[i] = c.JobGenes[i];
                    if (i < _modesLength)
                    {
                        TimeGenes[i] = c.TimeGenes[i];
                        ModeGenes[i] = c.ModeGenes[i];
                    }
                }
            }

            public double Distance(ScheduleGenome c)
            {
                double d = 0;
                for (int i = 0; i < _modesLength; i++)
                {
                    d += Math.Pow(TimeGenes[i] - c.TimeGenes[i], 2.0);
                }
                return Math.Sqrt(d);
            }

            public void RealCrossover(ref ScheduleGenome p2, ref ScheduleGenome o1)
            {
                switch (realCrossover)
                {
                    case RealCrossoverOp.MeanWithNoise:
                        // Mean-with-noise Crossover:
                        for (int i = 0; i < _modesLength; i++)
                        {
                            double mean = TimeGenes[i] + p2.TimeGenes[i];
                            mean = mean / 2.0;
                            o1.TimeGenes[i] = SimpleRNG.GetNormal(mean, 0.5);
                            if (o1.TimeGenes[i] < 0.0)
                            {
                                o1.TimeGenes[i] = 0.0;
                            }
                        }
                        break;
                    case RealCrossoverOp.Uniform:
                        // Uniform Crossover:
                        int cutpoint = SimpleRNG.Next(0, _modesLength + 1);
                        for (int i = 0; i < cutpoint; i++)
                        {
                            o1.TimeGenes[i] = TimeGenes[i];
                        }
                        for (int i = cutpoint; i < _modesLength; i++)
                        {
                            o1.TimeGenes[i] = p2.TimeGenes[i];
                        }
                        break;
                }
            }

            public void Crossover(ref ScheduleGenome p2, ref ScheduleGenome o1)
            {
                int cutpoint = SimpleRNG.Next(0, _jobsLength);

                //Debug.Write(Environment.NewLine + "Distance between " + p1 + " - " + p2 + ": " + population[p1].Distance(population[p2]));

                for (int i = 0; i < _jobsLength; i++)
                {
                    o1.JobGenes[i] = JobGenes[i];
                }

                Debug.Assert(o1.IsValid(), "Invalid creature before crossover");

                List<int> remainder = new List<int>(p2.JobGenes);
                for (int i = 0; i < cutpoint; i++)
                {
                    remainder.Remove(JobGenes[i]);
                }
                for (int i = cutpoint; i < _jobsLength; i++)
                {
                    o1.JobGenes[i] = remainder[i - cutpoint];
                }

                Debug.Assert(o1.IsValid(), "Invalid creature after crossover");

                RealCrossover(ref p2, ref o1);
                CombinationCrossover(ref p2, ref o1);
            }
            public void CombinationCrossover(ref ScheduleGenome p2, ref ScheduleGenome o1)
            {
                int cutpoint = SimpleRNG.Next(0, _modesLength);

                for (int i = 0; i < cutpoint; i++)
                {
                    o1.ModeGenes[i] = p2.ModeGenes[i];
                }
                for (int i = cutpoint; i < _modesLength; i++)
                {
                    o1.ModeGenes[i] = ModeGenes[i];
                }
            }
            public void Mutate()
            {
                for (int i = 0; i < _jobsLength; i++)
                {
                    if (SimpleRNG.GetUniform() < _mutationRate)
                    {
                        int r = SimpleRNG.Next(0, _jobsLength);
                        int temp = JobGenes[i];
                        JobGenes[i] = JobGenes[r];
                        JobGenes[r] = temp;
                        // Mutate the delay time
                        r = SimpleRNG.Next(0, _modesLength);
                        double mutatedDelay = 0;
                        //mutatedDelay = SimpleRNG.GetExponential(_delayMean);
                        mutatedDelay = SimpleRNG.GetNormal(TimeGenes[r], 1.0);
                        //mutatedDelay = _rand.NextDouble() * _delayMean;
                        if (mutatedDelay < 0.0)
                        {
                            mutatedDelay = 0.0;
                        }
                        TimeGenes[r] = mutatedDelay;
                    }
                }
                //Mutate the Mode vector:
                for (int i = 0; i < _modesLength; i++)
                {
                    if (SimpleRNG.GetUniform() < _mutationRate)
                    {
                        ModeGenes[i] = SimpleRNG.Next(0, _numberOfModes);
                    }
                }

            }
            public bool IsValid()
            {
                List<int> vals = new List<int>();
                bool r = true;
                foreach (int i in JobGenes)
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
    }
}