
Bonsai.Vision.CameraCapture cameraCapture = new Bonsai.Vision.CameraCapture();
camera.Index = 0;

Bonsai.Vision.Crop crop = new Bonsai.Vision.Crop();
crop.RegionOfInterest = new System.Windows.Rect(222,34,228,308);
IObservable<OpenCV.Net.IPlImage> cameraCaptureCropped = crop.Process(cameraCapture);

Bonsai.DSP.Sum sum = new Bonsai.DSP.Sum();
IObservable<OpenCV.Net.Scalar> sumRes = sum.Process(cameraCaptureCropped);

// how can I build a sequence from the red component of sum?
// what is a Bonsai expression (ExpressionBuilder)?
IObservable<???> sumResRed = ???

// how can I build a sequence greater than a value?
// what is a Bonsai expression?
IObservable<Boolean> sumResRedGreaterThan = ???

Bonsai.Reactive.DistinctUntilChanged distinctUntilChanged = new Bonsai.Reactive.DistinctUntilChanged();
IObservable<Boolean> distinctValues = distirctUntilChanged.Process(sumResRedGreaterThan);

// another expression
IObservable<Boolean> distinctValuesNot = ???

Bonsai.Arduino.DigitalOutput digitalOutput = new Bonsai.Arduino.DigitalOutput();
digitalOutput.Pin = 12;
digitalOutput.PortName = "COM4"
IObservable<Boolean> end = digitalOutput.Process(distinctValues);

end.Subscribe(item => "");
