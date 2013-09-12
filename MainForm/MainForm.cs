using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Junction;

namespace MainForm
{
    public partial class MainForm : Form
    {
        public Junction.GeneticAlgorithmSchedulingCS GAS;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.GAS = new Junction.GeneticAlgorithmSchedulingCS();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            // Turn on the wait cursor
            Cursor = Cursors.WaitCursor;

            // The next section of code loads worksheets into multiple data tables within a single data set
            // The dataset is then passed to the scheduler via the GAS.Masterdata Property.
            // Create a master data set with line, product, changeover, and order data in it.
            DataSet ds2 = new DataSet();

            // Add the Production Line Master Data
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "Resources");
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "Products");
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "Orders");
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "Change Over");
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "Change Over Penalties");
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "BOMItems");
            AddExcelTableToMasterData(ref ds2, tbWorkBookName.Text, "Inventory");

            if (this.GAS.seededRun)
            {
                // Add the pre-existing schedule 
                AddExcelTableToMasterData(ref ds2, tbStartingScheduleName.Text, "Raw Genome");
            }

            // Send the complete dataset to the scheduler
            this.GAS.MasterData = ds2;


            // Initialize the properties
            this.GAS.ShowStatusWhileRunning = cbShowStatus.Checked;
            //.LateCost = CDbl(tbLateCost.Text) / 60;
            this.GAS.WashTime = Convert.ToDouble(tbWashTime.Text) / 60;
            Junction.GeneticAlgorithmSchedulingCS.BOMPenaltyCost = Convert.ToDouble(tbComponentShortagePenalty.Text);
            Junction.GeneticAlgorithmSchedulingCS.ResourceNotFeasible = Convert.ToDouble(tbLineInfeasibility.Text);
            this.GAS.ResourcePref = Convert.ToDouble(tbLineAffinity.Text);

            if (tbRandomSeed.Text != "")
            {
                this.GAS.randomSeed = Convert.ToInt32(tbRandomSeed.Text);
            }
            else
            {
                this.GAS.randomSeed = Environment.TickCount;
            }

            int populationSize = Convert.ToInt32(tbHerdSize.Text);
            int numberOfGenerations = Convert.ToInt32(tbGenerations.Text);
            double mutationRate = Convert.ToDouble(tbMutationProbability.Text);
            double deathRate = Convert.ToDouble(tbDeathRate.Text);

            this.GAS.meanDelayTime = Convert.ToDouble(tbMeanDelay.Text);
            this.GAS.delayRate = Convert.ToDouble(tbDelayProb.Text);
            this.GAS.InitializeGA(populationSize, numberOfGenerations, mutationRate, deathRate);
            if (rbStruggle.Checked)
            {
                this.GAS.CGA.survivalSelection = Junction.GeneticOptimizer.SurvivalSelectionOp.Struggle;
            }
            else if (rbElitist.Checked)
            {
                this.GAS.CGA.survivalSelection= Junction.GeneticOptimizer.SurvivalSelectionOp.Elitist;
            }
            else if (rbGenerational.Checked)
            {
                this.GAS.CGA.survivalSelection = Junction.GeneticOptimizer.SurvivalSelectionOp.Generational;
            }
            else if (rbReplaceWorst.Checked)
            {
                this.GAS.CGA.survivalSelection = Junction.GeneticOptimizer.SurvivalSelectionOp.ReplaceWorst;
            }
            if (rbFitnessProportional.Checked)
            {
                this.GAS.CGA.parentSelection = Junction.GeneticOptimizer.ParentSelectionOp.FitnessProportional;
            }
            else if (rbTournament.Checked)
            {
                this.GAS.CGA.parentSelection = Junction.GeneticOptimizer.ParentSelectionOp.Tournament;
            }
            if (rbUniform.Checked)
            {
                this.GAS.CGA.realCrossover = Junction.GeneticOptimizer.RealCrossoverOp.Uniform;
            }
            else if (rbMeanWithNoise.Checked)
            {
                this.GAS.CGA.realCrossover = Junction.GeneticOptimizer.RealCrossoverOp.MeanWithNoise;
            }
            /*
            Catch exp As Exception
                ' Will catch any error that we're not explicitly trapping.
                Dim MessageText As String = " Problem with input spreadsheet. Run is terminating."
                MessageBox.Show(exp.Message & MessageText, MessageText, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Cursor = Cursors.Default
                Exit Sub
            End Try
            */


            // *************************************************************
            // Start the run
            // *************************************************************
            // Set up a timer
            DateTime begin = DateTime.UtcNow;
            // Display the results
            gbResults.Visible = true;

            try
            {
                lblResult.Text = String.Format("{0: #,###.00}", GAS.Schedule(mutationRate,numberOfGenerations, deathRate, populationSize));
            }
            catch (Exception ex)
            {
                String MessageText = " Problem during execution. Run is terminating.";
                MessageBox.Show(ex.Message, MessageText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Cursor = Cursors.Default;
            }


            // Display the timer
            lblSolveTime.Text = "";
            DateTime end = DateTime.UtcNow;
            //ElapsedTime = DateAndTime.Timer - ElapsedTime;
            lblSolveTime.Text = String.Format("{0: #,###.00}", (end - begin).Seconds + " Seconds");
            lblChangeOverTime.Text = GAS.ChangeOverTime.ToString("#,###.00") + " Hours";
            lblLateJobsLine.Text = GAS.NumberOfResourceLateJobs.ToString();
            lblLateJobsService.Text = GAS.NumberOfServiceLateJobs.ToString();
            lblResourceViolations.Text = GAS.NumberOfResourceFeasibilityViolations.ToString();
            lblEarlyStartViolations.Text = GAS.NumberOfEarlyStartViolations.ToString();
            lblBOMViolations.Text = GAS.NumberOfBOMViolations.ToString();

            lblTotalTime.Text = GAS.TotalTime.ToString("#,###.00") + " Hours";
            lblRunTime.Text = GAS.RunTime.ToString("#,###.00") + " Hours";

            // reset the cursor
            Cursor = Cursors.Default;

            // set the output to excel and gantt display buttons to visible
            //btnOutputToExcel.Visible = true;
            //btnDisplayGantt.Visible = true;

            // bind the gridview control to the schedule data set
            dgvSchedule.DataSource = this.GAS.ScheduleDataSet.Tables[0].DefaultView;
            dgvSchedule.AutoResizeColumn(2, DataGridViewAutoSizeColumnMode.AllCells);
            dgvSchedule.AutoResizeColumn(3, DataGridViewAutoSizeColumnMode.AllCells);
            dgvSchedule.AutoResizeColumn(4, DataGridViewAutoSizeColumnMode.AllCells);
            dgvSchedule.AutoSize = true;
        }
        private void AddExcelTableToMasterData(ref DataSet ds2, string bookName, string sheetName)
        {
            DataSet ds = new DataSet();
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(bookName, sheetName);
            DataTable dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);
        }
        private void btnSelectSpreadSheet_Click(object sender, EventArgs e)
        {
            String DefaultDir = Application.StartupPath;
            openFileDialog1.InitialDirectory = DefaultDir;
            openFileDialog1.FileName = "TestImport.xlsx";
            openFileDialog1.Filter = "Excel 2007 Workbooks (*.xlsx)|*.xlsx|Excel 98-2005 Workbooks (*.xls)|*.xls";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.ShowReadOnly = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbWorkBookName.Text = openFileDialog1.FileName;
            }
            else
            {
                //MsgBox("Error Selecting File", MsgBoxStyle.Exclamation);
            }
        }

        private void dgvSchedule_Paint(object sender, PaintEventArgs e)
        {
            // Set the row colors
            foreach (DataGridViewRow dgvr in dgvSchedule.Rows)
            {
                if (Convert.ToBoolean(dgvr.Cells["Resource Late"].Value) || Convert.ToBoolean(dgvr.Cells["Service Late"].Value) || Convert.ToBoolean(dgvr.Cells["Early Violation"].Value) || Convert.ToBoolean(dgvr.Cells["Resource Feasibility"].Value) || Convert.ToBoolean(dgvr.Cells["BOM Violation"].Value))
                {
                    dgvr.DefaultCellStyle.BackColor = Color.LightPink;
                }
            }
        }

        private void btnOutputToExcel_Click(object sender, EventArgs e)
        {
            Junction.ExcelAutomation.CreateResultsWorksheet(GAS.ScheduleDataSet, GAS.GAResult);
        }

        private void btnSelectStartingSchedule_Click(object sender, EventArgs e)
        {
            string defaultDir = Application.StartupPath;
            openFileDialog1.InitialDirectory = defaultDir;
            openFileDialog1.FileName = "TestImport.xlsx";
            openFileDialog1.Filter = "Excel 2007 Workbooks (*.xlsx)|*.xlsx|Excel 98-2005 Workbooks (*.xls)|*.xls";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.ShowReadOnly = true;
            if( openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbStartingScheduleName.Text = openFileDialog1.FileName;
            }
            else
            {
                MessageBox.Show("Error Selecting File");
            }
            GAS.seededRun = true;
        }

        private void cbLoadSchedule_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLoadSchedule.Checked == true)
            {
                this.GAS.seededRun = true;
                tbStartingScheduleName.Enabled = true;
                btnSelectStartingSchedule.Enabled = true;
            }
            else
            {
                this.GAS.seededRun = false;
                tbStartingScheduleName.Enabled = false;
                btnSelectStartingSchedule.Enabled = false;
            }
        }
    }
}
