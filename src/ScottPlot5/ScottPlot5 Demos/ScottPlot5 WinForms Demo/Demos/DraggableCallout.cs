﻿using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggableCallout : Form, IDemoWindow
{
    public string Title => "Draggable Callout";
    public string Description => "Demonstrates how to make a Callout mouse-interactive";

    Callout? CalloutBeingDragged = null;

    public DraggableCallout()
    {
        InitializeComponent();

        var fp = formsPlot1.Plot.Add.Function(SampleData.DunningKrugerCurve);
        fp.MinX = 0;
        fp.MaxX = 2;
        fp.LineWidth = 3;

        formsPlot1.Plot.YLabel("Confidence");
        formsPlot1.Plot.XLabel("Competence");
        formsPlot1.Plot.Title("Dunning-Kruger Effect", 24);

        formsPlot1.Plot.Axes.SetLimitsX(0, 2);
        formsPlot1.Plot.Axes.SetLimitsY(0, 1.2);

        var callout = formsPlot1.Plot.Add.Callout(
            text: "A draggable Callout\npoints to a coordinate",
            textX: 0.3,
            textY: 1.1,
            tipX: 0.2185,
            tipY: 0.8925);

        callout.Label.BorderColor = Colors.Blue;
        callout.Label.BackgroundColor = Colors.Blue.WithAlpha(.5);
        callout.Label.Padding = 5;

        callout.LineStyle = new LineStyle()
        {
            Color = callout.Label.BorderColor,
            Width = callout.Label.BorderWidth,
        };

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (CalloutBeingDragged is null)
        {
            Callout? calloutUnderMouse = GetCalloutUnderMouse(e.X, e.Y);
            Cursor = calloutUnderMouse is null ? Cursors.Arrow : Cursors.Hand;
        }
        else
        {
            CalloutBeingDragged.Move(e.X, e.Y);
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        CalloutBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        Callout? calloutUnderMouse = GetCalloutUnderMouse(e.X, e.Y);
        if (calloutUnderMouse is null)
            return;

        CalloutBeingDragged = calloutUnderMouse;
        CalloutBeingDragged.StartMove(e.X, e.Y);
        formsPlot1.Interaction.Disable();
    }

    private Callout? GetCalloutUnderMouse(float x, float y)
    {
        return formsPlot1.Plot
            .GetPlottables<Callout>()
            .Where(p => p.IsUnderMouse(x, y))
            .FirstOrDefault();
    }
}
