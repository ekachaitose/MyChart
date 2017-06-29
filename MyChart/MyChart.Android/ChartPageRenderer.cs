using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

using Xamarin.Forms.Platform.Android;
using MyChart.Droid;
using MyChart;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;
using Java.Lang;

[assembly: ExportRenderer(typeof(ChartPage), typeof(ChartPageRenderer))]
namespace MyChart.Droid
{
    public class ChartPageRenderer : PageRenderer
    {
        LineChart mLineChart;
        BarChart mBarChart;
        Android.Views.View view;
        public ChartPageRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            var activity = Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.activity_chart, this, false);
            mLineChart = view.FindViewById<LineChart>(Resource.Id.mChart);
            drawLineChart();
            AddView(view);
        }
        public void drawLineChart()
        {
            //ปรับแต่งเส้นและข้อมูลที่แสดงใน chart UI
            mLineChart.Description.Text = "CodeMobiles IoT";
            mLineChart.AxisLeft.SetDrawLabels(true);
            mLineChart.AxisRight.Enabled = false;
            mLineChart.AxisLeft.GridColor = Android.Graphics.Color.ParseColor("#22000000");
            mLineChart.AxisLeft.AxisLineColor = Android.Graphics.Color.ParseColor("#22000000");
            mLineChart.AxisLeft.TextColor = Android.Graphics.Color.ParseColor("#78000000");


            mLineChart.AxisRight.SetDrawLabels(false);
            mLineChart.XAxis.SetDrawLabels(true);
            mLineChart.XAxis.AxisLineColor = Android.Graphics.Color.ParseColor("#22000000");
            mLineChart.XAxis.GridColor = Android.Graphics.Color.ParseColor("#22000000");
            mLineChart.AxisRight.TextColor = Android.Graphics.Color.ParseColor("#78000000");
            mLineChart.XAxis.Position = MikePhil.Charting.Components.XAxis.XAxisPosition.Bottom;

            //สร้าง entry
            IList<MikePhil.Charting.Data.Entry> yVals = new List<MikePhil.Charting.Data.Entry>();
            yVals.Add(new MikePhil.Charting.Data.Entry(0, 20));//(แกน x, แกนy)
            yVals.Add(new MikePhil.Charting.Data.Entry(1, 4));
            yVals.Add(new MikePhil.Charting.Data.Entry(2, 6));
            yVals.Add(new MikePhil.Charting.Data.Entry(3, 3));
            yVals.Add(new MikePhil.Charting.Data.Entry(4, 12));
            yVals.Add(new MikePhil.Charting.Data.Entry(5, 16));


            //ยัด entry ลง dataset
            LineDataSet dataSet = new LineDataSet(yVals, "CodeMobile Training");
            dataSet.SetCircleColor(Android.Graphics.Color.Green);
            dataSet.SetDrawCircleHole(true);
            dataSet.CircleRadius = 8;
            dataSet.CircleHoleRadius = 5;
            dataSet.LineWidth = 5;
            dataSet.SetMode(LineDataSet.Mode.Linear);
            dataSet.FillAlpha = 70; // 70%
            dataSet.SetDrawFilled(true);
            dataSet.FillColor = Android.Graphics.Color.ParseColor("#00FFFF");
            dataSet.Color = Android.Graphics.Color.ParseColor("#FF0000");

            //นำข้อมูลในdataset มาพล็อตกราฟ
            LineData data = new LineData(dataSet);
            data.SetValueFormatter(new YourValueFormatter());
        
            mLineChart.Data = data;
        }

        public void drawBarChart()
        {
            mBarChart.Description.Text = "CodeMobiles Outsource";
            mBarChart.AxisLeft.SetDrawLabels(true);
            mBarChart.AxisLeft.SetDrawAxisLine(false);
            mBarChart.AxisLeft.SetDrawGridLines(false);
            mBarChart.AxisLeft.SetDrawZeroLine(true);

            mBarChart.AxisRight.Enabled = false;
            mBarChart.AxisRight.SetDrawLabels(false);

            mBarChart.XAxis.SetDrawLabels(true);
            mBarChart.XAxis.Position = MikePhil.Charting.Components.XAxis.XAxisPosition.Bottom;
            mBarChart.XAxis.Enabled = false; // to remove all grid line


            //[20.0, 4.0, 6.0, 3.0, 12.0, 16.0]
            IList<MikePhil.Charting.Data.BarEntry> yVals = new List<MikePhil.Charting.Data.BarEntry>();
            yVals.Add(new MikePhil.Charting.Data.BarEntry(0, 20));
            yVals.Add(new MikePhil.Charting.Data.BarEntry(1, 4));
            yVals.Add(new MikePhil.Charting.Data.BarEntry(2, 6));
            yVals.Add(new MikePhil.Charting.Data.BarEntry(3, 3));
            yVals.Add(new MikePhil.Charting.Data.BarEntry(4, 12));
            yVals.Add(new MikePhil.Charting.Data.BarEntry(5, 16));

            BarDataSet dataSet = new BarDataSet(yVals, "CodeMobile IoT");


            dataSet.Color = Android.Graphics.Color.ParseColor("#FF0000");
            dataSet.Colors = ToJavaListInt(ColorTemplate.JoyfulColors);
            BarData data = new BarData(dataSet);
            data.SetValueFormatter(new YourValueFormatter());

            mBarChart.Data = data;
        }

        IList<Integer> ToJavaListInt(IList<int> oldValue)
        {
            var newValue = new List<Java.Lang.Integer>();
            foreach (var item in oldValue)
            {
                newValue.Add(new Java.Lang.Integer(item));
            }

            return newValue;
        }

        public class YourValueFormatter : Java.Lang.Object, IValueFormatter
        {
            public YourValueFormatter()
            {
            }

            public string GetFormattedValue(float value, MikePhil.Charting.Data.Entry entry, int dataSetIndex, ViewPortHandler viewPortHandler)
            {
                return Java.Lang.Math.Round(value) + ":" + entry.GetX().ToString();
            }
        }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            base.OnLayout(changed, l, t, r, b);
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }
    }
}