using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Charts;
using DevExpress.Mvvm.UI.Interactivity;

namespace GenericChart
{
    public class PieChartRotationBehavior : Behavior<ChartControl> {
        ChartControl Chart { get { return AssociatedObject; } }
        PieSeries2D Series { get { return (PieSeries2D)Chart.Diagram.Series[0]; } }
        bool rotate;
        Point startPosition;

        protected override void OnAttached() {
            base.OnAttached();
            Chart.MouseDown += Chart_MouseDown;
            Chart.MouseMove += Chart_MouseMove;
            Chart.MouseUp += Chart_MouseUp;
        }

        void Chart_MouseDown(object sender, MouseButtonEventArgs e) {
            Point position = e.GetPosition(Chart);
            ChartHitInfo hitInfo = Chart.CalcHitInfo(position);
            if(hitInfo != null && hitInfo.SeriesPoint != null) {
                rotate = true;
                startPosition = position;
            }
        }
        void Chart_MouseMove(object sender, MouseEventArgs e) {
            var position = e.GetPosition(Chart);
            var hitInfo = Chart.CalcHitInfo(position);
            if(hitInfo != null && rotate) {
                var angleDelta = CalcAngle(startPosition, position);
                angleDelta *= Series.SweepDirection == PieSweepDirection.Clockwise ? -1.0 : 1.0;
                var newAngle = Series.Rotation + angleDelta;
                if(Math.Abs(newAngle) > 360)
                    newAngle += -720 * Math.Sign(newAngle);
                Series.Rotation = newAngle;
                startPosition = position;
            }
        }
        void Chart_MouseUp(object sender, MouseButtonEventArgs e) {
            rotate = false;
        }
        double CalcAngle(Point p1, Point p2) {
            var center = new Point(Chart.Diagram.ActualWidth / 2, Chart.ActualHeight / 2);
            Vector startVector = p1 - center;
            Vector endVector = p2 - center;
            return Vector.AngleBetween(endVector, startVector);
        }
    }
}
