using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TestSimpleRNG;
using System.Diagnostics;



namespace Junction
{
    public sealed class GeneticAlgorithmSchedulingCS
    {
        // An arbitrary amount of time to add in decimal hours to create an unconstrained condition
        private const double UNCONSTRAINED_TIME = 999999;

        // Create a bit flag enumeration for allergens
        [Flags]
        public enum Allergens
        {
            None = 0,
            //Wheat = 1,
            //Milk = 2,
            //Soy = 4,
            Cat_16 = 1,
            Cat_2 = 2,
            Cat_8 = 4,
            Sesame = 8,
            Sulfite = 16,
            Eggs = 32,
            Fish = 64,
            Peanuts = 128,
            Nuts = 256
        }

        char[] AllergenList = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };

        // The mean of the delay times:
        public double meanDelayTime;
        // The rate at which delay times are generated (probablilty of non-zero delay time):
        public double delayRate;
        // The number used to seed the random number generator:
        public int randomSeed;
        // Just for debugging purposes:
        static bool shouldBreak = false;

        // Pre-existing inventory:
        private static int[] Inventory;
        private static double[] InventoryTime;

        //private double[] ProdRunTime;
        private static double[] JobRunTime;
        private String[] ProductName;
        // Used to hold the index from the product to the BOMItem list
        private static int[] BOMItemIndex; 
        private System.Collections.Hashtable ProductNumberHash;
        private string[] AllergensInProduct;
        private static double[,] ChangeOver;
        private static double[,] ChangeOverPenalties;
        private static int[] JobsToSchedule;
        private static double[] OrderQty;
        private object[,] ScheduleResult;
        // The due time in decimal hours
        private static double[] Priority;
        // Earliest start time in decimal hours
        private static double[] EarlyStart; 
        private double[] FitnessArray;
        private DateTime[] productionEndTime;
        private String[] ResourceName;
        private static double[] ProdEndTime;
        private DateTime[] productionStartTime;
        private static double[] ProdStartTime;
        private static int[] StartProduct;
        // Minimum Resource Late Cost
        private static double[] RLCMin; 
        // Maximum Resource Late Cost
        private static double[] RLCMax; 
        // Increase in late cost for each hour late
        private static double[] RLCRatePerHour; 
        private static double[] MinLateCost;
        private static double[] MaxLateCost;
        private static double[] LateCostPerHour;
        private static double[] MinEarlyCost;
        private static double[] MaxEarlyCost;
        private static double[] EarlyCostPerHour;
        private double[] MaxVolume;
        private double[] MinVolume;
        private double[] MaxFlowIn;
        private double[] MaxFlowOut;
        private int[] MaxInputs;
        private int[] MaxOutputs;
        public enum ResourceTypes
        {
            Batch = 0,
            Flow = 1,
            Tank = 2,
        }
        private ResourceTypes[] ResourceType;

        private static List<BOMItem> BOMItems = new List<BOMItem>();

        public static bool[] ConstrainedStart { get; set; }

        public GeneticOptimizer.GA CGA;
        private int[] Genes;
        private double[] Times;
        private int[] Modes;
        public bool seededRun;

        private static int NumberOfRealJobs;
        private DataSet masterData;


        public bool ValidDataInput { get; set; }
        public bool ShowStatusWhileRunning { get; set; }
        public static int NumberOfResources { get; set; }
        public double TotalTime { get; private set; }
        public int NumberOfResourceLateJobs { get; private set; }
        public int NumberOfServiceLateJobs { get; private set; }
        public int NumberOfResourceFeasibilityViolations { get; private set; }
        public int NumberOfBOMViolations { get; private set; }
        public int NumberOfEarlyStartViolations { get; private set; }
        public double RunTime { get; private set; }
        public double ChangeOverTime { get; private set; }
        public DataSet ScheduleDataSet { get; private set; }
        public DataSet GAResult { get; private set; }
        public static bool IsFeasible { get; private set; }
        //public double LateCost { get; set; }
        public static double BOMPenaltyCost { get; set; }
        public static double ResourceNotFeasible { get; set; }
        public double ResourcePref { get; set; }
        public static double[,] ResourcePreference;
        public double WashTime { get; set; }
        //public double RinseTime { get; set; }

        public DataSet MasterData
        {
            get
            {
                return masterData;
            }
            set
            {
                masterData = value;
                SetResourceData(masterData.Tables["Resources"]);
                SetProdData(masterData.Tables["Products"]);
                SetConstrainedStartData(masterData.Tables["Resources"]);
                SetChangeOverData(masterData.Tables["Change Over"]);
                SetChangeOverPenaltyData(MasterData.Tables["Change Over Penalties"]);
                SetOrderData(masterData.Tables["Orders"]);
                SetBOMData(masterData.Tables["BOMItems"]);
                SetInventoryData(masterData.Tables["Inventory"]);
                SetExistingSchedule(masterData.Tables["Raw Genome"]);
            }
        }

        private void CalcTime(ref object[,] Schedule, int TimeColumn, int SetupColumn, int[] genes, double[] delayTimes, int[] modes)
        {
            //Calculates Time Only - For Fitness use the the CalcFitness method
            double Time = ProdStartTime[0];
            int PreviousProd = -1;
            int CurrentProd;

            //Clear the global time variables
            TotalTime = 0;
            RunTime = 0;
            ChangeOverTime = 0;

            //Calculate the time for the following jobs
            for (int Resource = 0; Resource < NumberOfResources; Resource++)
            {
                if (ConstrainedStart[Resource])
                {
                    PreviousProd = StartProduct[Resource];
                }
                else
                {
                    PreviousProd = -1;
                }
                Time = ProdStartTime[Resource];
                //Split the gene string into resources
                int FirstGeneInResource = NumberOfRealJobs * Resource;
                int LastGeneInResource = (NumberOfRealJobs * (Resource + 1)) - 1;

                int[] output = new int[NumberOfResources * NumberOfRealJobs];
                Transform(genes, modes, ref output);
                for (int i = FirstGeneInResource; i <= LastGeneInResource; i++)
                {
                    //int CurrentJob = (int)Schedule[0, i];
                    //CurrentProd = (int)Schedule[1, i];
                    int CurrentJob = output[i]; // Population[BestIndex, i];
                    CurrentProd = JobsToSchedule[CurrentJob];
                    double co = 0;
                    if (PreviousProd != -1 & CurrentProd != -1)
                    {
                        co = ChangeOver[PreviousProd, CurrentProd];
                    }
                    Schedule[SetupColumn, i] = co;
                    if (CurrentProd != -1)
                    {
                        Time += JobRunTime[CurrentJob] + co + delayTimes[CurrentJob];
                        Schedule[TimeColumn, i] = Time;
                        //Update the global variables for time and late jobs
                        RunTime += JobRunTime[CurrentJob] + delayTimes[CurrentJob];
                        ChangeOverTime += co;
                        //Set the previous = current for the next loop
                        PreviousProd = CurrentProd;
                    }
                    else
                    {
                        Schedule[TimeColumn, i] = Time;
                    }
                }
                TotalTime += Time - ProdStartTime[Resource];
                //Reset the variables for the next resource
                PreviousProd = -1;
            }
        }

        private void AddDataColumnToTable(DataTable dt, Type type, string colName)
        {
            // Create a new data column, set the type and name, and add it to the table
            DataColumn dc = new DataColumn();
            dc.DataType = type;
            dc.ColumnName = colName;
            dc.ReadOnly = true;
            dc.AutoIncrement = false;
            dt.Columns.Add(dc);
        }

        private void CreateScheduleDataTable(int[] genes, double[] delayTimes, int[] modes)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            AddDataColumnToTable(dt, Type.GetType("System.Int32"), "Sequence Number");
            AddDataColumnToTable(dt, Type.GetType("System.Int32"), "Job Number");
            AddDataColumnToTable(dt, Type.GetType("System.Int32"), "Product Index");
            AddDataColumnToTable(dt, Type.GetType("System.String"), "Product Number");
            AddDataColumnToTable(dt, Type.GetType("System.String"), "Product Name");
            AddDataColumnToTable(dt, Type.GetType("System.DateTime"), "Early Start");
            AddDataColumnToTable(dt, Type.GetType("System.Double"), "Setup Time");
            AddDataColumnToTable(dt, Type.GetType("System.DateTime"), "Start Time");
            AddDataColumnToTable(dt, Type.GetType("System.Double"), "Run Time");
            AddDataColumnToTable(dt, Type.GetType("System.DateTime"), "End Time");
            AddDataColumnToTable(dt, Type.GetType("System.DateTime"), "Time Due");
            AddDataColumnToTable(dt, Type.GetType("System.Double"), "Order Quantity");
            AddDataColumnToTable(dt, Type.GetType("System.Int32"), "Resource Number");
            AddDataColumnToTable(dt, Type.GetType("System.String"), "Resource Name");
            AddDataColumnToTable(dt, Type.GetType("System.String"), "Production Order");
            AddDataColumnToTable(dt, Type.GetType("System.String"), "Allergens");
            AddDataColumnToTable(dt, Type.GetType("System.String"), "Allergen Alert");
            AddDataColumnToTable(dt, Type.GetType("System.Boolean"), "Resource Late");
            AddDataColumnToTable(dt, Type.GetType("System.Boolean"), "Service Late");
            AddDataColumnToTable(dt, Type.GetType("System.Boolean"), "Early Violation");
            AddDataColumnToTable(dt, Type.GetType("System.Boolean"), "Resource Feasibility");
            AddDataColumnToTable(dt, Type.GetType("System.Boolean"), "BOM Violation");
            dt.Columns["BOM Violation"].ReadOnly = false;

            DateTime d = DateTime.Today;

            //**************************************************************
            int[] output = new int[NumberOfResources * NumberOfRealJobs];

            Transform(genes, modes, ref output);
            int jMax = ScheduleResult.GetUpperBound(1) + 1;

            for (int i = 0; i < jMax; i++)
            {
                int Job;
                int Product;
                //Job = Population[BestIndex, i];
                Job = output[i];
                Product = JobsToSchedule[Job];
                ScheduleResult[0, i] = Job;
                ScheduleResult[1, i] = Product;
                if (Product == -1)
                {
                    ScheduleResult[2, i] = "Slack";
                    ScheduleResult[4, i] = UNCONSTRAINED_TIME;
                }
                else
                {
                    ScheduleResult[2, i] = ProductName[Product];
                    ScheduleResult[4, i] = Priority[Job];
                }
                ScheduleResult[5, i] = (int)(Math.Truncate((double)i / (double)NumberOfRealJobs)) + 1;
            }
            //put the time and setuptime into the schedule
            CalcTime(ref ScheduleResult, 3, 6, genes, delayTimes, modes);

            //Update the number of late jobs
            NumberOfServiceLateJobs = 0;
            NumberOfResourceLateJobs = 0;
            NumberOfResourceFeasibilityViolations = 0;
            NumberOfEarlyStartViolations = 0;
            NumberOfBOMViolations = 0; // Added 3/24/13

            for (int i = 0; i < jMax; i++)
            {
                dr = dt.NewRow();
                int Product = Convert.ToInt32(ScheduleResult[1, i]);
                int ResourceNum = (int)ScheduleResult[5, i] - 1;
                //Todo Could add a filter to skip delay jobs here (delay jobs are input with product code 9999 currently)
                if (Product != -1 & Product != 9999) //Skip slack jobs
                {
                    int LastRow = dt.Rows.Count - 1;
                    dr["Sequence Number"] = LastRow + 2;
                    int CurrentJob = (int)ScheduleResult[0, i];
                    dr["Job Number"] = CurrentJob;
                    dr["Product Index"] = Product;
                    dr["Product Number"] = masterData.Tables["Products"].Rows[Product]["Product Number"];
                    dr["Product Name"] = ScheduleResult[2, i];

                    dr["End Time"] = Conversions.ConvertDate((double)ScheduleResult[3, i]);
                    dr["Production Order"] = masterData.Tables["Orders"].Rows[(int)ScheduleResult[0, i]]["Production Order"];
                    dr["Setup Time"] = (double)(ScheduleResult[6, i]) * 60.0;//Convert Decimal Hour Setup Time to Minutes
                    dr["Run Time"] = JobRunTime[CurrentJob] * 60; //Convert Decimal Hour Run Times to Minutes
                    dr["Order Quantity"] = OrderQty[CurrentJob];

                    DateTime st = Conversions.ConvertDate((double)ScheduleResult[3, i]);
                    TimeSpan rm = new TimeSpan(0, (int)(JobRunTime[CurrentJob] * 60), 0);
                    st -= rm;
                    dr["Start Time"] = st;

                    dr["Early Start"] = Conversions.ConvertDate(EarlyStart[CurrentJob]);
                    if (st < Conversions.ConvertDate(EarlyStart[CurrentJob]) & EarlyStart[CurrentJob] != 0)
                    {
                        NumberOfEarlyStartViolations++;
                        dr["Early Violation"] = true;
                    }
                    else
                    {
                        dr["Early Violation"] = false;
                    }


                    if ((double)ScheduleResult[3, i] > (double)ScheduleResult[4, i])
                    {
                        NumberOfServiceLateJobs++;
                        dr["Service Late"] = true;
                    }
                    else
                    {
                        dr["Service Late"] = false;
                    }
                    if ((double)ScheduleResult[3, i] > ProdEndTime[ResourceNum])
                    {
                        NumberOfResourceLateJobs++;
                        dr["Resource Late"] = true;
                    }
                    else
                    {
                        dr["Resource Late"] = false;
                    }

                    // calculate infeasible resources
                    int prod = (int)ScheduleResult[1, i]; //find the product
                    int rn = (int)ScheduleResult[5, i] - 1;  //find the resource (base zero)
                    double rp = ResourcePreference[prod, rn];
                    if (rp == ResourceNotFeasible)
                    {
                        NumberOfResourceFeasibilityViolations++;
                        dr["Resource Feasibility"] = true;
                    }
                    else
                    {
                        dr["Resource Feasibility"] = false;
                    }

                    // Initialize BOM Violation to false as a defualt
                    dr["BOM Violation"] = false;

                    dr["Allergens"] = AllergensInProduct[Product];
                    dr["Resource Number"] = ScheduleResult[5, i];
                    dr["Resource Name"] = ResourceName[(int)ScheduleResult[5, i] - 1];
                    // Set the allergen alerts
                    dr["Allergen Alert"] = Allergens.None;


                    if (LastRow == -1 & ConstrainedStart[ResourceNum])
                    {
                        dr["Allergen Alert"] = AllergenAlert(StartProduct[ResourceNum], Product);
                    }
                    if (i > 0 & LastRow > -1)
                    {
                        int pResc = (int)dt.Rows[LastRow]["Resource Number"] - 1;
                        if (ResourceNum == pResc)
                        {
                            dr["Allergen Alert"] = AllergenAlert((int)dt.Rows[LastRow]["Product Index"], Product);
                        }
                        else
                        {
                            if (ConstrainedStart[ResourceNum])
                            {
                                dr["Allergen Alert"] = AllergenAlert(StartProduct[ResourceNum], Product);
                            }
                            else
                            {
                                dr["Allergen Alert"] = Allergens.None;
                            }
                        }
                    }

                    // Convert the decimal due date to a date or a blank if unconstrained
                    double decDate = (double)ScheduleResult[4, i];
                    // Double decDate = Priority[Job];
                    if (decDate == UNCONSTRAINED_TIME)
                    {
                        dr["Time Due"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Time Due"] = Conversions.ConvertDate(decDate);
                    }

                    // Convert unconstrained early start time to a blank
                    DateTime es = (DateTime)dr["Early Start"];
                    if (es == DateTime.Today)
                    {
                        dr["Early Start"] = DBNull.Value;
                    }

                    //ToDo 3/26 it looks like I messed up the logic near here
                    dt.Rows.Add(dr);


                }
            }
            // 3/24/13 changes
            // Display BOM Violations - Need to go back through the data set to find all potential BOM violaionns
            NumberOfBOMViolations = 0;
            int[] myInventory = new int[Inventory.Length];

            for (int i = 0; i < myInventory.Length; i++)
            {
                myInventory[i] = Inventory[i];
            }
            //// List of production orders to support calculation of BOM Item requirements
            //List<ProdSchedule> pSched = new List<ProdSchedule>();

            //******************************************************************************************
            //Get ready to check for BOM violations
            if (BOMItems.Count > 0) //This is purely a speed enhancement to skip this section if there are no BOM items.
            {
                List<ProdSchedule> pSched = new List<ProdSchedule>();
                int rMax = dt.Rows.Count;// -1;
                if (shouldBreak)
                {
                    Debug.Write(Environment.NewLine + " ---DataTable pSched----");
                }
                for (int i = 0; i < rMax; i++)
                {
                    dr = dt.Rows[i];
                    double EndTime = Conversions.ConvertDateTimetoDecimalHours((DateTime)dr["End Time"]);
                    double StartTime = Conversions.ConvertDateTimetoDecimalHours((DateTime)dr["Start Time"]);
                    double OrderQuantity = (double)dr["Order Quantity"];
                    int ProductIndex = (int)dr["Product Index"];
                    int JobNum = (int)dr["Job Number"];
                    //Build the output schedule for BOM Items
                    //First create a new production schedule item
                    ProdSchedule p = new ProdSchedule(ProductIndex, StartTime, EndTime, OrderQuantity, JobNum);
                    //Second, add the new schedule item to the list
                    if ((string)dr["Product Number"] != "9999")
                    {
                        pSched.Add(p); //don't add slack jobs
                        if (shouldBreak)
                        {
                            Debug.Write(Environment.NewLine +
                                p.Product +
                                " " + p.StartTime +
                                " " + p.EndTime +
                                " " + p.OrderQty +
                                " " + p.AvailableQuantity);
                        }
                    }
                }


                List<ProdSchedule> ComponentSchedule = new List<ProdSchedule>();
                foreach (ProdSchedule ps in pSched)
                {
                    // Find out if the item has components
                    int bIdx = BOMItemIndex[ps.Product];
                    if (bIdx == -1) continue;
                    int cIdx = -1;
                    BOMItem bi = BOMItems[bIdx];
                    foreach (int component in bi.Components)
                    {
                        cIdx++;
                        //add the demand of the component quantity to the schedule
                        double ComponentDemand = -(ps.OrderQty * bi.Percent[cIdx]);

                        //Note: StartTime is used twice in the line below. This is not a mistake.
                        //Component demand is not a real scheduled job.
                        //This simplification makes sure the demand occurs at the start of the parent job.
                        //Adding .0001 keeps the posting sequence = (debit inventory first at any given time then credit)
                        ProdSchedule cd = new ProdSchedule(component, ps.StartTime, ps.StartTime + 0.0001, ComponentDemand, ps.JobNum);
                        ComponentSchedule.Add(cd);
                    }

                }
                pSched.AddRange(ComponentSchedule);
                pSched.Sort();

                int pProd = -99; // set up a variable to hold the previous product
                double pQty = 0; //set up a variable to hold the previous quantity
                if (shouldBreak)
                {
                    Debug.Write(Environment.NewLine + " ---DataTable BOM pSched----");
                }
                //Calculate the available quantities
                foreach (ProdSchedule ps in pSched)
                {
                    if (shouldBreak)
                    {
                        Debug.Write(Environment.NewLine +
                            ps.Product +
                            " " + ps.StartTime +
                            " " + ps.EndTime +
                            " " + ps.OrderQty +
                            " " + ps.AvailableQuantity);
                    }
                    // Calculate the penalty
                    if (pProd == ps.Product)
                    {
                        pQty += ps.OrderQty;
                        ps.AvailableQuantity = pQty;
                    }
                    else
                    {
                        pQty = ps.OrderQty;
                        pProd = ps.Product;
                        ps.AvailableQuantity = pQty;
                    }
                    if (Inventory[ps.Product] > 0)
                    {
                        ps.AvailableQuantity += myInventory[ps.Product];
                        myInventory[ps.Product]--;
                    }
                    if (ps.AvailableQuantity < 0.0)
                    {
                        //BOMPenalties += BOMPenaltyCost;
                        //ScheduleViolationBOM = true;
                        NumberOfBOMViolations += 1;

                        // Update the output BOM Violations
                        int rwMax = dt.Rows.Count; //- 1;
                        for (int i = 0; i < rwMax; i++)
                        {
                            dr = dt.Rows[i];
                            int JobNum = (int)dr["Job Number"];
                            if (JobNum == ps.JobNum & ps.Product != -1)
                            {
                                dr["BOM Violation"] = true;
                                break;
                            }
                        }
                    }
                }
            }
            ScheduleDataSet = new DataSet();
            //Add the new DataTable to the dataset
            ScheduleDataSet.Tables.Add(dt);
            SaveRawElite(genes, delayTimes, modes);
        }
        public void SaveRawElite(int[] genes, double[] delayTimes, int[] modes)
        {
            GAResult = new DataSet();

            //Add the raw elite genome.
            DataTable dt2 = new DataTable();

            DataColumn geneCol = new DataColumn();
            geneCol.DataType = Type.GetType("System.Int32");
            geneCol.ColumnName = "Genes";
            dt2.Columns.Add(geneCol);

            DataColumn timeCol = new DataColumn();
            timeCol.DataType = Type.GetType("System.Double");
            timeCol.ColumnName = "DelayTime";
            dt2.Columns.Add(timeCol);

            DataColumn modeCol = new DataColumn();
            modeCol.DataType = Type.GetType("System.Double");
            modeCol.ColumnName = "Mode";
            dt2.Columns.Add(modeCol);

            // This is no longer well-written. All vectors are the same length now.
            for (int i = 0; i < genes.Length; i++)
            {
                DataRow dr2 = dt2.NewRow();
                dr2["Genes"] = genes[i];
                if (i < delayTimes.Length)
                {
                    dr2["DelayTime"] = delayTimes[i];
                }
                if (i < delayTimes.Length)
                {
                    dr2["Mode"] = modes[i];
                }
                dt2.Rows.Add(dr2);
            }

            GAResult.Tables.Add(dt2);
        }

        public void InitializeGA(int populationSize, int numberOfGenerations, double mutationRate, double replacementRate)
        {
            GeneticOptimizer.ScheduleGenome exampleGenome = new GeneticOptimizer.ScheduleGenome(NumberOfRealJobs, NumberOfRealJobs, NumberOfRealJobs, NumberOfResources, mutationRate, delayRate, meanDelayTime);
            CGA = new GeneticOptimizer.GA(randomSeed, populationSize, populationSize, replacementRate / 100.0);
            CGA.IntializePopulations(exampleGenome);
        }

        // This is the main method invoked to begin the scheduling process
        public double Schedule(double MutationProbability, int NumberOfGenerations, double DeathRate, int PopulationSize)
        {
            // The new function only exists to support "demo" code.
            DateTime now = new DateTime();
            now = DateTime.Today;
            if (now > DateTime.Parse("10/01/2013"))
            {
                throw new ApplicationException("***** Time limit for this demo version is exceeded.******\n\r Please contact Junction Solutions to obtain an updated and licensed version.\n\r");
            }

            int NumJobs = JobsToSchedule.GetUpperBound(0) + 1;
            ScheduleResult = new object[7, NumJobs];
            FitnessArray = new double[PopulationSize];
            //bool Stopped = false; //Allow for interruption of a scheduling run
            IsFeasible = false;

            // display the status form
            StatusForm frmStatus = new StatusForm();
            if (ShowStatusWhileRunning)
            {
                frmStatus.Visible = true;
                frmStatus.lblGeneration.Text = "Initialized";
                frmStatus.lblFeasible.Text = "No Feasible Solution Found";
            }
            double eliteFitness = 0;
            // Take parameters from calling functions parameters
            int popsize = PopulationSize;
            double mutarate = MutationProbability;
            
            if (seededRun)
            {
                CGA.SeedPopulation(Genes, Times, Modes, NumberOfResources, mutarate);
            }
            //CGA.survivalSelection = survivalMode;
            //CGA.parentSelection = parentMode;
            //CGA.realCrossover = realCrossoverMode;
            CGA.FitnessFunction = CalcFitness;
            CGA.EvaluatePopulation();
            for (int i = 0; i < NumberOfGenerations; i++)
            {
                CGA.GenerateOffspring();
                CGA.SurvivalSelection();
                if (ShowStatusWhileRunning & (i % (NumberOfGenerations / 100) == 0))
                {
                    // Update the status form
                    CGA.FindElite();
                    frmStatus.lblGeneration.Text = "Generation " + i.ToString();
                    frmStatus.lblAvgFitness.Text = String.Format("Average Fitness ={0: #,###.00}", Math.Abs(CGA.AverageFitness()));
                    frmStatus.lblCurrentValue.Text = String.Format("Best Fitness ={0: #,###.00}", Math.Abs(CGA.elite.fitness));
                    if (IsFeasible)
                    {
                        frmStatus.lblFeasible.Text = "Feasible Solution Found";
                    }
                    if (frmStatus.cbStopped.Checked)
                    {
                        break;
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            CGA.EvaluatePopulation();
            CGA.FindElite();

            // Close the status form
            frmStatus.Close();
            frmStatus = null;
            /*
            Debug.Write(Environment.NewLine + CGA.elite.fitness);
            for (int i = 0; i < NumJobs; i++)
            {
                Debug.Write(Environment.NewLine + CGA.elite.Genes[i] + "  " + CGA.elite.Times[i]);
            }*/
            Debug.Write(Environment.NewLine + "Random Seed = " + randomSeed);
            // Create a data table with the best schedule
            CreateScheduleDataTable(CGA.elite.JobGenes, CGA.elite.TimeGenes, CGA.elite.ModeGenes);
            shouldBreak = true;
            eliteFitness = CGA.FitnessFunction(CGA.elite.JobGenes, CGA.elite.TimeGenes, CGA.elite.ModeGenes);
            shouldBreak = false;

            return -1 * eliteFitness;
        }

        private double CalcAllergenChangeOver(int FromProduct, int ToProduct)
        {
            if (FromProduct == ToProduct)
            {
                return 0;
            }

            Allergens fJob = Allergens.None;
            Allergens tJob = Allergens.None;

            //find the allergens in the "from" job
            string fStr = AllergensInProduct[FromProduct];
            foreach (char c in fStr)
            {
                for (int i = 0; i < AllergenList.GetUpperBound(0); i++)
                {
                    if (c == AllergenList[i])
                    {
                        fJob = fJob | (Allergens)System.Math.Pow(2.0, (double)i);
                    }
                }
            }
            //find the allergens in the "to" job
            string tStr = AllergensInProduct[ToProduct];
            foreach (char c in tStr)
            {
                for (int i = 0; i < AllergenList.GetUpperBound(0); i++)
                {
                    if (c == AllergenList[i])
                    {
                        tJob = tJob | (Allergens)System.Math.Pow(2.0, (double)i);
                    }
                }
            }

            // find the allergens that were in the "from job" that are not in the "to job". 
            Allergens temp = fJob ^ tJob;
            Allergens result = temp & fJob;

            if (result == Allergens.None)
            {
                return 0; // This is a hard coded penalty to return
            }
            else
            {
                return WashTime;
            }
        }

        private Allergens AllergenAlert(int FromProduct, int ToProduct)
        {
            //char[] AllergenList = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };

            Allergens fJob = Allergens.None;
            Allergens tJob = Allergens.None;

            //find up the allergens in the "from" job
            string fStr = AllergensInProduct[FromProduct];
            foreach (char c in fStr)
            {
                for (int i = 0; i < AllergenList.GetUpperBound(0); i++)
                {
                    if (c == AllergenList[i])
                    {
                        fJob = fJob | (Allergens)System.Math.Pow(2.0, (double)i);
                    }
                }
            }
            //find up the allergens in the "to" job
            string tStr = AllergensInProduct[ToProduct];
            foreach (char c in tStr)
            {
                for (int i = 0; i < AllergenList.GetUpperBound(0); i++)
                {
                    if (c == AllergenList[i])
                    {
                        tJob = tJob | (Allergens)System.Math.Pow(2.0, (double)i);
                    }
                }
            }

            // find the allergens that were in the "from job" that are not in the "to job". 
            Allergens temp = fJob ^ tJob;
            Allergens result = temp & fJob;

            return result;
        }

        private void SetProdData(DataTable dt)
        {
            int i = 0;
            int numberOfProducts = dt.Rows.Count;

            //redimension the arrays to hold the product data
            ProductName = new string[numberOfProducts];
            AllergensInProduct = new string[numberOfProducts];

            BOMItemIndex = new int[numberOfProducts];
            //initialize the BOMItemIndex so that all values are -1;
            //The correct values will be set in the SetBOMData method
            for (int j = 0; j < numberOfProducts; j++)
            {
                BOMItemIndex[j] = -1;
            }

            ProductNumberHash = new System.Collections.Hashtable();
            ResourcePreference = new double[numberOfProducts, NumberOfResources];

            foreach (DataRow dr in dt.Rows)
            {
                ProductName[i] = (string)dr["Product Name"]; //Read in the product name
                string ProdNum = dr["Product Number"].ToString();

                ProductNumberHash.Add(dr["Product Number"].ToString(), i);
                if (dr.IsNull(4))
                {
                    AllergensInProduct[i] = "";
                }
                else
                {
                    AllergensInProduct[i] = (string)dr["Allergens"];
                }
                //Adding resource affinity here. 
                for (int j = 5; j < 5 + NumberOfResources; j++)
                {
                    int pref = (int)(double)dr[j]; ;
                    if (pref == 0)
                    {
                        ResourcePreference[i, j - 5] = ResourceNotFeasible;
                    }
                    else
                    {
                        ResourcePreference[i, j - 5] = (ResourcePref / pref) - (ResourcePref / 10);
                    }
                }
                i++;
            }

        }
        private void SetExistingSchedule(DataTable dt)
        {
            //int numberOfItems = dt.Rows.Count;
            //Inventory = new int[numberOfItems];
            // InventoryTime = new double[numberOfItems];
            if (seededRun)
            {
                int NumJobs = JobsToSchedule.GetUpperBound(0) + 1;
                Genes = new int[NumberOfRealJobs];
                Times = new double[NumberOfRealJobs];
                Modes = new int[NumberOfRealJobs];
                int i = 0;
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Genes[i] = (int)(double)dr["Genes"];
                        if (i < NumberOfRealJobs)
                        {
                            Modes[i] = (int)(double)dr["Mode"];
                            Times[i] = (double)dr["DelayTime"];
                        }
                        i++;
                    }
                }
                catch (Exception)
                {
                    ValidDataInput = false;
                    throw new ApplicationException("Invalid Starting Schedule - May not match input data");
                }
            }
        }
        private void SetInventoryData(DataTable dt)
        {
            int numberOfItems = dt.Rows.Count;
            Inventory = new int[numberOfItems];
            InventoryTime = new double[numberOfItems];

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string productNumber = dr["Product"].ToString();
                //ProductNumberHash.Add(dr["Product Number"].ToString(), i);
                int ProdIndex = (int)ProductNumberHash[productNumber];
                Inventory[ProdIndex] = (int)(double)dr["Quantity"];
                //InventoryTimes[ProdIndex] = 
                //productionStartTime[i] = (DateTime)dt.Rows[i]["Start_Date_Time"];
                InventoryTime[ProdIndex] = Conversions.ConvertDateTimetoDecimalHours((DateTime)dr["Time Available"]);
                i++;
            }
            /*
            for (int i = 0; i < numberOfItems; i++)
            {
                Inventory[i] = 0;
            }
            Inventory[0] = 1;
             */

            //redimension the arrays to hold the product data
            //ProductName = new string[numberOfProducts];
            //AllergensInProduct = new string[numberOfProducts];
        }
        private void SetBOMData(DataTable dt)
        {
            BOMItems.Clear();
            int r = 1;
            int ParentIndex;
            int rMax = dt.Rows.Count;
            foreach (DataRow dr in dt.Rows)
            {
                string Parent = (string)dr["Parent"].ToString();
                // Error catch statement added 4/23/2009
                try
                {
                    ParentIndex = (int)ProductNumberHash[Parent];
                }
                catch (Exception)
                {
                    ValidDataInput = false;
                    throw new ApplicationException("Invalid BOM Item " + Parent + " at row " + r + ". \r\n");
                }

                int iMax = dt.Columns.Count;
                List<int> myComponents = new List<int>();
                List<double> myPercent = new List<double>();
                for (int i = 1; i < iMax; i += 2)
                {
                    string Component = (string)dr[i].ToString();
                    if (Component == "") break;
                    int ComponentIndex;
                    // Error catch statement added 4/23/2009
                    try
                    {
                        ComponentIndex = (int)ProductNumberHash[Component];
                    }
                    catch (Exception)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException("Invalid Component Item " + Component + " at row " + r + ". \r\n");
                    }

                    myComponents.Add(ComponentIndex);
                    myPercent.Add(Convert.ToDouble(dr[i + 1]));
                }
                BOMItem bi = new BOMItem(ParentIndex, myComponents, myPercent);
                BOMItems.Add(bi);

                // Set the index to indicate that the item has BOM's
                BOMItemIndex[ParentIndex] = r - 1;

                r++;
            }

        }
        private void SetResourceData(DataTable dt)
        {
            NumberOfResources = dt.Rows.Count;
            // Dimension the resource related arrays
            productionEndTime = new DateTime[NumberOfResources];
            ProdEndTime = new double[NumberOfResources];
            productionStartTime = new DateTime[NumberOfResources];
            ProdStartTime = new double[NumberOfResources];
            StartProduct = new int[NumberOfResources];
            ConstrainedStart = new bool[NumberOfResources];
            RLCMax = new double[NumberOfResources];
            RLCMin = new double[NumberOfResources];
            RLCRatePerHour = new double[NumberOfResources];
            ResourceName = new string[NumberOfResources];
            MaxVolume = new double[NumberOfResources];
            MinVolume = new double[NumberOfResources];
            MaxFlowIn = new double[NumberOfResources];
            MaxFlowOut = new double[NumberOfResources];
            MaxInputs = new int[NumberOfResources];
            MaxOutputs = new int[NumberOfResources];
            ResourceType = new ResourceTypes[NumberOfResources];



            // Fill in the base data
            for (int i = 0; i < NumberOfResources; i++)
            {
                productionStartTime[i] = (DateTime)dt.Rows[i]["Start_Date_Time"];
                ProdStartTime[i] = Conversions.ConvertDateTimetoDecimalHours(productionStartTime[i]);
                productionEndTime[i] = (DateTime)dt.Rows[i]["End_Date_Time"];
                ProdEndTime[i] = Conversions.ConvertDateTimetoDecimalHours(productionEndTime[i]);
                RLCMin[i] = (double)dt.Rows[i]["LLCMin"];
                RLCMax[i] = (double)dt.Rows[i]["LLCMax"];
                RLCRatePerHour[i] = (double)dt.Rows[i]["LLCperHour"];
                ResourceName[i] = (string)dt.Rows[i]["Resource Name"];
                string rt = (string)dt.Rows[i]["Resource Type"];
                ResourceType[i] = (ResourceTypes)Enum.Parse(typeof(ResourceTypes), rt);

                if (ResourceType[i] == ResourceTypes.Tank)
                {
                    MaxVolume[i] = (double)dt.Rows[i]["Max Volume"];
                    MinVolume[i] = (double)dt.Rows[i]["Min Useable Volume"];
                    MaxFlowIn[i] = (double)dt.Rows[i]["Max Flow In"];
                    MaxFlowOut[i] = (double)dt.Rows[i]["Max Flow Out"];
                    MaxInputs[i] = (int)(double)dt.Rows[i]["Max Simultaneous Inputs"];
                    MaxOutputs[i] = (int)(double)dt.Rows[i]["Max Simultaneous Outputs"];
                }
            }
        }
        private void SetConstrainedStartData(DataTable dt)
        {
            for (int i = 0; i < NumberOfResources; i++)
            {
                if (dt.Rows[i].IsNull("Start Product"))
                {
                    StartProduct[i] = -1;
                    ConstrainedStart[i] = false;
                }
                else
                {
                    string StartProductStr = dt.Rows[i]["Start Product"].ToString();

                    StartProduct[i] = (int)ProductNumberHash[StartProductStr];
                    ConstrainedStart[i] = true;
                }

            }
        }
        private void SetChangeOverData(DataTable dt)
        {
            int jMax = dt.Columns.Count - 1;
            int iMax = dt.Rows.Count;

            // Make sure the changeover matrix is valid
            int NumberOfProducts = ProductName.GetUpperBound(0) + 1;
            if (iMax != NumberOfProducts | jMax != NumberOfProducts)
            {
                //throw new ApplicationException("Wrong number of rows or columns in the Change Over matrix.\r\n The number of rows and columns must be equal to the number of products. There were " + iMax + " rows, " + jMax + " columns, and " + NumberOfProducts + " Products found. \r\n Make sure there is no stray input, including blanks, on the changeover matrix spreadsheet.\r\n");
            }

            //ChangeOver = new double[iMax, jMax];
            ChangeOver = new double[NumberOfProducts, NumberOfProducts];
            for (int i = 0; i < NumberOfProducts; i++)
            {
                for (int j = 0; j < NumberOfProducts; j++)
                {
                    //try
                    if (i >= iMax || j >= jMax)
                    {
                        ChangeOver[i, j] = 0;
                    }
                    else
                    {
                        ChangeOver[i, j] = (double)dt.Rows[i].ItemArray[j + 1] / 60.0;
                        ChangeOver[i, j] += CalcAllergenChangeOver(i, j);
                    }
                    /*
                    catch (Exception)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException("Invalid changeover time at row " + (i + 2) + " column " + (j + 2) + ". \r\n");
                    }*/
                }
            }
        }
        private void SetChangeOverPenaltyData(DataTable dt)
        {
            int jMax = dt.Columns.Count - 1;
            int iMax = dt.Rows.Count;

            // Make sure the changeover matrix is valid
            int NumberOfProducts = ProductName.GetUpperBound(0) + 1;
            if (iMax != ProductName.GetUpperBound(0) + 1 | jMax != ProductName.GetUpperBound(0) + 1)
            {
                //throw new ApplicationException("Wrong number of rows or columns in the Change Over Penalty matrix.\r\n The number of rows and columns must be equal to the number of products. There were " + iMax + " rows, " + jMax + " columns, and " + (ProductName.GetUpperBound(0) + 1) + " Products found. \r\n Make sure there is no stray input, including blanks, on the changeover penalty matrix spreadsheet.\r\n");
            }

            ChangeOverPenalties = new double[NumberOfProducts, NumberOfProducts];
            for (int i = 0; i < NumberOfProducts; i++)
            {
                for (int j = 0; j < NumberOfProducts; j++)
                {
                    if (i >= iMax || j >= jMax)
                    {
                        ChangeOver[i, j] = 0;
                    }
                    else
                    //try
                    {
                        ChangeOverPenalties[i, j] = (double)dt.Rows[i].ItemArray[j + 1];// / 60.0;
                        //this.ChangeOverPenalties[i, j] += CalcAllergenChangeOver(i, j);
                    }
                    /*catch (Exception)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException("Invalid changeover penalty at row " + (i + 2) + " column " + (j + 2) + ". \r\n");
                    }*/
                }
            }
        }
        private void SetOrderData(DataTable dt)
        {
            if (NumberOfResources < 1)
            {
                throw new ApplicationException("Order Data Set cannot be initialized. The Resources Data Table must be initialized first.\r\n");
            }
            // Note: "NumberOfRealJobs" is misleading.
            // It is the number of actual orders with real products + number of delay jobs, whether they are generated or in the input spreadsheet. 
            int SlackJobs, TotalJobs;
            int numberOfDelayJobs = 0;
            NumberOfRealJobs = 0;
            NumberOfRealJobs = masterData.Tables["Orders"].Rows.Count;
            SlackJobs = (NumberOfRealJobs * NumberOfResources) - NumberOfRealJobs;
            TotalJobs = NumberOfRealJobs + SlackJobs;

            JobsToSchedule = new int[TotalJobs];
            Priority = new double[TotalJobs];
            EarlyStart = new double[TotalJobs];
            JobRunTime = new double[TotalJobs];
            OrderQty = new double[TotalJobs];
            MaxLateCost = new double[TotalJobs];
            MinLateCost = new double[TotalJobs];
            LateCostPerHour = new double[TotalJobs];
            MaxEarlyCost = new double[TotalJobs];
            MinEarlyCost = new double[TotalJobs];
            EarlyCostPerHour = new double[TotalJobs];

            double avgJobRunTime = 0;

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string ProdNum = dr["Product Number"].ToString();
                    int ProdIndex = (int)ProductNumberHash[ProdNum];
                    OrderQty[i] = (double)dr["Quantity"];
                    JobsToSchedule[i] = ProdIndex;

                    JobRunTime[i] = (double)dr.ItemArray[3] / 60.0; //Read in the product run time and convert from minutes to decimal hours
                    avgJobRunTime += JobRunTime[i];

                    if (JobsToSchedule[i] > ProductName.GetUpperBound(0))
                    {
                        throw new ApplicationException();
                    }
                    // Load the order late cost penalties
                    MinLateCost[i] = (double)dr["Min Late Cost"];
                    MaxLateCost[i] = (double)dr["Max Late Cost"];
                    LateCostPerHour[i] = (double)dr["Late Cost Per Hour"];
                    // Load the order early cost penalties
                    MinEarlyCost[i] = (double)dr["Min Early Cost"];
                    MaxEarlyCost[i] = (double)dr["Max Early Cost"];
                    EarlyCostPerHour[i] = (double)dr["Early Cost Per Hour"];

                }
                catch (Exception)
                {
                    ValidDataInput = false;
                    throw new ApplicationException("Invalid Product Index. Error found at row " + (i + 2) + " in the Orders spreadsheet.\r\n");
                }
                //input the due time if not blank
                if (dr.IsNull(4))
                {
                    Priority[i] = UNCONSTRAINED_TIME;
                }
                else
                {
                    try
                    {
                        //Priority[i] = Conversions.ConvertMilitaryTimeStringToDecimalHours(dr.ItemArray[4].ToString());
                        Priority[i] = Conversions.ConvertDateTimetoDecimalHours((DateTime)dr.ItemArray[4]);

                    }
                    catch (ApplicationException ex)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException(ex.Message + "Error found at row " + i + 2 + " in the Orders spreadsheet.\r\n");
                    }
                    catch (Exception ex)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException(ex.Message + "Error found at row " + i + 2 + " in the Orders spreadsheet.\r\n");
                    }
                }

                //input the early start time if not blank
                if (dr.IsNull("Early Start"))
                {
                    EarlyStart[i] = 0;
                }
                else
                {
                    try
                    {
                        EarlyStart[i] = Conversions.ConvertDateTimetoDecimalHours((DateTime)dr["Early Start"]);
                    }
                    catch (ApplicationException ex)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException(ex.Message + "Error found at row " + i + 2 + " in the Orders spreadsheet.\r\n");
                    }
                    catch (Exception ex)
                    {
                        ValidDataInput = false;
                        throw new ApplicationException(ex.Message + "Error found at row " + i + 2 + " in the Orders spreadsheet.\r\n");
                    }
                }

                i++;
            }

            avgJobRunTime = avgJobRunTime / (NumberOfRealJobs - numberOfDelayJobs);
            //Set up the Slack Jobs
            if (TotalJobs > NumberOfRealJobs)
            {
                for (int j = NumberOfRealJobs; j < TotalJobs; j++)
                {
                    Priority[j] = UNCONSTRAINED_TIME;
                    JobsToSchedule[j] = -1; //Set this to a -1 to indicate that this is a slack job.
                }
            }
        }
        private static void Transform(int[] genes, int[] modes, ref int[] output)
        {
            for (int i = 0; i < NumberOfResources * NumberOfRealJobs; i++)
            {
                output[i] = NumberOfRealJobs + 1;
            }
            int[] used = new int[NumberOfResources];
            for (int i = 0; i < NumberOfRealJobs; i++)
            {
                output[modes[i] * NumberOfRealJobs + used[modes[i]]] = i;
                used[modes[i]]++;
            }

        }
        private static double CalcFitness(int[] genes, double[] delayTimes, int[] modes)
        {
            double Time = ProdStartTime[0];
            double JobStartTime, JobEndTime;
            double Fitness;
            double SumOfServiceEarlyPenalties = 0;
            double SumOfServiceLatePenalties = 0;
            double SumOfResourceLatePenalties = 0;
            double BOMPenalties = 0;
            int PreviousProd = -1;
            bool ScheduleViolationBOM = false;
            bool ScheduleViolationResourceLate = false;
            bool ScheduleViolationOrderLate = false;
            bool ScheduleViolationResourceFeasiblilty = false;
            bool ScheduleViolationEarlyTime = false;
            double LateTime, EarlyTime;
            int CurrentProd;
            double TotalTimeAllResources = 0;
            int CurrentJob;
            double ResourcePrefPenalties = 0;
            double SumOfChangeOverPenalties = 0;
            double EarlyStartFactor = 0;
            int[] myInventory = new int[Inventory.Length];

            for (int i = 0; i < myInventory.Length; i++)
            {
                myInventory[i] = Inventory[i];
            }

            // List of production orders to support calculation of BOM Item requirements
            List<ProdSchedule> pSched = new List<ProdSchedule>();
            int[] output = new int[NumberOfResources * NumberOfRealJobs];

            Transform(genes, modes, ref output);

            for (int Resource = 0; Resource < NumberOfResources; Resource++)
            {
                if (ConstrainedStart[Resource])
                {
                    PreviousProd = StartProduct[Resource];
                }
                else
                {
                    PreviousProd = -1;
                }
                Time = ProdStartTime[Resource];
                // Calculate the time for the following jobs
                int FirstGeneInResource = NumberOfRealJobs * Resource;
                int LastGeneInResource = (NumberOfRealJobs * (Resource + 1)) - 1;
                double co, cop;
                for (int i = FirstGeneInResource; i <= LastGeneInResource; i++)
                {
                    CurrentJob = output[i];
                    CurrentProd = JobsToSchedule[CurrentJob];
                    if (PreviousProd != -1 & CurrentProd != -1)
                    {
                        co = ChangeOver[PreviousProd, CurrentProd];
                        cop = ChangeOverPenalties[PreviousProd, CurrentProd];
                    }
                    else
                    {
                        co = 0;
                        cop = 0;
                    }
                    if (CurrentProd != -1)
                    {
                        Time += delayTimes[CurrentJob] + co;
                        JobStartTime = Time;
                        Time += JobRunTime[CurrentJob];
                        JobEndTime = Time;

                        //Used to encourage jobs to start as soon as possible
                        //Todo  Turn Early start facto into a configurable factor (by resorce?)
                        
                        if (JobStartTime > ProdEndTime[NumberOfResources - 1])
                        {
                            EarlyStartFactor += JobStartTime;
                        }
                        
                        PreviousProd = JobsToSchedule[CurrentJob];

                        //Calculate Service Early Cost
                        EarlyTime = EarlyStart[CurrentJob];
                        if (JobStartTime < EarlyTime & EarlyTime != 0.0)
                        {
                            SumOfServiceEarlyPenalties += Math.Min(MaxEarlyCost[CurrentJob], MinEarlyCost[CurrentJob] + EarlyCostPerHour[CurrentJob] * (EarlyTime - (JobStartTime)));
                            ScheduleViolationEarlyTime = true;
                        }

                        //Calculate Service Late Cost
                        LateTime = Priority[CurrentJob];
                        if (Time > LateTime)
                        {
                            SumOfServiceLatePenalties += Math.Min(MaxLateCost[CurrentJob], MinLateCost[CurrentJob] + LateCostPerHour[CurrentJob] * (JobEndTime - Priority[CurrentJob]));
                            ScheduleViolationOrderLate = true;
                        }

                        //Calculate Resource Late Cost
                        if (Time > ProdEndTime[Resource])
                        {
                            SumOfResourceLatePenalties += Math.Min(RLCMax[Resource], RLCMin[Resource] + RLCRatePerHour[Resource] * (Time - ProdEndTime[Resource]));
                            ScheduleViolationResourceLate = true;
                        }

                        // Add the resource preference penalties
                        ResourcePrefPenalties += ResourcePreference[CurrentProd, Resource];
                        if (ResourcePreference[CurrentProd, Resource] == ResourceNotFeasible)
                        {
                            ScheduleViolationResourceFeasiblilty = true;
                        }

                        //Sum up the changeover penalties
                        SumOfChangeOverPenalties += cop;

                        //Build the output schedule for BOM Items
                        //First create a new production schedule item
                        ProdSchedule p = new ProdSchedule(CurrentProd, JobStartTime, JobEndTime, OrderQty[CurrentJob]);
                        //Second, add the new schedule item to the list
                        pSched.Add(p);
                    }
                }
                // Calculate the total production time required
                TotalTimeAllResources += Time - ProdStartTime[Resource];
            }

            //Get ready to check for BOM violations
            if (BOMItems.Count > 0) //This is purely a speed enhancement to skip this section if there are no BOM items.
            {
                if (shouldBreak)
                {
                    Debug.Write(Environment.NewLine + " --- CalcFitness pSched --- ");
                }
                List<ProdSchedule> ComponentSchedule = new List<ProdSchedule>();
                foreach (ProdSchedule ps in pSched)
                {
                    if (shouldBreak)
                    {
                        Debug.Write(Environment.NewLine +
                            ps.Product +
                            " " + ps.StartTime +
                            " " + ps.EndTime +
                            " " + ps.OrderQty +
                            " " + ps.AvailableQuantity);
                    }
                    //Find out if the item has components
                    int bIdx = BOMItemIndex[ps.Product];
                    if (bIdx == -1) continue;
                    int cIdx = -1;
                    BOMItem bi = BOMItems[bIdx];
                    foreach (int component in bi.Components)
                    {
                        cIdx++;
                        //add the demand of the component quantity to the schedule
                        double ComponentDemand = -(ps.OrderQty * bi.Percent[cIdx]);
                        //Note: StartTime is used twice in the line below. This is not a mistake.
                        //Component demand is not a real scheduled job.
                        //This simplification makes sure the demand occurs at the start of the parent job.
                        //Adding .0001 keeps the posting sequence = (debit inventory first at any given time then credit)
                        ProdSchedule cd = new ProdSchedule(component, ps.StartTime, ps.StartTime + 0.0001, ComponentDemand);
                        ComponentSchedule.Add(cd);
                    }
                }
                pSched.AddRange(ComponentSchedule);
                pSched.Sort();

                int pProd = -99; // set up a variable to hold the previous product
                double pQty = 0; //set up a variable to hold the previous quantity
                bool pBomViolation = false;
                double pStartTime = 0.0;
                double timeDiff = 0.0; //variable to hold BOM violation time delta
                if (shouldBreak)
                {
                    Debug.Write(Environment.NewLine + " --- CalcFitness BOM pSched --- ");
                }
                //Calculate the available quantities
                foreach (ProdSchedule ps in pSched)
                {
                    if (shouldBreak)
                    {
                        Debug.Write(Environment.NewLine +
                            ps.Product +
                            " " + ps.StartTime +
                            " " + ps.EndTime +
                            " " + ps.OrderQty +
                            " " + ps.AvailableQuantity);
                    }

                    // Calculate the penalty
                    if (pProd == ps.Product)
                    {
                        timeDiff = ps.EndTime - pStartTime;
                        pQty += ps.OrderQty;
                        ps.AvailableQuantity = pQty;
                    }
                    else
                    {
                        //timeDiff = 0.0;
                        pStartTime = ps.StartTime;
                        pQty = ps.OrderQty;
                        pProd = ps.Product;
                        ps.AvailableQuantity = pQty;
                    }
                    if (shouldBreak) Debug.Write(" " + timeDiff);
                    if (pBomViolation == true)
                    {
                        if (shouldBreak) Debug.Write(" VP: " + timeDiff * 10);
                        BOMPenalties += timeDiff * 20;
                        pBomViolation = false;
                        timeDiff = 0.0;
                    }
                    if (Inventory[ps.Product] > 0 && ps.EndTime > InventoryTime[ps.Product])
                    {
                        ps.AvailableQuantity += myInventory[ps.Product];
                        myInventory[ps.Product]--;
                    }
                    if (ps.AvailableQuantity < 0.0)
                    {
                        if (shouldBreak) Debug.Write(" BOM VIOLATION");
                        pBomViolation = true;
                        BOMPenalties += BOMPenaltyCost;
                        ScheduleViolationBOM = true;
                    }
                }
            }

            if (!(ScheduleViolationBOM | ScheduleViolationResourceLate | ScheduleViolationOrderLate | ScheduleViolationResourceFeasiblilty | ScheduleViolationEarlyTime))
            {
                IsFeasible = true;
            }

            //Fitness = TotalTimeAllResources + SumOfResourceLatePenalties + SumOfServiceLatePenalties + BOMPenalties + ResourcePrefPenalties
            //    + SumOfChangeOverPenalties + SumOfServiceEarlyPenalties; 
            Fitness = TotalTimeAllResources + SumOfResourceLatePenalties + SumOfServiceLatePenalties + BOMPenalties + ResourcePrefPenalties + SumOfChangeOverPenalties + SumOfServiceEarlyPenalties + EarlyStartFactor;
            //Fitness = ResourcePrefPenalties;// +SumOfChangeOverPenalties + SumOfServiceEarlyPenalties;


            Debug.Assert(Fitness > 0.0, "Fitness < 0.0");
            return (-1.0 * Fitness);
        }
    }

    // This is the fastest reverse comparer for type specific sorting
    internal class ReverseComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            // Compare y and x in reverse order.
            return y.CompareTo(x);
        }
    }
    // Create a class for production sequencing to support BOM Items
    public class ProdSchedule : IComparable<ProdSchedule>
    {
        // Define a 4 argument constructor for use during the fitness function calculations
        public ProdSchedule(int product, double startTime, double endTime, double orderQty)
        {
            this.Product = product;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.OrderQty = orderQty;
            this.AvailableQuantity = 0;
        }
        // Define a 5 argument constructor for use during the results display functions
        public ProdSchedule(int product, double startTime, double endTime, double orderQty, int jobNum)
        {
            this.Product = product;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.OrderQty = orderQty;
            this.JobNum = jobNum;
            this.AvailableQuantity = 0;
        }

        public int Product { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double OrderQty { get; set; }
        public double AvailableQuantity { get; set; }
        public int JobNum { get; set; }

        //Implements IComparer to sort by product and time
        public int CompareTo(ProdSchedule other)
        {
            int compare = this.Product.CompareTo(other.Product);
            if (compare == 0)
            {
                //Todo: validate start time vs end time impact on this logic.
                // May need to add a use time to the logic that is start for consumption orders or end for production orders
                // May also be start for tank production orders
                // This could be a separate field or calculated in the compare logic of the ProductionSchedule class
                compare = this.EndTime.CompareTo(other.EndTime);
            }
            return compare;
        }
    }
    // Create a class to support BOM items
    public class BOMItem
    {
        public BOMItem(int product, List<int> components, List<double> percent)
        {
            this.Product = product;
            this.Components = components;
            this.Percent = percent;
        }
        public int Product { get; set; }
        public List<int> Components { get; set; }
        public List<double> Percent { get; set; }
    }


}
