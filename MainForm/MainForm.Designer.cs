namespace MainForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnOutputToExcel = new System.Windows.Forms.Button();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.lblBOMViolations = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label24 = new System.Windows.Forms.Label();
            this.lblResourceViolations = new System.Windows.Forms.Label();
            this.lblLateJobsService = new System.Windows.Forms.Label();
            this.LineLateJobs = new System.Windows.Forms.Label();
            this.Label25 = new System.Windows.Forms.Label();
            this.lblLateJobsLine = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.lblEarlyStartViolations = new System.Windows.Forms.Label();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.lblChangeOverTime = new System.Windows.Forms.Label();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.Label22 = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.lblSolveTime = new System.Windows.Forms.Label();
            this.Label21 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label20 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSolve = new System.Windows.Forms.Button();
            this.tbStartingScheduleName = new System.Windows.Forms.TextBox();
            this.Label29 = new System.Windows.Forms.Label();
            this.btnSelectStartingSchedule = new System.Windows.Forms.Button();
            this.btnSelectSpreadSheet = new System.Windows.Forms.Button();
            this.tbWorkBookName = new System.Windows.Forms.TextBox();
            this.Label18 = new System.Windows.Forms.Label();
            this.cbShowStatus = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.GroupBox10 = new System.Windows.Forms.GroupBox();
            this.rbMeanWithNoise = new System.Windows.Forms.RadioButton();
            this.rbUniform = new System.Windows.Forms.RadioButton();
            this.GroupBox9 = new System.Windows.Forms.GroupBox();
            this.rbTournament = new System.Windows.Forms.RadioButton();
            this.rbFitnessProportional = new System.Windows.Forms.RadioButton();
            this.gbSurvival = new System.Windows.Forms.GroupBox();
            this.rbReplaceWorst = new System.Windows.Forms.RadioButton();
            this.rbStruggle = new System.Windows.Forms.RadioButton();
            this.rbGenerational = new System.Windows.Forms.RadioButton();
            this.rbElitist = new System.Windows.Forms.RadioButton();
            this.Label26 = new System.Windows.Forms.Label();
            this.Label23 = new System.Windows.Forms.Label();
            this.tbMeanDelay = new System.Windows.Forms.TextBox();
            this.tbDelayProb = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.tbDeathRate = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbGenerations = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbHerdSize = new System.Windows.Forms.TextBox();
            this.tbMutationProbability = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.Label30 = new System.Windows.Forms.Label();
            this.tbComponentShortagePenalty = new System.Windows.Forms.TextBox();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.Label27 = new System.Windows.Forms.Label();
            this.tbLineInfeasibility = new System.Windows.Forms.TextBox();
            this.Label28 = new System.Windows.Forms.Label();
            this.tbLineAffinity = new System.Windows.Forms.TextBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.tbWashTime = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.Label15 = new System.Windows.Forms.Label();
            this.tbLateCost = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.tbOvertimeIncrementCost = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.tbStandardCost = new System.Windows.Forms.TextBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.dtpProductionEndDate = new System.Windows.Forms.DateTimePicker();
            this.Label17 = new System.Windows.Forms.Label();
            this.dtpProductionDate = new System.Windows.Forms.DateTimePicker();
            this.Label13 = new System.Windows.Forms.Label();
            this.dtpProductionStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpProductionEndTime = new System.Windows.Forms.DateTimePicker();
            this.Label14 = new System.Windows.Forms.Label();
            this.tbOvertimeStart = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvSchedule = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRandomSeed = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.GroupBox7.SuspendLayout();
            this.gbResults.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.GroupBox10.SuspendLayout();
            this.GroupBox9.SuspendLayout();
            this.gbSurvival.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.GroupBox6.SuspendLayout();
            this.GroupBox5.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(832, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnOutputToExcel);
            this.tabPage1.Controls.Add(this.GroupBox7);
            this.tabPage1.Controls.Add(this.gbResults);
            this.tabPage1.Controls.Add(this.GroupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(824, 523);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnOutputToExcel
            // 
            this.btnOutputToExcel.Location = new System.Drawing.Point(596, 207);
            this.btnOutputToExcel.Name = "btnOutputToExcel";
            this.btnOutputToExcel.Size = new System.Drawing.Size(94, 27);
            this.btnOutputToExcel.TabIndex = 40;
            this.btnOutputToExcel.Text = "Output to Excel";
            this.btnOutputToExcel.UseVisualStyleBackColor = true;
            this.btnOutputToExcel.Click += new System.EventHandler(this.btnOutputToExcel_Click);
            // 
            // GroupBox7
            // 
            this.GroupBox7.Controls.Add(this.lblBOMViolations);
            this.GroupBox7.Controls.Add(this.Label5);
            this.GroupBox7.Controls.Add(this.Label24);
            this.GroupBox7.Controls.Add(this.lblResourceViolations);
            this.GroupBox7.Controls.Add(this.lblLateJobsService);
            this.GroupBox7.Controls.Add(this.LineLateJobs);
            this.GroupBox7.Controls.Add(this.Label25);
            this.GroupBox7.Controls.Add(this.lblLateJobsLine);
            this.GroupBox7.Controls.Add(this.Label6);
            this.GroupBox7.Controls.Add(this.lblEarlyStartViolations);
            this.GroupBox7.Location = new System.Drawing.Point(587, 16);
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.Size = new System.Drawing.Size(205, 171);
            this.GroupBox7.TabIndex = 39;
            this.GroupBox7.TabStop = false;
            this.GroupBox7.Text = "Constraint Violations";
            // 
            // lblBOMViolations
            // 
            this.lblBOMViolations.AutoSize = true;
            this.lblBOMViolations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBOMViolations.Location = new System.Drawing.Point(115, 118);
            this.lblBOMViolations.Name = "lblBOMViolations";
            this.lblBOMViolations.Size = new System.Drawing.Size(13, 13);
            this.lblBOMViolations.TabIndex = 38;
            this.lblBOMViolations.Text = "0";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(6, 118);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(79, 13);
            this.Label5.TabIndex = 37;
            this.Label5.Text = "BOM Violations";
            // 
            // Label24
            // 
            this.Label24.AutoSize = true;
            this.Label24.Location = new System.Drawing.Point(6, 52);
            this.Label24.Name = "Label24";
            this.Label24.Size = new System.Drawing.Size(98, 13);
            this.Label24.TabIndex = 27;
            this.Label24.Text = "Late Jobs - Service";
            // 
            // lblResourceViolations
            // 
            this.lblResourceViolations.AutoSize = true;
            this.lblResourceViolations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResourceViolations.Location = new System.Drawing.Point(115, 96);
            this.lblResourceViolations.Name = "lblResourceViolations";
            this.lblResourceViolations.Size = new System.Drawing.Size(13, 13);
            this.lblResourceViolations.TabIndex = 36;
            this.lblResourceViolations.Text = "0";
            // 
            // lblLateJobsService
            // 
            this.lblLateJobsService.AutoSize = true;
            this.lblLateJobsService.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLateJobsService.Location = new System.Drawing.Point(115, 52);
            this.lblLateJobsService.Name = "lblLateJobsService";
            this.lblLateJobsService.Size = new System.Drawing.Size(13, 13);
            this.lblLateJobsService.TabIndex = 28;
            this.lblLateJobsService.Text = "0";
            // 
            // LineLateJobs
            // 
            this.LineLateJobs.AutoSize = true;
            this.LineLateJobs.Location = new System.Drawing.Point(6, 30);
            this.LineLateJobs.Name = "LineLateJobs";
            this.LineLateJobs.Size = new System.Drawing.Size(82, 13);
            this.LineLateJobs.TabIndex = 31;
            this.LineLateJobs.Text = "Late Jobs - Line";
            // 
            // Label25
            // 
            this.Label25.AutoSize = true;
            this.Label25.Location = new System.Drawing.Point(6, 96);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(101, 13);
            this.Label25.TabIndex = 35;
            this.Label25.Text = "Resource Violations";
            // 
            // lblLateJobsLine
            // 
            this.lblLateJobsLine.AutoSize = true;
            this.lblLateJobsLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLateJobsLine.Location = new System.Drawing.Point(115, 30);
            this.lblLateJobsLine.Name = "lblLateJobsLine";
            this.lblLateJobsLine.Size = new System.Drawing.Size(13, 13);
            this.lblLateJobsLine.TabIndex = 32;
            this.lblLateJobsLine.Text = "0";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(6, 74);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(103, 13);
            this.Label6.TabIndex = 33;
            this.Label6.Text = "Early Start Violations";
            // 
            // lblEarlyStartViolations
            // 
            this.lblEarlyStartViolations.AutoSize = true;
            this.lblEarlyStartViolations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEarlyStartViolations.Location = new System.Drawing.Point(115, 74);
            this.lblEarlyStartViolations.Name = "lblEarlyStartViolations";
            this.lblEarlyStartViolations.Size = new System.Drawing.Size(13, 13);
            this.lblEarlyStartViolations.TabIndex = 34;
            this.lblEarlyStartViolations.Text = "0";
            // 
            // gbResults
            // 
            this.gbResults.Controls.Add(this.lblChangeOverTime);
            this.gbResults.Controls.Add(this.lblRunTime);
            this.gbResults.Controls.Add(this.Label22);
            this.gbResults.Controls.Add(this.lblTotalTime);
            this.gbResults.Controls.Add(this.lblSolveTime);
            this.gbResults.Controls.Add(this.Label21);
            this.gbResults.Controls.Add(this.Label7);
            this.gbResults.Controls.Add(this.Label20);
            this.gbResults.Controls.Add(this.Label3);
            this.gbResults.Controls.Add(this.lblResult);
            this.gbResults.Location = new System.Drawing.Point(376, 16);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(205, 171);
            this.gbResults.TabIndex = 38;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results";
            // 
            // lblChangeOverTime
            // 
            this.lblChangeOverTime.AutoSize = true;
            this.lblChangeOverTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeOverTime.Location = new System.Drawing.Point(115, 91);
            this.lblChangeOverTime.Name = "lblChangeOverTime";
            this.lblChangeOverTime.Size = new System.Drawing.Size(13, 13);
            this.lblChangeOverTime.TabIndex = 26;
            this.lblChangeOverTime.Text = "0";
            // 
            // lblRunTime
            // 
            this.lblRunTime.AutoSize = true;
            this.lblRunTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunTime.Location = new System.Drawing.Point(115, 69);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(13, 13);
            this.lblRunTime.TabIndex = 30;
            this.lblRunTime.Text = "0";
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label22.Location = new System.Drawing.Point(6, 91);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(96, 13);
            this.Label22.TabIndex = 25;
            this.Label22.Text = "Change Over Time";
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTime.Location = new System.Drawing.Point(115, 114);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(13, 13);
            this.lblTotalTime.TabIndex = 24;
            this.lblTotalTime.Text = "0";
            // 
            // lblSolveTime
            // 
            this.lblSolveTime.AutoSize = true;
            this.lblSolveTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolveTime.Location = new System.Drawing.Point(115, 49);
            this.lblSolveTime.Name = "lblSolveTime";
            this.lblSolveTime.Size = new System.Drawing.Size(60, 13);
            this.lblSolveTime.TabIndex = 10;
            this.lblSolveTime.Text = "Solve Time";
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label21.Location = new System.Drawing.Point(6, 69);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(85, 13);
            this.Label21.TabIndex = 29;
            this.Label21.Text = "Processing Time";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(6, 49);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(60, 13);
            this.Label7.TabIndex = 9;
            this.Label7.Text = "Solve Time";
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label20.Location = new System.Drawing.Point(6, 114);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(57, 13);
            this.Label20.TabIndex = 23;
            this.Label20.Text = "Total Time";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(6, 28);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(58, 13);
            this.Label3.TabIndex = 7;
            this.Label3.Text = "Best Value";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(115, 28);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(75, 13);
            this.lblResult.TabIndex = 8;
            this.lblResult.Text = "Solution Value";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.btnSolve);
            this.GroupBox1.Controls.Add(this.tbStartingScheduleName);
            this.GroupBox1.Controls.Add(this.Label29);
            this.GroupBox1.Controls.Add(this.btnSelectStartingSchedule);
            this.GroupBox1.Controls.Add(this.btnSelectSpreadSheet);
            this.GroupBox1.Controls.Add(this.tbWorkBookName);
            this.GroupBox1.Controls.Add(this.Label18);
            this.GroupBox1.Controls.Add(this.cbShowStatus);
            this.GroupBox1.Location = new System.Drawing.Point(8, 16);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(362, 171);
            this.GroupBox1.TabIndex = 18;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Run Conditions";
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(281, 138);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(75, 23);
            this.btnSolve.TabIndex = 2;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // tbStartingScheduleName
            // 
            this.tbStartingScheduleName.Location = new System.Drawing.Point(9, 85);
            this.tbStartingScheduleName.Name = "tbStartingScheduleName";
            this.tbStartingScheduleName.Size = new System.Drawing.Size(251, 20);
            this.tbStartingScheduleName.TabIndex = 20;
            // 
            // Label29
            // 
            this.Label29.AutoSize = true;
            this.Label29.Location = new System.Drawing.Point(6, 69);
            this.Label29.Name = "Label29";
            this.Label29.Size = new System.Drawing.Size(118, 13);
            this.Label29.TabIndex = 21;
            this.Label29.Text = "Load Starting Schedule";
            // 
            // btnSelectStartingSchedule
            // 
            this.btnSelectStartingSchedule.AccessibleName = "";
            this.btnSelectStartingSchedule.Location = new System.Drawing.Point(281, 83);
            this.btnSelectStartingSchedule.Name = "btnSelectStartingSchedule";
            this.btnSelectStartingSchedule.Size = new System.Drawing.Size(75, 23);
            this.btnSelectStartingSchedule.TabIndex = 19;
            this.btnSelectStartingSchedule.Text = "Browse";
            this.btnSelectStartingSchedule.UseVisualStyleBackColor = true;
            this.btnSelectStartingSchedule.Click += new System.EventHandler(this.btnSelectStartingSchedule_Click);
            // 
            // btnSelectSpreadSheet
            // 
            this.btnSelectSpreadSheet.AccessibleName = "";
            this.btnSelectSpreadSheet.Location = new System.Drawing.Point(281, 44);
            this.btnSelectSpreadSheet.Name = "btnSelectSpreadSheet";
            this.btnSelectSpreadSheet.Size = new System.Drawing.Size(75, 23);
            this.btnSelectSpreadSheet.TabIndex = 1;
            this.btnSelectSpreadSheet.Text = "Browse";
            this.btnSelectSpreadSheet.UseVisualStyleBackColor = true;
            this.btnSelectSpreadSheet.Click += new System.EventHandler(this.btnSelectSpreadSheet_Click);
            // 
            // tbWorkBookName
            // 
            this.tbWorkBookName.Location = new System.Drawing.Point(9, 44);
            this.tbWorkBookName.Name = "tbWorkBookName";
            this.tbWorkBookName.Size = new System.Drawing.Size(251, 20);
            this.tbWorkBookName.TabIndex = 16;
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(6, 28);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(92, 13);
            this.Label18.TabIndex = 17;
            this.Label18.Text = "Work Book Name";
            // 
            // cbShowStatus
            // 
            this.cbShowStatus.AutoSize = true;
            this.cbShowStatus.Checked = true;
            this.cbShowStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowStatus.Location = new System.Drawing.Point(9, 144);
            this.cbShowStatus.Name = "cbShowStatus";
            this.cbShowStatus.Size = new System.Drawing.Size(159, 17);
            this.cbShowStatus.TabIndex = 13;
            this.cbShowStatus.Text = "Show Status While Running";
            this.cbShowStatus.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.tbRandomSeed);
            this.tabPage2.Controls.Add(this.GroupBox10);
            this.tabPage2.Controls.Add(this.GroupBox9);
            this.tabPage2.Controls.Add(this.gbSurvival);
            this.tabPage2.Controls.Add(this.Label26);
            this.tabPage2.Controls.Add(this.Label23);
            this.tabPage2.Controls.Add(this.tbMeanDelay);
            this.tabPage2.Controls.Add(this.tbDelayProb);
            this.tabPage2.Controls.Add(this.Label9);
            this.tabPage2.Controls.Add(this.tbDeathRate);
            this.tabPage2.Controls.Add(this.Label2);
            this.tabPage2.Controls.Add(this.tbGenerations);
            this.tabPage2.Controls.Add(this.Label8);
            this.tabPage2.Controls.Add(this.Label1);
            this.tabPage2.Controls.Add(this.tbHerdSize);
            this.tabPage2.Controls.Add(this.tbMutationProbability);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(824, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Optimization Parameters";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GroupBox10
            // 
            this.GroupBox10.Controls.Add(this.rbMeanWithNoise);
            this.GroupBox10.Controls.Add(this.rbUniform);
            this.GroupBox10.Location = new System.Drawing.Point(141, 204);
            this.GroupBox10.Name = "GroupBox10";
            this.GroupBox10.Size = new System.Drawing.Size(136, 66);
            this.GroupBox10.TabIndex = 58;
            this.GroupBox10.TabStop = false;
            this.GroupBox10.Text = "Delay Times Crossover";
            // 
            // rbMeanWithNoise
            // 
            this.rbMeanWithNoise.AutoSize = true;
            this.rbMeanWithNoise.Checked = true;
            this.rbMeanWithNoise.Location = new System.Drawing.Point(6, 17);
            this.rbMeanWithNoise.Name = "rbMeanWithNoise";
            this.rbMeanWithNoise.Size = new System.Drawing.Size(104, 17);
            this.rbMeanWithNoise.TabIndex = 1;
            this.rbMeanWithNoise.TabStop = true;
            this.rbMeanWithNoise.Text = "Mean with Noise";
            this.rbMeanWithNoise.UseVisualStyleBackColor = true;
            // 
            // rbUniform
            // 
            this.rbUniform.AutoSize = true;
            this.rbUniform.Location = new System.Drawing.Point(6, 40);
            this.rbUniform.Name = "rbUniform";
            this.rbUniform.Size = new System.Drawing.Size(61, 17);
            this.rbUniform.TabIndex = 0;
            this.rbUniform.Text = "Uniform";
            this.rbUniform.UseVisualStyleBackColor = true;
            // 
            // GroupBox9
            // 
            this.GroupBox9.Controls.Add(this.rbTournament);
            this.GroupBox9.Controls.Add(this.rbFitnessProportional);
            this.GroupBox9.Location = new System.Drawing.Point(141, 129);
            this.GroupBox9.Name = "GroupBox9";
            this.GroupBox9.Size = new System.Drawing.Size(121, 69);
            this.GroupBox9.TabIndex = 57;
            this.GroupBox9.TabStop = false;
            this.GroupBox9.Text = "Parent Selection";
            // 
            // rbTournament
            // 
            this.rbTournament.AutoSize = true;
            this.rbTournament.Checked = true;
            this.rbTournament.Location = new System.Drawing.Point(6, 42);
            this.rbTournament.Name = "rbTournament";
            this.rbTournament.Size = new System.Drawing.Size(82, 17);
            this.rbTournament.TabIndex = 1;
            this.rbTournament.TabStop = true;
            this.rbTournament.Text = "Tournament";
            this.rbTournament.UseVisualStyleBackColor = true;
            // 
            // rbFitnessProportional
            // 
            this.rbFitnessProportional.AutoSize = true;
            this.rbFitnessProportional.Location = new System.Drawing.Point(6, 19);
            this.rbFitnessProportional.Name = "rbFitnessProportional";
            this.rbFitnessProportional.Size = new System.Drawing.Size(117, 17);
            this.rbFitnessProportional.TabIndex = 0;
            this.rbFitnessProportional.Text = "Fitness Proportional";
            this.rbFitnessProportional.UseVisualStyleBackColor = true;
            // 
            // gbSurvival
            // 
            this.gbSurvival.Controls.Add(this.rbReplaceWorst);
            this.gbSurvival.Controls.Add(this.rbStruggle);
            this.gbSurvival.Controls.Add(this.rbGenerational);
            this.gbSurvival.Controls.Add(this.rbElitist);
            this.gbSurvival.Location = new System.Drawing.Point(11, 129);
            this.gbSurvival.Name = "gbSurvival";
            this.gbSurvival.Size = new System.Drawing.Size(115, 120);
            this.gbSurvival.TabIndex = 56;
            this.gbSurvival.TabStop = false;
            this.gbSurvival.Text = "Survival Selection";
            // 
            // rbReplaceWorst
            // 
            this.rbReplaceWorst.AutoSize = true;
            this.rbReplaceWorst.Location = new System.Drawing.Point(6, 65);
            this.rbReplaceWorst.Name = "rbReplaceWorst";
            this.rbReplaceWorst.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbReplaceWorst.Size = new System.Drawing.Size(93, 17);
            this.rbReplaceWorst.TabIndex = 3;
            this.rbReplaceWorst.Text = "ReplaceWorst";
            this.rbReplaceWorst.UseVisualStyleBackColor = true;
            // 
            // rbStruggle
            // 
            this.rbStruggle.AutoSize = true;
            this.rbStruggle.Location = new System.Drawing.Point(6, 42);
            this.rbStruggle.Name = "rbStruggle";
            this.rbStruggle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbStruggle.Size = new System.Drawing.Size(64, 17);
            this.rbStruggle.TabIndex = 2;
            this.rbStruggle.Text = "Struggle";
            this.rbStruggle.UseVisualStyleBackColor = true;
            // 
            // rbGenerational
            // 
            this.rbGenerational.AutoSize = true;
            this.rbGenerational.Location = new System.Drawing.Point(6, 88);
            this.rbGenerational.Name = "rbGenerational";
            this.rbGenerational.Size = new System.Drawing.Size(85, 17);
            this.rbGenerational.TabIndex = 1;
            this.rbGenerational.Text = "Generational";
            this.rbGenerational.UseVisualStyleBackColor = true;
            // 
            // rbElitist
            // 
            this.rbElitist.AutoSize = true;
            this.rbElitist.Checked = true;
            this.rbElitist.Location = new System.Drawing.Point(6, 19);
            this.rbElitist.Name = "rbElitist";
            this.rbElitist.Size = new System.Drawing.Size(49, 17);
            this.rbElitist.TabIndex = 0;
            this.rbElitist.TabStop = true;
            this.rbElitist.Text = "Elitist";
            this.rbElitist.UseVisualStyleBackColor = true;
            // 
            // Label26
            // 
            this.Label26.AutoSize = true;
            this.Label26.Location = new System.Drawing.Point(8, 342);
            this.Label26.Name = "Label26";
            this.Label26.Size = new System.Drawing.Size(130, 13);
            this.Label26.TabIndex = 55;
            this.Label26.Text = "Mean Delay (in unit hours)";
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.Location = new System.Drawing.Point(8, 300);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(85, 13);
            this.Label23.TabIndex = 54;
            this.Label23.Text = "Delay Probability";
            // 
            // tbMeanDelay
            // 
            this.tbMeanDelay.Location = new System.Drawing.Point(11, 358);
            this.tbMeanDelay.Name = "tbMeanDelay";
            this.tbMeanDelay.Size = new System.Drawing.Size(100, 20);
            this.tbMeanDelay.TabIndex = 53;
            this.tbMeanDelay.Text = "24.0";
            // 
            // tbDelayProb
            // 
            this.tbDelayProb.Location = new System.Drawing.Point(11, 316);
            this.tbDelayProb.Name = "tbDelayProb";
            this.tbDelayProb.Size = new System.Drawing.Size(100, 20);
            this.tbDelayProb.TabIndex = 52;
            this.tbDelayProb.Text = "1.0";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(138, 75);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(81, 13);
            this.Label9.TabIndex = 51;
            this.Label9.Text = "% Replacement";
            // 
            // tbDeathRate
            // 
            this.tbDeathRate.Location = new System.Drawing.Point(141, 91);
            this.tbDeathRate.Name = "tbDeathRate";
            this.tbDeathRate.Size = new System.Drawing.Size(100, 20);
            this.tbDeathRate.TabIndex = 50;
            this.tbDeathRate.Text = "50";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(8, 23);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(116, 13);
            this.Label2.TabIndex = 47;
            this.Label2.Text = "Number of Generations";
            // 
            // tbGenerations
            // 
            this.tbGenerations.Location = new System.Drawing.Point(11, 39);
            this.tbGenerations.Name = "tbGenerations";
            this.tbGenerations.Size = new System.Drawing.Size(100, 20);
            this.tbGenerations.TabIndex = 46;
            this.tbGenerations.Text = "2000";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(138, 23);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(109, 13);
            this.Label8.TabIndex = 49;
            this.Label8.Text = "Number of Individuals";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(8, 75);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(99, 13);
            this.Label1.TabIndex = 45;
            this.Label1.Text = "Mutation Probability";
            // 
            // tbHerdSize
            // 
            this.tbHerdSize.Location = new System.Drawing.Point(138, 39);
            this.tbHerdSize.Name = "tbHerdSize";
            this.tbHerdSize.Size = new System.Drawing.Size(100, 20);
            this.tbHerdSize.TabIndex = 48;
            this.tbHerdSize.Text = "200";
            // 
            // tbMutationProbability
            // 
            this.tbMutationProbability.Location = new System.Drawing.Point(11, 91);
            this.tbMutationProbability.Name = "tbMutationProbability";
            this.tbMutationProbability.Size = new System.Drawing.Size(100, 20);
            this.tbMutationProbability.TabIndex = 44;
            this.tbMutationProbability.Text = "0.05";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.GroupBox6);
            this.tabPage3.Controls.Add(this.GroupBox5);
            this.tabPage3.Controls.Add(this.GroupBox4);
            this.tabPage3.Controls.Add(this.GroupBox3);
            this.tabPage3.Controls.Add(this.GroupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(824, 523);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Production Parameters";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.Label30);
            this.GroupBox6.Controls.Add(this.tbComponentShortagePenalty);
            this.GroupBox6.Location = new System.Drawing.Point(539, 183);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Size = new System.Drawing.Size(193, 93);
            this.GroupBox6.TabIndex = 55;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "BOM Parameters";
            // 
            // Label30
            // 
            this.Label30.AutoSize = true;
            this.Label30.Location = new System.Drawing.Point(18, 39);
            this.Label30.Name = "Label30";
            this.Label30.Size = new System.Drawing.Size(145, 13);
            this.Label30.TabIndex = 44;
            this.Label30.Text = "Component Shortage Penalty";
            // 
            // tbComponentShortagePenalty
            // 
            this.tbComponentShortagePenalty.Location = new System.Drawing.Point(21, 55);
            this.tbComponentShortagePenalty.Name = "tbComponentShortagePenalty";
            this.tbComponentShortagePenalty.Size = new System.Drawing.Size(100, 20);
            this.tbComponentShortagePenalty.TabIndex = 43;
            this.tbComponentShortagePenalty.Text = "300";
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.Label27);
            this.GroupBox5.Controls.Add(this.tbLineInfeasibility);
            this.GroupBox5.Controls.Add(this.Label28);
            this.GroupBox5.Controls.Add(this.tbLineAffinity);
            this.GroupBox5.Location = new System.Drawing.Point(8, 294);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(282, 93);
            this.GroupBox5.TabIndex = 54;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "Production Line Parameters";
            // 
            // Label27
            // 
            this.Label27.AutoSize = true;
            this.Label27.Location = new System.Drawing.Point(135, 39);
            this.Label27.Name = "Label27";
            this.Label27.Size = new System.Drawing.Size(58, 13);
            this.Label27.TabIndex = 46;
            this.Label27.Text = "Infeasibility";
            // 
            // tbLineInfeasibility
            // 
            this.tbLineInfeasibility.Location = new System.Drawing.Point(138, 55);
            this.tbLineInfeasibility.Name = "tbLineInfeasibility";
            this.tbLineInfeasibility.Size = new System.Drawing.Size(100, 20);
            this.tbLineInfeasibility.TabIndex = 45;
            this.tbLineInfeasibility.Text = "4000";
            // 
            // Label28
            // 
            this.Label28.AutoSize = true;
            this.Label28.Location = new System.Drawing.Point(18, 39);
            this.Label28.Name = "Label28";
            this.Label28.Size = new System.Drawing.Size(61, 13);
            this.Label28.TabIndex = 44;
            this.Label28.Text = "Line Affinity";
            // 
            // tbLineAffinity
            // 
            this.tbLineAffinity.Location = new System.Drawing.Point(21, 55);
            this.tbLineAffinity.Name = "tbLineAffinity";
            this.tbLineAffinity.Size = new System.Drawing.Size(100, 20);
            this.tbLineAffinity.TabIndex = 43;
            this.tbLineAffinity.Text = "50";
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.Label19);
            this.GroupBox4.Controls.Add(this.tbWashTime);
            this.GroupBox4.Location = new System.Drawing.Point(539, 6);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(193, 112);
            this.GroupBox4.TabIndex = 53;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Allergen Impact";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(18, 39);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(93, 13);
            this.Label19.TabIndex = 44;
            this.Label19.Text = "Alergen Alert Time";
            // 
            // tbWashTime
            // 
            this.tbWashTime.Location = new System.Drawing.Point(21, 55);
            this.tbWashTime.Name = "tbWashTime";
            this.tbWashTime.Size = new System.Drawing.Size(100, 20);
            this.tbWashTime.TabIndex = 43;
            this.tbWashTime.Text = "0";
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.Label15);
            this.GroupBox3.Controls.Add(this.tbLateCost);
            this.GroupBox3.Controls.Add(this.Label10);
            this.GroupBox3.Controls.Add(this.tbOvertimeIncrementCost);
            this.GroupBox3.Controls.Add(this.Label11);
            this.GroupBox3.Controls.Add(this.tbStandardCost);
            this.GroupBox3.Enabled = false;
            this.GroupBox3.Location = new System.Drawing.Point(311, 6);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(193, 270);
            this.GroupBox3.TabIndex = 52;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Cost Parameters";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(33, 177);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(52, 13);
            this.Label15.TabIndex = 45;
            this.Label15.Text = "Late Cost";
            // 
            // tbLateCost
            // 
            this.tbLateCost.Location = new System.Drawing.Point(36, 193);
            this.tbLateCost.Name = "tbLateCost";
            this.tbLateCost.Size = new System.Drawing.Size(100, 20);
            this.tbLateCost.TabIndex = 44;
            this.tbLateCost.Text = "2000";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(33, 101);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(147, 13);
            this.Label10.TabIndex = 43;
            this.Label10.Text = "Overtime Incremental Cost/Hr";
            // 
            // tbOvertimeIncrementCost
            // 
            this.tbOvertimeIncrementCost.Location = new System.Drawing.Point(36, 117);
            this.tbOvertimeIncrementCost.Name = "tbOvertimeIncrementCost";
            this.tbOvertimeIncrementCost.Size = new System.Drawing.Size(100, 20);
            this.tbOvertimeIncrementCost.TabIndex = 42;
            this.tbOvertimeIncrementCost.Text = "50";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(33, 34);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(95, 13);
            this.Label11.TabIndex = 41;
            this.Label11.Text = "Standard Cost/HR";
            // 
            // tbStandardCost
            // 
            this.tbStandardCost.Location = new System.Drawing.Point(36, 50);
            this.tbStandardCost.Name = "tbStandardCost";
            this.tbStandardCost.Size = new System.Drawing.Size(100, 20);
            this.tbStandardCost.TabIndex = 40;
            this.tbStandardCost.Text = "100";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.Label16);
            this.GroupBox2.Controls.Add(this.dtpProductionEndDate);
            this.GroupBox2.Controls.Add(this.Label17);
            this.GroupBox2.Controls.Add(this.dtpProductionDate);
            this.GroupBox2.Controls.Add(this.Label13);
            this.GroupBox2.Controls.Add(this.dtpProductionStartTime);
            this.GroupBox2.Controls.Add(this.dtpProductionEndTime);
            this.GroupBox2.Controls.Add(this.Label14);
            this.GroupBox2.Controls.Add(this.tbOvertimeStart);
            this.GroupBox2.Controls.Add(this.Label12);
            this.GroupBox2.Enabled = false;
            this.GroupBox2.Location = new System.Drawing.Point(8, 6);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(282, 270);
            this.GroupBox2.TabIndex = 51;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Time Parameters";
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(19, 99);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(106, 13);
            this.Label16.TabIndex = 60;
            this.Label16.Text = "Production End Date";
            // 
            // dtpProductionEndDate
            // 
            this.dtpProductionEndDate.Checked = false;
            this.dtpProductionEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpProductionEndDate.Location = new System.Drawing.Point(22, 115);
            this.dtpProductionEndDate.Name = "dtpProductionEndDate";
            this.dtpProductionEndDate.ShowUpDown = true;
            this.dtpProductionEndDate.Size = new System.Drawing.Size(106, 20);
            this.dtpProductionEndDate.TabIndex = 61;
            this.dtpProductionEndDate.Value = new System.DateTime(2007, 12, 31, 0, 0, 0, 0);
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(19, 31);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(109, 13);
            this.Label17.TabIndex = 58;
            this.Label17.Text = "Production Start Date";
            // 
            // dtpProductionDate
            // 
            this.dtpProductionDate.Checked = false;
            this.dtpProductionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpProductionDate.Location = new System.Drawing.Point(22, 47);
            this.dtpProductionDate.Name = "dtpProductionDate";
            this.dtpProductionDate.ShowUpDown = true;
            this.dtpProductionDate.Size = new System.Drawing.Size(106, 20);
            this.dtpProductionDate.TabIndex = 59;
            this.dtpProductionDate.Value = new System.DateTime(2007, 12, 31, 0, 0, 0, 0);
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(147, 31);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(109, 13);
            this.Label13.TabIndex = 56;
            this.Label13.Text = "Production Start Time";
            // 
            // dtpProductionStartTime
            // 
            this.dtpProductionStartTime.Checked = false;
            this.dtpProductionStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpProductionStartTime.Location = new System.Drawing.Point(150, 47);
            this.dtpProductionStartTime.Name = "dtpProductionStartTime";
            this.dtpProductionStartTime.ShowUpDown = true;
            this.dtpProductionStartTime.Size = new System.Drawing.Size(106, 20);
            this.dtpProductionStartTime.TabIndex = 57;
            this.dtpProductionStartTime.Value = new System.DateTime(2007, 12, 31, 0, 0, 0, 0);
            // 
            // dtpProductionEndTime
            // 
            this.dtpProductionEndTime.Checked = false;
            this.dtpProductionEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpProductionEndTime.Location = new System.Drawing.Point(150, 115);
            this.dtpProductionEndTime.Name = "dtpProductionEndTime";
            this.dtpProductionEndTime.ShowUpDown = true;
            this.dtpProductionEndTime.Size = new System.Drawing.Size(106, 20);
            this.dtpProductionEndTime.TabIndex = 54;
            this.dtpProductionEndTime.Value = new System.DateTime(2007, 12, 31, 0, 0, 0, 0);
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(147, 99);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(106, 13);
            this.Label14.TabIndex = 45;
            this.Label14.Text = "Production End Time";
            // 
            // tbOvertimeStart
            // 
            this.tbOvertimeStart.Enabled = false;
            this.tbOvertimeStart.Location = new System.Drawing.Point(22, 193);
            this.tbOvertimeStart.Name = "tbOvertimeStart";
            this.tbOvertimeStart.Size = new System.Drawing.Size(100, 20);
            this.tbOvertimeStart.TabIndex = 38;
            this.tbOvertimeStart.Text = "1730";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(19, 177);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(74, 13);
            this.Label12.TabIndex = 39;
            this.Label12.Text = "Overtime Start";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvSchedule);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(824, 523);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Schedule Results";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvSchedule
            // 
            this.dgvSchedule.AllowUserToAddRows = false;
            this.dgvSchedule.AllowUserToDeleteRows = false;
            this.dgvSchedule.AllowUserToOrderColumns = true;
            this.dgvSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSchedule.Location = new System.Drawing.Point(3, 3);
            this.dgvSchedule.Name = "dgvSchedule";
            this.dgvSchedule.ReadOnly = true;
            this.dgvSchedule.Size = new System.Drawing.Size(818, 517);
            this.dgvSchedule.TabIndex = 1;
            this.dgvSchedule.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSchedule_Paint);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "RNG Seed";
            // 
            // tbRandomSeed
            // 
            this.tbRandomSeed.Location = new System.Drawing.Point(147, 316);
            this.tbRandomSeed.Name = "tbRandomSeed";
            this.tbRandomSeed.Size = new System.Drawing.Size(100, 20);
            this.tbRandomSeed.TabIndex = 59;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 549);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Genetic Optimizer Scheduler: Demo and Testing Interface";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.GroupBox7.ResumeLayout(false);
            this.GroupBox7.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.GroupBox10.ResumeLayout(false);
            this.GroupBox10.PerformLayout();
            this.GroupBox9.ResumeLayout(false);
            this.GroupBox9.PerformLayout();
            this.gbSurvival.ResumeLayout(false);
            this.gbSurvival.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.GroupBox6.ResumeLayout(false);
            this.GroupBox6.PerformLayout();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox5.PerformLayout();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.TextBox tbStartingScheduleName;
        internal System.Windows.Forms.Label Label29;
        internal System.Windows.Forms.Button btnSelectStartingSchedule;
        internal System.Windows.Forms.Button btnSelectSpreadSheet;
        internal System.Windows.Forms.TextBox tbWorkBookName;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.CheckBox cbShowStatus;
        internal System.Windows.Forms.Button btnSolve;
        internal System.Windows.Forms.GroupBox GroupBox10;
        internal System.Windows.Forms.RadioButton rbMeanWithNoise;
        internal System.Windows.Forms.RadioButton rbUniform;
        internal System.Windows.Forms.GroupBox GroupBox9;
        internal System.Windows.Forms.RadioButton rbTournament;
        internal System.Windows.Forms.RadioButton rbFitnessProportional;
        internal System.Windows.Forms.GroupBox gbSurvival;
        internal System.Windows.Forms.RadioButton rbReplaceWorst;
        internal System.Windows.Forms.RadioButton rbStruggle;
        internal System.Windows.Forms.RadioButton rbGenerational;
        internal System.Windows.Forms.RadioButton rbElitist;
        internal System.Windows.Forms.Label Label26;
        internal System.Windows.Forms.Label Label23;
        internal System.Windows.Forms.TextBox tbMeanDelay;
        internal System.Windows.Forms.TextBox tbDelayProb;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox tbDeathRate;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox tbGenerations;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbHerdSize;
        internal System.Windows.Forms.TextBox tbMutationProbability;
        private System.Windows.Forms.TabPage tabPage3;
        internal System.Windows.Forms.GroupBox GroupBox6;
        internal System.Windows.Forms.Label Label30;
        internal System.Windows.Forms.TextBox tbComponentShortagePenalty;
        internal System.Windows.Forms.GroupBox GroupBox5;
        internal System.Windows.Forms.Label Label27;
        internal System.Windows.Forms.TextBox tbLineInfeasibility;
        internal System.Windows.Forms.Label Label28;
        internal System.Windows.Forms.TextBox tbLineAffinity;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.Label Label19;
        internal System.Windows.Forms.TextBox tbWashTime;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.Label Label15;
        internal System.Windows.Forms.TextBox tbLateCost;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.TextBox tbOvertimeIncrementCost;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.TextBox tbStandardCost;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.DateTimePicker dtpProductionEndDate;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.DateTimePicker dtpProductionDate;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.DateTimePicker dtpProductionStartTime;
        internal System.Windows.Forms.DateTimePicker dtpProductionEndTime;
        internal System.Windows.Forms.Label Label14;
        internal System.Windows.Forms.TextBox tbOvertimeStart;
        internal System.Windows.Forms.Label Label12;
        private System.Windows.Forms.TabPage tabPage4;
        internal System.Windows.Forms.DataGridView dgvSchedule;
        internal System.Windows.Forms.GroupBox GroupBox7;
        internal System.Windows.Forms.Label lblBOMViolations;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label24;
        internal System.Windows.Forms.Label lblResourceViolations;
        internal System.Windows.Forms.Label lblLateJobsService;
        internal System.Windows.Forms.Label LineLateJobs;
        internal System.Windows.Forms.Label Label25;
        internal System.Windows.Forms.Label lblLateJobsLine;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label lblEarlyStartViolations;
        internal System.Windows.Forms.GroupBox gbResults;
        internal System.Windows.Forms.Label lblChangeOverTime;
        internal System.Windows.Forms.Label lblRunTime;
        internal System.Windows.Forms.Label Label22;
        internal System.Windows.Forms.Label lblTotalTime;
        internal System.Windows.Forms.Label lblSolveTime;
        internal System.Windows.Forms.Label Label21;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label20;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOutputToExcel;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox tbRandomSeed;
    }
}

