using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Yolo6.NetCore.Models;

namespace Yolo6.NetCore.Examples
{
    public static class CpuImageExample
    {
        public static void Run()
        {
            using var image = Image.FromFile("input.jpg");
            using var yolo = new Yolo<YoloCocoModel>("Models/yolov6s.onnx");
            var predictions = yolo.Predict(image);

            using var graphics = Graphics.FromImage(image);
            foreach (var prediction in predictions)
            {
                var score = Math.Round(prediction.Score, 2);
                graphics.DrawRectangles(new Pen(Color.Blue, 2),
                    new[] { prediction.Rectangle });

                var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);

                graphics.DrawString($"{prediction.Label.Name} ({score})",
                    new Font("Consolas", 32, GraphicsUnit.Pixel), new SolidBrush(Color.White),
                    new PointF(x, y));
            }

            image.Save("output.jpg");
            Console.WriteLine("FrameExample complete.");
        }
    }
}
