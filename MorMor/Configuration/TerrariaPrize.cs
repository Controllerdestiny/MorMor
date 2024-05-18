using MorMor.Model.Terraria;
using Newtonsoft.Json;

namespace MorMor.Configuration;

public class TerrariaPrize
{
    [JsonProperty("抽奖费用")]
    public long Fess = 10;

    [JsonProperty("奖池内容")]
    public List<Prize> Pool = [];

    public Prize Next()
    {
        Random random = new Random();
        int totalWeight = Pool.Sum(x => x.Probability);
        int randomNumber = random.Next(1, totalWeight + 1);
        int num = 0;
        foreach (var prize in Pool)
        {
            num += prize.Probability;
            if (randomNumber <= num)
            {
                return prize;
            }
        }
        return Pool[random.Next(0, Pool.Count - 1)];
    }

    public List<Prize> Nexts(int count)
    {
        var res = new List<Prize>();
        for (int i = 0; i < count; i++)
        {
            res.Add(Next());
        }
        return res;
    }

    public bool Add(string name, int id, int rate, int max, int min)
    {
        if (Pool.Any(x => x.Name == name || x.ID == id))
            return false;

        Pool.Add(new Prize()
        {
            Name = name,
            ID = id,
            Probability = rate,
            Max = max,
            Min = min
        });
        return true;
    }

    public bool Remove(string name)
    {
        return Pool.RemoveAll(x => x.Name == name) > 0;

    }
}
