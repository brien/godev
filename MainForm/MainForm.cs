﻿using System;
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
            // this.GAS = new Junction.GeneticAlgorithmSchedulingCS();
            // Turn on the wait cursor
            Cursor = Cursors.WaitCursor;
            // Dim EA As New Junction.ExcelAutomation

            // Dim dr As DataRow

            // Initialize the properties
            this.GAS.ShowStatusWhileRunning = cbShowStatus.Checked;
            //.LateCost = CDbl(tbLateCost.Text) / 60;
            this.GAS.WashTime = Convert.ToDouble(tbWashTime.Text) / 60;
            Junction.GeneticAlgorithmSchedulingCS.BOMPenaltyCost = Convert.ToDouble(tbComponentShortagePenalty.Text);
            Junction.GeneticAlgorithmSchedulingCS.ResourceNotFeasible = Convert.ToDouble(tbLineInfeasibility.Text);
            this.GAS.ResourcePref = Convert.ToDouble(tbLineAffinity.Text);

            this.GAS.meanDelayTime = Convert.ToDouble(tbMeanDelay.Text);
            this.GAS.delayRate = Convert.ToDouble(tbDelayProb.Text);
            if (rbStruggle.Checked)
            {
                this.GAS.survivalMode = Junction.GeneticOptimizer.SurvivalSelectionOp.Struggle;
            }
            else if (rbElitist.Checked)
            {
                this.GAS.survivalMode = Junction.GeneticOptimizer.SurvivalSelectionOp.Elitist;
            }
            else if (rbGenerational.Checked)
            {
                this.GAS.survivalMode = Junction.GeneticOptimizer.SurvivalSelectionOp.Generational;
            }
            else if (rbReplaceWorst.Checked)
            {
                this.GAS.survivalMode = Junction.GeneticOptimizer.SurvivalSelectionOp.ReplaceWorst;
            }
            if (rbFitnessProportional.Checked)
            {
                this.GAS.parentMode = Junction.GeneticOptimizer.ParentSelectionOp.FitnessProportional;
            }
            else if (rbTournament.Checked)
            {
                this.GAS.parentMode = Junction.GeneticOptimizer.ParentSelectionOp.Tournament;
            }
            if (rbUniform.Checked)
            {
                this.GAS.realCrossoverMode = Junction.GeneticOptimizer.RealCrossoverOp.Uniform;
            }
            else if (rbMeanWithNoise.Checked)
            {
                this.GAS.realCrossoverMode = Junction.GeneticOptimizer.RealCrossoverOp.MeanWithNoise;
            }
            // The next section of code loads worksheets into multiple data tables within a single data set
            // The dataset is then passed to the scheduler via the GAS.Masterdata Property.
            // Create a master data set with line, product, changeover, and order data in it.
            DataSet ds;
            DataSet ds2 = new DataSet();

            // Add the Production Line Master Data
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "Resources");
            DataTable dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);


            // Add the Product Spreadsheet.
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "Products");
            dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);

            // Add the orders data set
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "Orders");
            dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);

            // Add the Changeover time. Time of changing (From, To) products
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "Change Over");
            dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);

            // Add the Changeover Penalty. Penalty (not time) of changing (From, To) products
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "Change Over Penalties");
            dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);

            // Add the BOM Items
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "BOMItems");
            dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);

            // Add the pre-existing inventory
            ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbWorkBookName.Text, "Inventory");
            dt = ds.Tables[0];
            ds.Tables.Remove(dt);
            ds2.Tables.Add(dt);

            if (this.GAS.seededRun)
            {
                // Add the pre-existing schedule
                ds = Junction.ExcelAutomation.GetDataSetFromExcel(tbStartingScheduleName.Text, "Raw Genome");
                dt = ds.Tables[0];
                ds.Tables.Remove(dt);
                ds2.Tables.Add(dt);
            }


            // Send the complete dataset to the scheduler
            this.GAS.MasterData = ds2;

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
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            // Display the results
            gbResults.Visible = true;

            try
            {
                lblResult.Text = String.Format("{0: #,###.00}", GAS.Schedule(Convert.ToDouble(tbMutationProbability.Text), Convert.ToInt32(tbGenerations.Text)
                                          , Convert.ToDouble(tbDeathRate.Text), Convert.ToInt32(tbHerdSize.Text)));
            }
            catch (Exception ex)
            {
                String MessageText = " Problem during execution. Run is terminating.";
                MessageBox.Show(ex.Message, MessageText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Cursor = Cursors.Default;
            }


            // Display the timer
            lblSolveTime.Text = "";
            stopwatch.Stop();
            //ElapsedTime = DateAndTime.Timer - ElapsedTime;
            lblSolveTime.Text = String.Format("{0: #,###.00}", stopwatch.Elapsed.Seconds + " Seconds");
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
            tbStartingScheduleName.Text = "";
            this.GAS.seededRun = false;
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
    }
}