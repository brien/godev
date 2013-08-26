// -----------------------------------------------------------------------
// <copyright file="GanttChart.cs">
// http://www.codeproject.com/Articles/20731/Gantt-Chart
// <summary>
// Adds an easy to use Gantt Chart to your application
// Created by Adrian "Adagio" Grau
// Version 0.55
// </summary>
// <remarks></remarks>
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// TODO: Update summary.
/// </summary>
public class GanttChart : Control
{

    private MouseOverPart mouseHoverPart = MouseOverPart.Empty;

    private int mouseHoverBarIndex = -1;
    private List<ChartBarDate> bars = new List<ChartBarDate>();
    private System.DateTime headerFromDate;

    private System.DateTime headerToDate;

    private int barIsChanging = -1;
    private int barStartRight = 20;
    private int barStartLeft = 100;
    private int headerTimeStartTop = 30;

    private List<Header> shownHeaderList;
    private int barStartTop = 50;
    private int barHeight = 9;

    private int barSpace = 5;

    private int widthPerItem;
    private System.DateTime _mouseOverColumnValue;
    private string _mouseOverRowText = "";

    private object _mouseOverRowValue = null;
    private Pen lineColor = Pens.Bisque;
    private Font dateTextFont = new Font("VERDANA", 8.0f, FontStyle.Regular, GraphicsUnit.Point);
    private Font timeTextFont = new Font("VERDANA", 8.0f, FontStyle.Regular, GraphicsUnit.Point);

    private Font rowTextFont = new Font("VERDANA", 8.0f, FontStyle.Regular, GraphicsUnit.Point);
    private System.Windows.Forms.ToolTip withEventsField_ToolTip = new System.Windows.Forms.ToolTip();
    internal System.Windows.Forms.ToolTip ToolTip
    {
        get { return withEventsField_ToolTip; }
        set
        {
            if (withEventsField_ToolTip != null)
            {
                withEventsField_ToolTip.Draw -= ToolTipText_Draw;
                withEventsField_ToolTip.Popup -= ToolTipText_Popup;
            }
            withEventsField_ToolTip = value;
            if (withEventsField_ToolTip != null)
            {
                withEventsField_ToolTip.Draw += ToolTipText_Draw;
                withEventsField_ToolTip.Popup += ToolTipText_Popup;
            }
        }

    }

    private bool _allowEditBarWithMouse = false;
    public event MouseDraggedEventHandler MouseDragged;
    public delegate void MouseDraggedEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
    public event BarChangedEventHandler BarChanged;
    public delegate void BarChangedEventHandler(object sender, object barValue);

    private Bitmap objBmp;

    private Graphics objGraphics;

    protected new bool DesignMode
    {
        get
        {
            if (base.DesignMode)
                return true;

            return System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;
        }
    }

    #region "Public properties"

    /// <summary>
    /// Sets to true if the user should be able to manually edit bars
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public bool AllowManualEditBar
    {
        get { return _allowEditBarWithMouse; }
        set { _allowEditBarWithMouse = value; }
    }

    /// <summary>
    /// The start date/time of the chart
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public System.DateTime FromDate
    {
        get { return headerFromDate; }
        set { headerFromDate = value; }
    }

    /// <summary>
    /// The end date/time of the chart
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public System.DateTime ToDate
    {
        get { return headerToDate; }
        set { headerToDate = value; }
    }

    /// <summary>
    /// The text for the current row the mouse hovers above
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public string MouseOverRowText
    {
        get { return _mouseOverRowText; }
    }

    /// <summary>
    /// The value for the current bar the mouse hovers above
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public object MouseOverRowValue
    {
        get { return _mouseOverRowValue; }
    }

    /// <summary>
    /// The date/time the mouse hovers above
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public System.DateTime MouseOverColumnDate
    {
        get { return _mouseOverColumnValue; }
    }

    /// <summary>
    /// The color of the grid
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public System.Drawing.Pen GridColor
    {
        get { return lineColor; }
        set { lineColor = value; }
    }

    /// <summary>
    /// The font used for the row text
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public Font RowFont
    {
        get { return rowTextFont; }
        set { rowTextFont = value; }
    }

    /// <summary>
    /// The font used for the "date" text in the columns
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public Font DateFont
    {
        get { return dateTextFont; }
        set { dateTextFont = value; }
    }

    /// <summary>
    /// The font used for the "time" text in the colums)
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public Font TimeFont
    {
        get { return timeTextFont; }
        set { timeTextFont = value; }
    }

    #endregion

    #region "Constructor"

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <remarks></remarks>

    public GanttChart()
    {
        if (!DesignMode)
        {

            MouseWheel += GanttChart_MouseWheel;
            MouseClick += GanttChart_Click;
            MouseDragged += GanttChart_MouseDragged;
            MouseLeave += GanttChart_MouseLeave;
            MouseMove += GanttChart_MouseMove;
            ToolTip.AutoPopDelay = 15000;
            ToolTip.InitialDelay = 250;
            ToolTip.OwnerDraw = true;

            objBmp = new Bitmap(1280, 1024, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            objGraphics = Graphics.FromImage(objBmp);

            // Flicker free drawing

            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

    }

    #endregion

    #region "Bars"

    private void SetBarStartLeft(string rowText)
    {
        Graphics gfx = this.CreateGraphics();

        int length = Convert.ToInt32(gfx.MeasureString(rowText, rowTextFont, 500).Width);

        if (length > barStartLeft)
        {
            barStartLeft = length;
        }
    }

    /// <summary>
    /// Adds a bar to the list
    /// </summary>
    /// <param name="rowText">Text for the row</param>
    /// <param name="barValue">Value for the row</param>
    /// <param name="fromTime">The date/time the bar starts</param>
    /// <param name="toTime">The date/time the bar ends</param>
    /// <param name="color">The color of the bar</param>
    /// <param name="hoverColor">The hover color of the bar</param>
    /// <param name="rowIndex">The rowindex of the bar (useful if you want several bars on the same row)</param>
    /// <remarks></remarks>

    public void AddChartBar(string rowText, object barValue, System.DateTime fromTime, System.DateTime toTime, Color color, Color hoverColor, int rowIndex)
    {
        ChartBarDate bar = new ChartBarDate();
        bar.Text = rowText;
        bar.Value = barValue;
        bar.StartValue = fromTime;
        bar.EndValue = toTime;
        bar.Color = color;
        bar.HoverColor = hoverColor;
        bar.RowIndex = rowIndex;
        bars.Add(bar);

        SetBarStartLeft(rowText);
    }

    /// <summary>
    /// Adds a bar to the list
    /// </summary>
    /// <param name="rowText">Text for the row</param>
    /// <param name="barValue">Value for the row</param>
    /// <param name="fromTime">The date/time the bar starts</param>
    /// <param name="toTime">The date/time the bar ends</param>
    /// <param name="color">The color of the bar</param>
    /// <param name="hoverColor">The hover color of the bar</param>
    /// <param name="rowIndex">The rowindex of the bar (useful if you want several bars on the same row)</param>
    /// <param name="hideFromMouseMove">If you want to "hide" the bar from mousemove event</param>
    /// <remarks></remarks>

    public void AddChartBar(string rowText, object barValue, System.DateTime fromTime, System.DateTime toTime, Color color, Color hoverColor, int rowIndex, bool hideFromMouseMove)
    {
        ChartBarDate bar = new ChartBarDate();
        bar.Text = rowText;
        bar.Value = barValue;
        bar.StartValue = fromTime;
        bar.EndValue = toTime;
        bar.Color = color;
        bar.HoverColor = hoverColor;
        bar.RowIndex = rowIndex;
        bar.HideFromMouseMove = hideFromMouseMove;
        bars.Add(bar);

        SetBarStartLeft(rowText);
    }

    /// <summary>
    /// Gets the next index
    /// </summary>
    /// <param name="rowText"></param>
    /// <returns></returns>
    /// <remarks></remarks>

    public int GetIndexChartBar(string rowText)
    {
        int index = -1;

        foreach (ChartBarDate bar in bars)
        {
            if (bar.Text.Equals(rowText) == true)
            {
                return bar.RowIndex;
            }
            if (bar.RowIndex > index)
            {
                index = bar.RowIndex;
            }
        }

        return index + 1;
    }

    /// <summary>
    /// Removes all bars from list
    /// </summary>
    /// <remarks></remarks>

    public void RemoveBars()
    {
        bars = new List<ChartBarDate>();

        barStartLeft = 100;
    }

    #endregion

    #region "Draw"

    /// <summary>
    /// Redraws the Gantt chart
    /// </summary>
    /// <remarks></remarks>

    public void PaintChart()
    {
        this.Invalidate();
    }

    /// <summary>
    /// Redraws the Gantt chart
    /// </summary>
    /// <param name="gfx"></param>
    /// <remarks></remarks>

    private void PaintChart(Graphics gfx)
    {
        gfx.Clear(this.BackColor);

        if (headerFromDate == null | headerToDate == null)
            return;

        DrawScrollBar(gfx);
        DrawHeader(gfx, null);
        DrawNetHorizontal(gfx);
        DrawNetVertical(gfx);
        DrawBars(gfx);

        objBmp = new Bitmap(this.Width - barStartRight, lastLineStop, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        objGraphics = Graphics.FromImage(objBmp);
    }

    /// <summary>
    /// Redraws the Gantt chart
    /// </summary>
    /// <param name="pe"></param>
    /// <remarks></remarks>

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs pe)
    {
        base.OnPaint(pe);
        if (!DesignMode)
        {
            PaintChart(pe.Graphics);
        }
    }

    /// <summary>
    /// Draws the list of headers. Automatically shows which headers to draw, based on the width of the Gantt Chart
    /// </summary>
    /// <param name="gfx"></param>
    /// <param name="headerList"></param>
    /// <remarks></remarks>

    private void DrawHeader(Graphics gfx, List<Header> headerList)
    {
        if (headerList == null)
        {
            headerList = GetFullHeaderList();
        }

        if (headerList.Count == 0)
            return;

        dynamic availableWidth = this.Width - 10 - barStartLeft - barStartRight;
        widthPerItem = availableWidth / headerList.Count;

        if (widthPerItem < 40)
        {
            List<Header> newHeaderList = new List<Header>();

            bool showNext = true;

            // If there's not enough room for all headers remove 50%

            foreach (Header header in headerList)
            {
                if (showNext == true)
                {
                    newHeaderList.Add(header);
                    showNext = false;
                }
                else
                {
                    showNext = true;
                }
            }

            DrawHeader(gfx, newHeaderList);
            return;
        }

        int index = 0;
        int headerStartPosition = -1;
        Header lastHeader = null;

        foreach (Header header in headerList)
        {
            int startPos = barStartLeft + (index * widthPerItem);
            bool showDateHeader = false;

            header.StartLocation = startPos;

            // Checks whether to show the date or not

            if (lastHeader == null)
            {
                showDateHeader = true;
            }
            else if (header.Time.Hour < lastHeader.Time.Hour)
            {
                showDateHeader = true;
            }
            else if (header.Time.Minute == lastHeader.Time.Minute)
            {
                showDateHeader = true;
            }

            // Show date

            if (showDateHeader == true)
            {
                string str = "";

                if (header.HeaderTextInsteadOfTime.Length > 0)
                {
                    str = header.HeaderTextInsteadOfTime;
                }
                else
                {
                    str = header.Time.ToString("d-MMM");
                }
                gfx.DrawString(str, dateTextFont, Brushes.Black, startPos, 0);
            }

            // Show time

            gfx.DrawString(header.HeaderText, timeTextFont, Brushes.Black, startPos, headerTimeStartTop);
            index += 1;

            lastHeader = header;
        }

        shownHeaderList = headerList;
        widthPerItem = (this.Width - 10 - barStartLeft - barStartRight) / shownHeaderList.Count;
    }

    /// <summary>
    /// Draws the bars
    /// </summary>
    /// <param name="grfx"></param>
    /// <remarks></remarks>

    private void DrawBars(Graphics grfx, bool ignoreScrollAndMousePosition = false)
    {
        if (shownHeaderList == null)
            return;
        if (shownHeaderList.Count <= 1)
            return;

        int index = 0;

        // Finds pixels per minute

        TimeSpan timeBetween = shownHeaderList[1].Time - shownHeaderList[0].Time;
        int minutesBetween = Convert.ToInt32(timeBetween.TotalMinutes);
        //(timeBetween.Days * 1440) + (timeBetween.Hours * 60) + timeBetween.Minutes
        dynamic widthBetween = (shownHeaderList[1].StartLocation - shownHeaderList[0].StartLocation);
        // this line was:
        // decimal perMinute = ...
        double perMinute = (double)widthBetween / (double)minutesBetween;

        // Draws each bar

        foreach (ChartBarDate bar in bars)
        {
            index = bar.RowIndex;

            int startLocation = 0;
            int width = 0;
            int startMinutes = 0;
            // Number of minutes from start of the gantt chart
            TimeSpan startTimeSpan = default(TimeSpan);
            int lengthMinutes = 0;
            // Number of minutes from bar start to bar end
            TimeSpan lengthTimeSpan = default(TimeSpan);

            int scrollPos = 0;

            if (ignoreScrollAndMousePosition == false)
            {
                scrollPos = scrollPosition;
            }

            // Calculates where the bar should be located

            startTimeSpan = bar.StartValue - FromDate;
            startMinutes = (startTimeSpan.Days * 1440) + (startTimeSpan.Hours * 60) + startTimeSpan.Minutes;

            startLocation = Convert.ToInt32(perMinute * startMinutes);

            System.DateTime endValue = bar.EndValue;

            if (endValue == null)
            {
                endValue = System.DateTime.Now;
            }

            lengthTimeSpan = endValue - bar.StartValue;
            lengthMinutes = (lengthTimeSpan.Days * 1440) + (lengthTimeSpan.Hours * 60) + lengthTimeSpan.Minutes;

            width = Convert.ToInt32(perMinute * lengthMinutes);

            int a = barStartLeft + startLocation;
            int b = barStartTop + (barHeight * (index - scrollPos)) + (barSpace * (index - scrollPos)) + 2;
            int c = width;
            int d = barHeight;

            if (c == 0)
                c = 1;

            // Stops a bar from going into the row-text area

            if (a - barStartLeft < 0)
            {
                a = barStartLeft;
            }

            System.Drawing.Color color = default(System.Drawing.Color);

            // If mouse is over bar, set the color to be hovercolor

            if (MouseOverRowText == bar.Text & bar.StartValue <= _mouseOverColumnValue & bar.EndValue >= _mouseOverColumnValue)
            {
                color = bar.HoverColor;
            }
            else
            {
                color = bar.Color;
            }

            // Set the location for the graphics

            bar.TopLocation.Left = new Point(a, b);
            bar.TopLocation.Right = new Point(a + c, b);
            bar.BottomLocation.Left = new Point(a, b + d);
            bar.BottomLocation.Right = new Point(a, b + d);

            LinearGradientBrush obBrush = null;
            Rectangle obRect = new Rectangle(a, b, c, d);

            if (bar.StartValue != null & endValue != null)
            {

                if ((index >= scrollPos & index < barsViewable + scrollPos) | ignoreScrollAndMousePosition == true)
                {
                    // Makes the bar gradient

                    obBrush = new LinearGradientBrush(obRect, color, Color.Gray, LinearGradientMode.Vertical);

                    // Draws the bar

                    grfx.DrawRectangle(Pens.Black, obRect);
                    grfx.FillRectangle(obBrush, obRect);

                    // Draws the rowtext

                    grfx.DrawString(bar.Text, rowTextFont, Brushes.Black, 0, barStartTop + (barHeight * (index - scrollPos)) + (barSpace * (index - scrollPos)));

                    obBrush = null;
                    //obRect = null;
                    //obRect.TryDispose();
                    obBrush = null;
                }
            }

            //color = null;
            //color.TryDispose();
        }
    }

    /// <summary>
    /// Draws the vertical lines
    /// </summary>
    /// <param name="grfx"></param>
    /// <remarks></remarks>

    public void DrawNetVertical(Graphics grfx)
    {
        if (shownHeaderList == null)
            return;
        if (shownHeaderList.Count == 0)
            return;

        int index = 0;
        int availableWidth = this.Width - 10 - barStartLeft - barStartRight;
        Header lastHeader = null;

        foreach (Header header in shownHeaderList)
        {
            int headerLocationY = 0;

            if (lastHeader == null)
            {
                headerLocationY = 0;
            }
            else if (header.Time.Hour < lastHeader.Time.Hour)
            {
                headerLocationY = 0;
            }
            else
            {
                headerLocationY = headerTimeStartTop;
            }

            grfx.DrawLine(Pens.Bisque, barStartLeft + (index * widthPerItem), headerLocationY, barStartLeft + (index * widthPerItem), lastLineStop);
            index += 1;

            lastHeader = header;
        }

        grfx.DrawLine(lineColor, barStartLeft + (index * widthPerItem), headerTimeStartTop, barStartLeft + (index * widthPerItem), lastLineStop);
    }

    /// <summary>
    /// Draws the horizontal lines
    /// </summary>
    /// <param name="grfx"></param>
    /// <remarks></remarks>

    public void DrawNetHorizontal(Graphics grfx)
    {
        if (shownHeaderList == null)
            return;
        if (shownHeaderList.Count == 0)
            return;

        int index = 0;
        int width = (widthPerItem * shownHeaderList.Count) + barStartLeft;

        // Last used index. Hopefully nobody will make a row named QQQ :o)
        for (index = 0; index <= GetIndexChartBar("QQQQQQ"); index++)
        {
            foreach (ChartBarDate bar in bars)
            {
                grfx.DrawLine(lineColor, 0, barStartTop + (barHeight * index) + (barSpace * index), width, barStartTop + (barHeight * index) + (barSpace * index));
            }
        }

        lastLineStop = barStartTop + (barHeight * (index - 1)) + (barSpace * (index - 1));
    }

    // This is the position (in pixels, from top) of the last line. Used for drawing lines


    private int lastLineStop = 0;
    #endregion

    #region "Header list"

    /// <summary>
    /// Gets the full header list, consisting of hours between the two dates set
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>

    private List<Header> GetFullHeaderList()
    {
        List<Header> result = new List<Header>();
        System.DateTime newFromTime = new System.DateTime(FromDate.Year, FromDate.Month, FromDate.Day);
        string item = null;

        TimeSpan interval = ToDate - FromDate;

        if (interval.TotalDays < 1)
        {
            var _with1 = newFromTime;
            newFromTime = _with1.AddHours(FromDate.Hour);

            if (headerFromDate.Minute < 59 & headerFromDate.Minute > 29)
            {
                newFromTime = _with1.AddMinutes(30);
            }
            else
            {
                newFromTime = _with1.AddMinutes(0);
            }

            while (newFromTime <= ToDate)
            {
                item = newFromTime.Hour + ":";

                if (newFromTime.Minute < 10)
                {
                    item += "0" + newFromTime.Minute;
                }
                else
                {
                    item += "" + newFromTime.Minute;
                }

                Header header = new Header();

                header.HeaderText = item;
                header.HeaderTextInsteadOfTime = "";
                header.Time = new System.DateTime(newFromTime.Year, newFromTime.Month, newFromTime.Day, newFromTime.Hour, newFromTime.Minute, 0);
                result.Add(header);

                newFromTime = newFromTime.AddMinutes(5);
                // The minimum interval of time between the headers
            }
        }
        else if (interval.TotalDays < 60)
        {
            while (newFromTime <= ToDate)
            {
                Header header = new Header();

                header.HeaderText = "";
                header.HeaderTextInsteadOfTime = "";
                header.Time = new System.DateTime(newFromTime.Year, newFromTime.Month, newFromTime.Day, 0, 0, 0);
                result.Add(header);

                newFromTime = newFromTime.AddDays(1);
                // The minimum interval of time between the headers
            }
        }
        else
        {
            while (newFromTime <= ToDate)
            {
                Header header = new Header();

                header.HeaderText = "";
                header.Time = new System.DateTime(newFromTime.Year, newFromTime.Month, newFromTime.Day, 0, 0, 0);
                header.HeaderTextInsteadOfTime = newFromTime.ToString("MMM");
                result.Add(header);

                newFromTime = newFromTime.AddMonths(1);
                // The minimum interval of time between the headers
            }
        }

        return result;
    }

    #endregion

    #region "Mouse Move"

    /// <summary>
    /// Finds the current row and column based on mouse position
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    private void GanttChart_MouseMove(System.Object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (shownHeaderList == null)
            return;
        if (shownHeaderList.Count == 0)
            return;

        if (e.Button != System.Windows.Forms.MouseButtons.Left)
        {
            mouseHoverPart = MouseOverPart.Empty;

            // If bar has changed manually, but left mouse button is no longer pressed the BarChanged event will be raised

            if (AllowManualEditBar == true)
            {
                if (barIsChanging >= 0)
                {
                    if (BarChanged != null)
                    {
                        BarChanged(this, bars[barIsChanging].Value);
                    }
                    barIsChanging = -1;
                }
            }
        }

        mouseHoverBarIndex = -1;

        Point LocalMousePosition = default(Point);

        LocalMousePosition = this.PointToClient(Cursor.Position);

        // Finds pixels per minute

        TimeSpan timeBetween = shownHeaderList[1].Time - shownHeaderList[0].Time;
        int minutesBetween = (timeBetween.Days * 1440) + (timeBetween.Hours * 60) + timeBetween.Minutes;
        dynamic widthBetween = (shownHeaderList[1].StartLocation - shownHeaderList[0].StartLocation);
        decimal perMinute = widthBetween / minutesBetween;

        // Finds the time at mousepointer

        int minutesAtCursor = 0;

        if (LocalMousePosition.X > barStartLeft && perMinute > 0)
        {
            minutesAtCursor = Convert.ToInt32((LocalMousePosition.X - barStartLeft) / perMinute);
            _mouseOverColumnValue = FromDate.AddMinutes(minutesAtCursor);
        }
        else
        {
            //_mouseOverColumnValue.TryDispose();
        }

        // Finds the row at mousepointer

        string rowText = "";
        object rowValue = null;
        string columnText = "";

        // Tests to see if the mouse pointer is hovering above the scrollbar

        bool scrollBarStatusChanged = false;

        // Tests to see if the mouse is hovering over the scroll-area bottom-arrow

        if (LocalMousePosition.X > BottomPart.Left & LocalMousePosition.Y < BottomPart.Right & LocalMousePosition.Y < BottomPart.Bottom & LocalMousePosition.Y > BottomPart.Top)
        {
            if (mouseOverBottomPart == false)
            {
                scrollBarStatusChanged = true;
            }

            mouseOverBottomPart = true;
        }
        else
        {
            if (mouseOverBottomPart == false)
            {
                scrollBarStatusChanged = true;
            }

            mouseOverBottomPart = false;
        }

        // Tests to see if the mouse is hovering over the scroll-area top-arrow

        if (LocalMousePosition.X > topPart.Left & LocalMousePosition.Y < topPart.Right & LocalMousePosition.Y < topPart.Bottom & LocalMousePosition.Y > topPart.Top)
        {
            if (mouseOverTopPart == false)
            {
                scrollBarStatusChanged = true;
            }

            mouseOverTopPart = true;
        }
        else
        {
            if (mouseOverTopPart == false)
            {
                scrollBarStatusChanged = true;
            }

            mouseOverTopPart = false;
        }

        // Tests to see if the mouse is hovering over the scroll

        if (LocalMousePosition.X > scroll.Left & LocalMousePosition.Y < scroll.Right & LocalMousePosition.Y < scroll.Bottom & LocalMousePosition.Y > scroll.Top)
        {
            if (mouseOverScrollBar == false)
            {
                scrollBarStatusChanged = true;
            }

            mouseOverScrollBar = true;
            mouseOverScrollBarArea = true;
        }
        else
        {
            if (mouseOverScrollBar == false)
            {
                scrollBarStatusChanged = true;
            }

            mouseOverScrollBar = false;
            mouseOverScrollBarArea = false;
        }

        // If the mouse is not above the scroll, test if it's over the scroll area (no need to test if it's not above the scroll)

        if (mouseOverScrollBarArea == false)
        {
            if (LocalMousePosition.X > scrollBarArea.Left & LocalMousePosition.Y < scrollBarArea.Right & LocalMousePosition.Y < scrollBarArea.Bottom & LocalMousePosition.Y > scrollBarArea.Top)
            {
                mouseOverScrollBarArea = true;
            }
        }


        // Tests to see if the mouse pointer is hovering above a bar

        int index = 0;


        foreach (ChartBarDate bar in bars)
        {
            // If the bar is set to be hidden from mouse move, the current bar will be ignored

            if (bar.HideFromMouseMove == false)
            {
                if (bar.EndValue == null)
                {
                    bar.EndValue = System.DateTime.Now;
                }

                // Mouse pointer needs to be inside the X and Y positions of the bar

                if (LocalMousePosition.Y > bar.TopLocation.Left.Y & LocalMousePosition.Y < bar.BottomLocation.Left.Y)
                {

                    if (LocalMousePosition.X > bar.TopLocation.Left.X & LocalMousePosition.X < bar.TopLocation.Right.X)
                    {
                        // If the current bar is the one where the mouse is above, the rowText and rowValue needs to be set correctly

                        rowText = bar.Text;
                        rowValue = bar.Value;
                        mouseHoverBarIndex = index;

                        if (mouseHoverPart != MouseOverPart.BarLeftSide & mouseHoverPart != MouseOverPart.BarRightSide)
                        {
                            mouseHoverPart = MouseOverPart.Bar;
                        }
                    }

                    // If mouse pointer is near the edges of the bar it will open up for editing the bar

                    if (AllowManualEditBar == true)
                    {
                        int areaSize = 5;

                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            areaSize = 50;
                        }

                        if (LocalMousePosition.X > bar.TopLocation.Left.X - areaSize & LocalMousePosition.X < bar.TopLocation.Left.X + areaSize & mouseHoverPart != MouseOverPart.BarRightSide)
                        {
                            this.Cursor = Cursors.VSplit;
                            mouseHoverPart = MouseOverPart.BarLeftSide;
                            mouseHoverBarIndex = index;
                        }
                        else if (LocalMousePosition.X > bar.TopLocation.Right.X - areaSize & LocalMousePosition.X < bar.TopLocation.Right.X + areaSize & mouseHoverPart != MouseOverPart.BarLeftSide)
                        {
                            this.Cursor = Cursors.VSplit;
                            mouseHoverPart = MouseOverPart.BarRightSide;
                            mouseHoverBarIndex = index;
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
            }

            index += 1;
        }

        // Sets the mouseover row value and text

        _mouseOverRowText = rowText;
        _mouseOverRowValue = rowValue;

        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            if (MouseDragged != null)
            {
                MouseDragged(sender, e);
            }

        }
        else
        {
            // A simple test to see if the mousemovement has caused any changes to how it should be displayed
            // It only redraws if mouse moves from a bar to blank area or from blank area to a bar
            // This increases performance compared to having a redraw every time a mouse moves

            if ((_mouseOverRowValue == null & (rowValue != null)) | ((_mouseOverRowValue != null) & rowValue == null) | scrollBarStatusChanged == true)
            {
                PaintChart();
            }
        }
    }

    /// <summary>
    /// Mouse leave event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    private void GanttChart_MouseLeave(System.Object sender, System.EventArgs e)
    {
        _mouseOverRowText = null;
        _mouseOverRowValue = null;
        mouseHoverPart = MouseOverPart.Empty;

        PaintChart();
    }

    /// <summary>
    /// Mouse drag event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    public void GanttChart_MouseDragged(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (mouseOverScrollBarArea == true)
        {
            ScrollPositionY = e.Location.Y;
        }

        if (AllowManualEditBar == true)
        {
            if (mouseHoverBarIndex > -1)
            {
                if (mouseHoverPart == MouseOverPart.BarLeftSide)
                {
                    barIsChanging = mouseHoverBarIndex;
                    bars[mouseHoverBarIndex].StartValue = _mouseOverColumnValue;
                    PaintChart();
                }
                else if (mouseHoverPart == MouseOverPart.BarRightSide)
                {
                    barIsChanging = mouseHoverBarIndex;
                    bars[mouseHoverBarIndex].EndValue = _mouseOverColumnValue;
                    PaintChart();
                }
            }
        }
    }


    #endregion

    #region "ToolTipText"

    private List<string> _toolTipText = new List<string>();

    private string _toolTipTextTitle = "";

    private Point MyPoint = new Point(0, 0);
    /// <summary>
    /// The title to draw
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    public string ToolTipTextTitle
    {
        get { return _toolTipTextTitle; }
        set { _toolTipTextTitle = value; }
    }

    /// <summary>
    /// Gets or sets the ToolTipText lines
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks>Don not use the add function directly on this, use ToolTipText = value</remarks>

    public List<string> ToolTipText
    {
        get
        {
            if (_toolTipText == null)
                _toolTipText = new List<string>();
            return _toolTipText;
        }
        set
        {
            _toolTipText = value;

            Point LocalMousePosition = default(Point);

            LocalMousePosition = this.PointToClient(Cursor.Position);


            if (LocalMousePosition == MyPoint)
                return;

            MyPoint = LocalMousePosition;

            ToolTip.SetToolTip(this, ".");
        }
    }

    /// <summary>
    /// Draws the ToolTip window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    private void ToolTipText_Draw(System.Object sender, System.Windows.Forms.DrawToolTipEventArgs e)
    {
        if (ToolTipText == null)
        {
            ToolTipText = new List<string>();
            return;
        }

        if (ToolTipText.Count == 0)
        {
            return;
        }
        else if (ToolTipText[0].Length == 0)
        {
            return;
        }

        int x = 0;
        int y = 0;

        e.Graphics.FillRectangle(Brushes.AntiqueWhite, e.Bounds);
        e.DrawBorder();

        int titleHeight = 14;
        int fontHeight = 12;

        // Draws the line just below the title

        e.Graphics.DrawLine(Pens.Black, 0, titleHeight, e.Bounds.Width, titleHeight);

        int lines = 1;
        string text = ToolTipTextTitle;

        // Draws the title

        using (Font font = new Font(e.Font, FontStyle.Bold))
        {
            x = Convert.ToInt32((e.Bounds.Width - e.Graphics.MeasureString(text, font).Width) / 2);
            y = Convert.ToInt32((titleHeight - e.Graphics.MeasureString(text, font).Height) / 2);
            e.Graphics.DrawString(text, font, Brushes.Black, x, y);
        }

        // Draws the lines
        for (int i = 0; i < ToolTipText.Count; i++)
        {
            Font font = new Font(e.Font, FontStyle.Regular);

            if (ToolTipText[i].Contains("[b]"))
            {
                font = new Font(font.FontFamily, font.Size, FontStyle.Bold, font.Unit);
                ToolTipText[i] = ToolTipText[i].Replace("[b]", "");
            }

            using (font)
            {
                x = 5;
                y = Convert.ToInt32((titleHeight - fontHeight - e.Graphics.MeasureString(ToolTipText[i], font).Height) / 2 + 10 + (lines * 14));
                e.Graphics.DrawString(ToolTipText[i], font, Brushes.Black, x, y);
            }

            lines += 1;
        }
        //foreach (string str in ToolTipText)
        //{
        //    Font font = new Font(e.Font, FontStyle.Regular);

        //    if (str.Contains("[b]"))
        //    {
        //        font = new Font(font.FontFamily, font.Size, FontStyle.Bold, font.Unit);
        //        str = str.Replace("[b]", "");
        //    }

        //    using (font)
        //    {
        //        x = 5;
        //        y = Convert.ToInt32((titleHeight - fontHeight - e.Graphics.MeasureString(str, font).Height) / 2 + 10 + (lines * 14));
        //        e.Graphics.DrawString(str, font, Brushes.Black, x, y);
        //    }

        //    lines += 1;
        //}
    }

    /// <summary>
    /// Automatically resizes the ToolTip window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    private void ToolTipText_Popup(System.Object sender, System.Windows.Forms.PopupEventArgs e)
    {
        if (ToolTipText == null)
        {
            ToolTipText = new List<string>();
        }

        if (ToolTipText.Count == 0)
        {
            e.ToolTipSize = new Size(0, 0);
            return;
        }
        else if (ToolTipText[0].Length == 0)
        {
            e.ToolTipSize = new Size(0, 0);
            return;
        }

        // resizes the ToolTip window

        int height = 18 + (ToolTipText.Count * 15);
        e.ToolTipSize = new Size(200, height);
    }

    #endregion

    #region "ChartBar"

    private class ChartBarDate
    {

        internal class Location
        {

            private Point _right = new Point(0, 0);

            private Point _left = new Point(0, 0);
            public Point Right
            {
                get { return _right; }
                set { _right = value; }
            }

            public Point Left
            {
                get { return _left; }
                set { _left = value; }
            }

        }

        private System.DateTime _startValue;

        private System.DateTime _endValue;
        private Color _color;

        private Color _hoverColor;
        private string _text;

        private object _value;

        private int _rowIndex;
        private Location _topLocation = new Location();

        private Location _bottomLocation = new Location();

        private bool _hideFromMouseMove = false;
        public System.DateTime StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        public System.DateTime EndValue
        {
            get { return _endValue; }
            set { _endValue = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        public bool HideFromMouseMove
        {
            get { return _hideFromMouseMove; }
            set { _hideFromMouseMove = value; }
        }

        internal Location TopLocation
        {
            get { return _topLocation; }
            set { _topLocation = value; }
        }

        internal Location BottomLocation
        {
            get { return _bottomLocation; }
            set { _bottomLocation = value; }
        }

    }

    #endregion

    #region "Headers"

    private class Header
    {

        private string _headerText;
        private int _startLocation;
        private string _headerTextInsteadOfTime = "";

        private System.DateTime _time;
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        public int StartLocation
        {
            get { return _startLocation; }
            set { _startLocation = value; }
        }

        /// <summary>
        /// If this string is larger than 0, this will be used instead of Time
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>

        public string HeaderTextInsteadOfTime
        {
            get { return _headerTextInsteadOfTime; }
            set { _headerTextInsteadOfTime = value; }
        }

        /// <summary>
        /// Time to display
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>

        public System.DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

    }

    #endregion

    #region "Resize"

    /// <summary>
    /// On resize the Gantt Chart is redrawn
    /// </summary>
    /// <param name="e"></param>
    /// <remarks></remarks>

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);

        scrollPosition = 0;

        // Used for when the Gantt Chart is saved as an image

        if (lastLineStop > 0)
        {
            objBmp = new Bitmap(this.Width - barStartRight, lastLineStop, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            objGraphics = Graphics.FromImage(objBmp);
        }

        PaintChart();
    }

    #endregion

    #region "Scrollbar"

    private int barsViewable = -1;
    private int scrollPosition = 0;
    private Rectangle topPart;
    private Rectangle BottomPart;
    private Rectangle scroll;

    private Rectangle scrollBarArea;
    private bool mouseOverTopPart = false;
    private bool mouseOverBottomPart = false;
    private bool mouseOverScrollBar = false;

    private bool mouseOverScrollBarArea = false;
    /// <summary>
    /// Draws a scrollbar to the component, if there's a need for it
    /// </summary>
    /// <param name="grfx"></param>
    /// <remarks></remarks>

    private void DrawScrollBar(Graphics grfx)
    {
        barsViewable = (this.Height - barStartTop) / (barHeight + barSpace);
        int barCount = GetIndexChartBar("QQQWWW");
        if (barCount == 0)
            return;

        int maxHeight = this.Height - 30;
        decimal scrollHeight = (maxHeight / barCount) * barsViewable;

        // If the scroll area is filled there's no need to show the scrollbar

        if (scrollHeight >= maxHeight)
            return;
        
        decimal scrollSpeed = 1.0M;
        if (barCount - barsViewable > 0)
        {
            scrollSpeed = (maxHeight - scrollHeight) / (barCount - barsViewable);
        }

        scrollBarArea = new Rectangle(this.Width - 20, 19, 12, maxHeight);
        scroll = new Rectangle(this.Width - 20, 19 + Convert.ToInt32((scrollPosition * scrollSpeed)), 12, Convert.ToInt32(scrollHeight));

        topPart = new Rectangle(this.Width - 20, 10, 12, 8);
        BottomPart = new Rectangle(this.Width - 20, this.Height - 10, 12, 8);

        Brush colorTopPart = null;
        Brush colorBottomPart = null;
        Brush colorScroll = null;

        if (mouseOverTopPart == true)
        {
            colorTopPart = Brushes.Black;
        }
        else
        {
            colorTopPart = Brushes.Gray;
        }

        if (mouseOverBottomPart == true)
        {
            colorBottomPart = Brushes.Black;
        }
        else
        {
            colorBottomPart = Brushes.Gray;
        }

        if (mouseOverScrollBar == true)
        {
            colorScroll = new LinearGradientBrush(scroll, Color.Bisque, Color.Gray, LinearGradientMode.Horizontal);
        }
        else
        {
            colorScroll = new LinearGradientBrush(scroll, Color.White, Color.Gray, LinearGradientMode.Horizontal);
        }

        // Draws the top and bottom part of the scrollbar

        grfx.DrawRectangle(Pens.Black, topPart);
        grfx.FillRectangle(Brushes.LightGray, topPart);

        grfx.DrawRectangle(Pens.Black, BottomPart);
        grfx.FillRectangle(Brushes.LightGray, BottomPart);

        // Draws arrows

        PointF[] points = new PointF[3];
        points[0] = new PointF(topPart.Left, topPart.Bottom - 1);
        points[1] = new PointF(topPart.Right, topPart.Bottom - 1);
        points[2] = new PointF((topPart.Left + topPart.Right) / 2, topPart.Top + 1);

        grfx.FillPolygon(colorTopPart, points);

        points[0] = new PointF(BottomPart.Left, BottomPart.Top + 1);
        points[1] = new PointF(BottomPart.Right, BottomPart.Top + 1);
        points[2] = new PointF((BottomPart.Left + BottomPart.Right) / 2, BottomPart.Bottom - 1);

        grfx.FillPolygon(colorBottomPart, points);

        // Draws the scroll area

        grfx.DrawRectangle(Pens.Black, scrollBarArea);
        grfx.FillRectangle(Brushes.DarkGray, scrollBarArea);

        // Draws the actual scrollbar

        grfx.DrawRectangle(Pens.Black, scroll);
        grfx.FillRectangle(colorScroll, scroll);
    }

    /// <summary>
    /// The Y-position of the center of the scroll
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>

    private int ScrollPositionY
    {
        get
        {
            if (scroll == null)
                return -1;
            return ((scroll.Height / 2) + scroll.Location.Y) + 19;
        }
        set
        {
            int barCount = GetIndexChartBar("QQQWWW");
            int maxHeight = this.Height - 30;
            decimal scrollHeight = (maxHeight / barCount) * barsViewable;
            decimal scrollSpeed = (maxHeight - scrollHeight) / (barCount - barsViewable);
            int index = 0;
            dynamic distanceFromLastPosition = 9999;

            // Tests to see what scrollposition is the closest to the set position

            while (index < barCount)
            {
                int newPositionTemp = Convert.ToInt32((index * scrollSpeed) + (scrollHeight / 2) + (30 / 2));
                dynamic distanceFromCurrentPosition = newPositionTemp - value;

                if (distanceFromLastPosition < 0)
                {
                    if (distanceFromCurrentPosition < distanceFromLastPosition)
                    {
                        scrollPosition = index - 1;
                        PaintChart();
                        return;
                    }
                }
                else
                {
                    if (distanceFromCurrentPosition > distanceFromLastPosition)
                    {
                        scrollPosition = index - 1;

                        // A precaution to make sure the scroll bar doesn't go too far down

                        if (scrollPosition + barsViewable > GetIndexChartBar("QQQWWW"))
                        {
                            scrollPosition = GetIndexChartBar("QQQWWW") - barsViewable;
                        }

                        PaintChart();
                        return;
                    }
                }

                distanceFromLastPosition = distanceFromCurrentPosition;

                index += 1;
            }
        }
    }

    /// <summary>
    /// Scrolls one row up
    /// </summary>
    /// <remarks></remarks>

    public void ScrollOneup()
    {
        if (scrollPosition == 0)
            return;

        scrollPosition -= 1;

        PaintChart();
    }

    /// <summary>
    /// Scrolls one row down
    /// </summary>
    /// <remarks></remarks>

    public void ScrollOneDown()
    {
        if (scrollPosition + barsViewable >= GetIndexChartBar("QQQWWW"))
            return;

        scrollPosition += 1;

        PaintChart();
    }

    /// <summary>
    /// If the user clicks on the scrollbar, scrolling functions will be called
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    private void GanttChart_Click(System.Object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            if (mouseOverBottomPart == true)
            {
                ScrollOneDown();
            }
            else if (mouseOverTopPart == true)
            {
                ScrollOneup();
            }
        }
    }

    /// <summary>
    /// When mousewheel is used, the scrollbar will scroll
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>

    private void GanttChart_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (e.Delta > 0)
        {
            ScrollOneup();
        }
        else
        {
            ScrollOneDown();
        }
    }

    #endregion

    #region "Save"

    /// <summary>
    /// Saves the GanttChart to specified image file
    /// </summary>
    /// <param name="filePath"></param>
    /// <remarks></remarks>

    public void SaveImage(string filePath)
    {
        objGraphics.SmoothingMode = SmoothingMode.HighSpeed;
        objGraphics.Clear(this.BackColor);

        if (headerFromDate == null | headerToDate == null)
            return;

        DrawHeader(objGraphics, null);
        DrawNetHorizontal(objGraphics);
        DrawNetVertical(objGraphics);
        DrawBars(objGraphics, true);

        objBmp.Save(filePath);
    }

    #endregion

    private enum MouseOverPart
    {

        Empty,
        Bar,
        BarLeftSide,
        BarRightSide

    }

}

