using NAI_7.Models;

namespace NAI_7.Services;

public static class Reader
{
    public static AppSettings Read(string path, string delimiter1 = " ", string delimiter2 = ",")
    {
        var data = File.ReadLines(path);
        var rows = data.Select(r => r.Split(delimiter2).ToList()).ToList();
        var items = rows[1].Zip(rows[2], (v, w) => (decimal.Parse(v), decimal.Parse(w))).ToList();
        var nums = data.First().Split(delimiter1).ToList();

        var settings = new AppSettings()
        {
            KnapsackCapacity = int.Parse(nums.First()),
            CountOfItems = int.Parse(nums[1]),
            Temperature = decimal.Parse(nums[2]),
            TempDecrease = nums.Count >= 4 ? decimal.Parse(nums[3]) : null,
            Items = items
            
        };
        return settings;
    }
}