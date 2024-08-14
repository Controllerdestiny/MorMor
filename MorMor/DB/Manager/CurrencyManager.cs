using System.Data;
using MorMor.Extensions;
using MySql.Data.MySqlClient;

namespace MorMor.DB.Manager;


public class CurrencyManager
{
    public class Currency
    {
        public long GroupID { get; set; }

        public long QQ { get; set; }

        public long num { get; set; }
    }

    private readonly IDbConnection database;

    private readonly List<Currency> Currencys = [];

    public CurrencyManager()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("Currency",
            new SqlColumn("QQ", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("GroupId", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("num", MySqlDbType.Int64) { DefaultValue = "0" }
            );

        var create = new SqlTableCreator(database,
                database.GetSqlType() == SqlType.Sqlite
                    ? new SqliteQueryCreator()
                    : new MysqlQueryCreator());

        create.EnsureTableStructure(table);
        Currencys = GetCurrencys();
    }

    private List<Currency> GetCurrencys()
    {
        List<Currency> currencys = new();
        using var read = database.QueryReader("select * from `Currency`");
        while (read.Read())
        {
            var QQ = read.Get<long>("QQ");
            var GroupId = read.Get<long>("GroupId");
            var num = read.Get<long>("num");
            currencys.Add(new Currency()
            {
                QQ = QQ,
                GroupID = GroupId,
                num = num
            });
        }
        return currencys;
    }

    public Currency? Query(long groupid, long id)
    {
        return Currencys.Find(x => x.GroupID == groupid && x.QQ == id);
    }

    public Currency? Del(long groupid, long id, long num)
    {
        var usercurr = Query(groupid, id);
        if (usercurr == null)
            throw new Exception("用户没有星币可以扣除!");
        if (database.Query("UPDATE `Currency` SET `num` = @0 WHERE `GroupId` = @1 AND `QQ` = @2", usercurr.num - num, groupid, id) == 1)
        {
            usercurr.num -= num;
        }
        else
        {
            throw new Exception("更新至数据库失败!");
        }
        return usercurr;
    }


    public Currency Add(long groupid, long id, long num)
    {
        var usercurr = Query(groupid, id);
        if (usercurr == null)
        {
            var exec = database.Query("INSERT INTO `Currency` (`QQ`, `GroupId`, `num`) VALUES (@0, @1, @2)", id, groupid, num);
            if (exec == 1)
            {
                var curr = new Currency()
                {
                    QQ = id,
                    GroupID = groupid,
                    num = num
                };
                Currencys.Add(curr);
                return curr;
            }
            else
            {
                throw new Exception($"添加至数据库失败!");
            }

        }
        else
        {
            if (database.Query("UPDATE `Currency` SET `num` = @0 WHERE `GroupId` = @1 AND `QQ` = @2", usercurr.num + num, groupid, id) == 1)
            {
                usercurr.num += num;
            }
            else
            {
                throw new Exception("更新至数据库失败!");
            }
        }
        return usercurr;
    }
}
