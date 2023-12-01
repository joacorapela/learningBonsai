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

	    // select the red channel
	    var sumSequenceVal2 = sumSequence.Select(x => x.Val2);
	    
	    // create a sequence of inner sequences, where each inner sequence
	    // contains 100 samples from sumSequenceVal2
	    var seqOfseqs = sumSequenceVal2.Window(100);

	    // average the values of each inner sequence
	    var average = seqOfseqs.Select(x => x.Average()).Concat();

	    // greaterThan is a boolean sequence telling the the sum of red 
	    // pixels is larger than the average of the previous 100 values
	    var greaterThan = sumSequenceVal2.CombineLatest(average, (sum, avg) => sum > avg);

            // for Rx operators we can just call them directly
            var distinctUntilChanged = greaterThan.DistinctUntilChanged();

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
