using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Terraria;

public class PlayerinventoryInfo
{
    /// <summary>
    /// 背包
    /// </summary>
    [JsonProperty("inventory")]
    public List<ItemInfo> Inventory { get; init; }

    /// <summary>
    /// 存钱罐
    /// </summary>
    public List<ItemInfo> Piggiy { get; init; }

    /// <summary>
    /// 保险箱
    /// </summary>
    [JsonProperty("safe")]
    public List<ItemInfo> Safe { get; init; }

    /// <summary>
    /// 虚空宝库
    /// </summary>
    public List<ItemInfo> VoidVault { get; init; }

    /// <summary>
    /// 护卫熔炉
    /// </summary>
    public List<ItemInfo> Forge { get; init; }

    /// <summary>
    /// 套装
    /// </summary>
    public List<LoadoutInfo> Loadout { get; init; }

    /// <summary>
    /// 宠物坐骑染料
    /// </summary>
    [JsonProperty("miscDye")]
    public List<ItemInfo> MiscDye { get; init; }

    /// <summary>
    /// 宠物坐骑
    /// </summary>
    [JsonProperty("miscEquip")]
    public List<ItemInfo> MiscEquip { get; init; }

    /// <summary>
    /// buff类型
    /// </summary>
    [JsonProperty("buffType")]
    public List<int> BuffType { get; init; }

    /// <summary>
    /// buff时间
    /// </summary>
    [JsonProperty("buttTime")]
    public List<int> BuffTime { get; init; }

    [JsonProperty("trashItem")]
    public List<ItemInfo> TrashItem { get; init; }
}
