using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Nitrogen.Controls.EngineTachometer
{
    public class TachometerControl : Control
    {
        // Перо для стрелки (создаем ОДИН раз)
        private readonly Pen _arrowPen;

        // Перо для шкалы (создаем ОДИН раз)
        private readonly Pen _scalePen;

        // Серебристая кисть для центральной заглушки
        private readonly Brush _hubBrush = new SolidColorBrush(Colors.Silver);

        // Жирная белая риска для основных делений
        private readonly Pen _majorTickPen = new Pen(Brushes.White, 3);

        // Тонкая серая риска для промежуточных (потом пригодится)
        private readonly Pen _minorTickPen = new Pen(Brushes.White, 2);

        private readonly Brush _redZoneBrush = new SolidColorBrush(Colors.Red, 0.8);

        // Поле для хранения текущих оборотов
        private double _currentRPM;

        // Свойство, которое мы будем менять извне
        public double CurrentRPM
        {
            get => _currentRPM;
            set
            {
                // Если значение не изменилось — не перерисовываем (экономия ресурсов)
                if (_currentRPM == value) return;

                _currentRPM = value;

                // Самое важное: говорим Авалонии "Эй, данные изменились, перерисуй меня!"
                InvalidateVisual();
            }
        }
        public TachometerControl()
        {
            // Красная стрелка, потолще, с круглым кончиком
            _arrowPen = new Pen(Brushes.Red, 4)
            {
                LineCap = PenLineCap.Round
            };

            // Белая обводка для круга
            _scalePen = new Pen(Brushes.White, 2);
        }
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (Bounds.Width <= 0 || Bounds.Height <= 0) return;
            var center = new Point(Bounds.Width / 2, Bounds.Height / 2);

            const double MinRPM = 0;
            const double MaxRPM = 6000;
            const double StartAngle = -135.0;
            const double EndAngle = 135.0;

            double totalAngle = EndAngle - StartAngle;
            double radius = Math.Min(Bounds.Width, Bounds.Height) / 2 * 0.9;

            // 1. Внешний круг
            context.DrawEllipse(null, _scalePen, center, radius, radius);

            // 2. КРАСНАЯ ЗОНА (4000-6000 RPM) — через аппроксимацию дуги
            const double RedZoneStart = 4000;
            const double RedZoneEnd = 6000;

            double startDeg = StartAngle + (totalAngle * ((RedZoneStart - MinRPM) / (MaxRPM - MinRPM)));
            double endDeg = StartAngle + (totalAngle * ((RedZoneEnd - MinRPM) / (MaxRPM - MinRPM)));

            double startRad = (startDeg - 90.0) * (Math.PI / 180.0);
            double endRad = (endDeg - 90.0) * (Math.PI / 180.0);

            // Рисуем дугу через ломаную (надёжнее, чем ArcTo)
            var arcGeometry = new StreamGeometry();
            using (var ctx = arcGeometry.Open())
            {
                ctx.BeginFigure(new Point(center.X + radius * Math.Cos(startRad),
                                          center.Y + radius * Math.Sin(startRad)), false);

                int segments = 20;
                for (int i = 1; i <= segments; i++)
                {
                    double t = (double)i / segments;
                    double currentDeg = startDeg + (endDeg - startDeg) * t;
                    double currentRad = (currentDeg - 90.0) * (Math.PI / 180.0);

                    ctx.LineTo(new Point(center.X + radius * Math.Cos(currentRad),
                                         center.Y + radius * Math.Sin(currentRad)));
                }
            }

            // Рисуем толстой линией = имитация сектора
            var thickPen = new Pen(_redZoneBrush, radius * 0.05);
            context.DrawGeometry(null, thickPen, arcGeometry);

            // 3. РИСКИ И ТЕКСТ
            int totalTicks = 12;

            for (int i = 0; i <= totalTicks; i++)
            {
                bool isMajor = (i % 2 == 0);
                double tickLength = isMajor ? 10 : 5;
                Pen tickPen = isMajor ? _majorTickPen : _minorTickPen;

                double currentAngleDeg = StartAngle + (totalAngle * (i / (double)totalTicks));
                double currentAngleRad = (currentAngleDeg - 90.0) * (Math.PI / 180.0);

                double outerX = center.X + radius * Math.Cos(currentAngleRad);
                double outerY = center.Y + radius * Math.Sin(currentAngleRad);
                double innerRadius = radius - tickLength;
                double innerX = center.X + innerRadius * Math.Cos(currentAngleRad);
                double innerY = center.Y + innerRadius * Math.Sin(currentAngleRad);

                context.DrawLine(tickPen, new Point(outerX, outerY), new Point(innerX, innerY));

                if (isMajor)
                {
                    double textRadius = radius - 35;
                    double textX = center.X + textRadius * Math.Cos(currentAngleRad);
                    double textY = center.Y + textRadius * Math.Sin(currentAngleRad);

                    var formattedText = new FormattedText(
                        (i / 2).ToString(),
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        16,
                        Brushes.White);

                    formattedText.TextAlignment = TextAlignment.Center;
                    double offsetY = -formattedText.Height / 2;

                    double offsetX = -5;

                    using (context.PushTransform(Matrix.CreateTranslation(textX + offsetX, textY + offsetY)))
                    {
                        context.DrawText(formattedText, new Point(0, 0));
                    }
                }
            }

            // 4. СТРЕЛКА
            double clampedRPM = Math.Clamp(CurrentRPM, MinRPM, MaxRPM);
            double percent = (clampedRPM - MinRPM) / (MaxRPM - MinRPM);
            double angleDeg = StartAngle + percent * totalAngle;
            double angleRad = (angleDeg - 90.0) * (Math.PI / 180.0);

            double needleWidth = 8.0;
            double needleLength = radius * 0.85;

            double tipX = center.X + needleLength * Math.Cos(angleRad);
            double tipY = center.Y + needleLength * Math.Sin(angleRad);

            double perpAngle = angleRad + Math.PI / 2;
            double halfWidth = needleWidth / 2;

            double baseX1 = center.X + halfWidth * Math.Cos(perpAngle);
            double baseY1 = center.Y + halfWidth * Math.Sin(perpAngle);
            double baseX2 = center.X - halfWidth * Math.Cos(perpAngle);
            double baseY2 = center.Y - halfWidth * Math.Sin(perpAngle);

            var arrowGeometry = new StreamGeometry();
            using (var ctx = arrowGeometry.Open())
            {
                ctx.BeginFigure(new Point(baseX1, baseY1), true);
                ctx.LineTo(new Point(baseX2, baseY2));
                ctx.LineTo(new Point(tipX, tipY));
                ctx.EndFigure(true);
            }

            context.DrawGeometry(Brushes.Red, null, arrowGeometry);

            // 5. СТУПИЦА
            context.DrawEllipse(_hubBrush, null, center, 15, 15);
            context.DrawEllipse(null, new Pen(Brushes.Gray, 1), center, 15, 15);
        }

    }
}
