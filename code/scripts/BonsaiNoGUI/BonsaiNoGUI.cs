using System;
using System.Reactive.Linq;
using Bonsai.Arduino;
using Bonsai.Dsp;
using Bonsai.Vision;
using OpenCV.Net;

namespace BonsaiNoGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // declare bonsai workflow elements
            CameraCapture cameraCapture = new() { Index = 0 };
            Crop crop = new() { RegionOfInterest = new Rect(222,34,228,308) };
            Sum sum = new();

            // compute sequence of pixel sums
            var sumSequence = sum.Process(crop.Process(cameraCapture.Generate()));

            // no need to use expression builders here since we can write expressions in c# directly
            var greaterThan = sumSequence.Select(value => value.Val2 > 3000000);

            // for Rx operators we can just call them directly
            var distinctUntilChanged = greaterThan.DistinctUntilChanged();

            // my clumsy implementation of not :)
            var notDistingctUntilChanged = distinctUntilChanged.Select(value => !value);

            // send output to arduino
            DigitalOutput digitalOutput = new() { Pin = 12, PortName = "COM4" };
            var output = digitalOutput.Process(distinctUntilChanged);

            // subscribe to entire pipeline; print out all values and errors
            using var subscription = output.Subscribe(
                value => Console.WriteLine(value),
                ex => Console.Error.WriteLine(ex));

            // wait for end...
            Console.ReadLine();
        }
    }
}
