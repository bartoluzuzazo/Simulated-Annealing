using System.Numerics;
using Microsoft.VisualBasic;
using NAI_7.Models;

namespace NAI_7.Services;

public class SimulatedAnnealing
{
    private readonly AppSettings _settings;

    public SimulatedAnnealing(AppSettings settings)
    {
        _settings = settings;
    }
    
    public List<bool> Solve()
    {
        var currentTemp = _settings.Temperature;
        var annealer = (decimal temp) => (decimal)(temp - _settings.TempDecrease!);
        var current = new List<bool>(new bool[_settings.CountOfItems]);
        var best = new List<bool>(new bool[_settings.CountOfItems]);
        for (var k = 1; currentTemp > 0; k++)
        {
            var candidate = RandomNeighbour(current);
            
            if (WeightOf(candidate) > _settings.KnapsackCapacity) continue;

            Console.WriteLine($"k={k} t={currentTemp}");
            Console.WriteLine($"best={string.Join("", best.Select(b => b ? "1" : "0"))}");
            Console.WriteLine($"current={string.Join("", current.Select(b => b ? "1" : "0"))} candidate={string.Join("", candidate.Select(b => b ? "1" : "0"))}");
            
            current = IsBetter(current, candidate) ? candidate : SwapWithProbability(current, candidate, currentTemp);
            
            if (WeightOf(current) < _settings.KnapsackCapacity && ValueOf(current) > ValueOf(best))
            {
                best = current;
            }
            
            currentTemp = annealer(currentTemp);
        }

        return best;
    }
    private List<bool> SwapWithProbability(List<bool> current, List<bool> candidate, decimal currentTemp)
    {
        var rand = new Random();
        // Console.WriteLine($"{ValueOf(current)}, {ValueOf(candidate)}");
        var p = Math.Pow(Math.E, (double)(-1*(Math.Abs(ValueOf(current) - ValueOf(candidate)) / currentTemp)));
        var q = rand.NextDouble();
        // Console.WriteLine($"{q} : {p} - {(q > p ? "swap":"noswap")}");
        return q > p ? current : candidate;
    }

    private List<bool> RandomNeighbour(List<bool> current)
    {
        var next = new List<bool>(current);
        var rand = new Random();
        var i = rand.Next(0, _settings.CountOfItems-1);
        next[i] = !current[i];
        return next;
    }

    public decimal ValueOf(List<bool> solution)
    {
        decimal value = 0;
        for (var i = 0; i < _settings.CountOfItems; i++)
        {
            if (solution[i])
            {
                value += _settings.Items[i].Item1;
            }
        }
        return value;
    }
    
    private decimal WeightOf(List<bool> solution)
    {
        decimal weight = 0;
        for (var i = 0; i < _settings.CountOfItems; i++)
        {
            if (solution[i])
            {
                weight += _settings.Items[i].Item2;
            }
        }
        return weight;
    }

    private bool IsBetter(List<bool> current, List<bool> candidate)
    {
        return ValueOf(candidate) > ValueOf(current);
    }
    
    public string Interpret(List<bool> solution)
    {
        var combination = "";
        for (var i = 0; i < _settings.CountOfItems; i++)
        {
            if (solution[i])
            {
                combination += _settings.Items[i] + " ";
            }
        }

        return combination;
    }
    
    private static readonly Func<int, decimal> Boltzmann = (k) => 1/(1+(decimal)Math.Log(k));
    
    private static readonly Func<int, decimal> Geometric = (k) => (decimal)Math.Pow(0.9, k);

}