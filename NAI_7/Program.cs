using NAI_7.Services;

var settings = Reader.Read(args[0]);
var alg = new SimulatedAnnealing(settings);
var combination = alg.Solve();

Console.WriteLine(alg.Interpret(combination));
Console.WriteLine(alg.ValueOf(combination));