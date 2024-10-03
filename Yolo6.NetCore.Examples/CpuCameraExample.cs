using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenCvSharp;
using Yolo6.NetCore.Models;

namespace Yolo6.NetCore.Examples
{
    public static class CpuCameraExample
    {
        public static void Run()
        {
            var capture = new VideoCapture(0); // Open the default camera (index 0)
            using var window = new Window("Camera");
            using var image = new Mat();

            // Initialize YOLO model once outside the loop to avoid reloading the model on every frame
            using var yolo = new Yolo<YoloCocoModel>("Models/yolov6s.onnx");

            // Pre-create resources to avoid excessive memory allocation
            var font = new Font("Consolas", 16, GraphicsUnit.Pixel);
            var pen = new Pen(Color.Blue, 1);
            var brush = new SolidBrush(Color.White);

            while (true)
            {
                capture.Read(image);
                if (image.Empty())
                    break;

                // Convert Mat to Bitmap for processing (still needed for YOLO input)
                using var frame = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);

                // Predict objects using YOLO
                var predictions = yolo.Predict(frame);

                // Draw predictions on the frame
                using var graphics = Graphics.FromImage(frame);
                foreach (var prediction in predictions)
                {
                    var score = Math.Round(prediction.Score, 2);
                    graphics.DrawRectangles(pen, new[] { prediction.Rectangle });

                    var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);
                    graphics.DrawString($"{prediction.Label.Name} ({score})", font, brush, new PointF(x, y));
                }

                // Convert modified Bitmap back to OpenCvSharp.Mat for display
                var outputMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame);

                // Show the processed image in the window
                window.ShowImage(outputMat);
                Cv2.WaitKey(1); // Wait for 1 ms between frames
            }

            // Clean up resources
            font.Dispose();
            pen.Dispose();
            brush.Dispose();
        }
    }
}