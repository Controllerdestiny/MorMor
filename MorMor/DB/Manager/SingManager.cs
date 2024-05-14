using MorMor.Exceptions;
using MorMor.Extensions;
using MySql.Data.MySqlClient;
using System.Data;

namespace MorMor.DB.Manager;
public class SignManager
{
    public class Sign
    {
        public long GroupID { get; init; }

        public long UserId { get; init; }

        public string LastDate { get; internal set; }

        public long Date { get; internal set; }
    }

    private readonly IDbConnection database;

    public readonly List<Sign> Signs;

    public SignManager()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("Sign",
            new SqlColumn("GroupId", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("QQ", MySqlDbType.Int64) { Unique = true },
            new SqlColumn("LastDate", MySqlDbType.VarChar),
            new SqlColumn("date", MySqlDbType.Int64)
            );

        var create = new SqlTableCreator(database,
        database.GetSqlType() == SqlType.Sqlite
            ? new SqliteQueryCreator()
            : new MysqlQueryCreator());

        create.EnsureTableStructure(table);
        Signs = ReadAll();
    }

    private List<Sign> ReadAll()
    {
        List<Sign> signs = new();
        using (var read = database.QueryReader("select * from Sign"))
        {
            while (read.Read())
            {
                var QQ = read.Get<long>("QQ");
                var LastDate = read.Get<string>("LastDate");
                var GroupId = read.Get<long>("GroupId");
                var date = read.Get<long>("date");
                signs.Add(new Sign()
                {
                    UserId = QQ,
                    LastDate = LastDate,
                    GroupID = GroupId,
                    Date = date,
                });
            }
        }
        return signs;
    }

    public Sign? Query(long groupid, long id)
    {
        return Signs.Find(x => x.GroupID == groupid && x.UserId == id);
    }

    public Sign SingIn(long groupid, long id)
    {
        var signinfo = Query(groupid, id);
        var Now = DateTime.Now.ToString("yyyyMMdd");
        if (signinfo == null)
        {
            var exec = database.Query("INSERT INTO `Sign` (`QQ`, `GroupId`, `LastDate`, `date`) VALUES (@0, @1, @2, @3)", id, groupid, Now, 1);
            if (exec == 1)
            {
                var signin = new Sign()
                {
                    Date = 1,
                    UserId = id,
                    GroupID = groupid,
                    LastDate = Now,
                };
                Signs.Add(signin);
                return signin;
            }
            else
            {
                throw new SignException($"添加至数据库失败!");
            }
        }
        else
        {
            if (signinfo.LastDate == Now)
            {
                throw new SignException("你已经签到过了你个傻逼!");
            }
            else
            {
                if (database.Query("UPDATE `Sign` SET `date` = @0, `LastDate` = @1 WHERE `GroupId` = @2 AND `QQ` = @3", signinfo.Date + 1, Now, groupid, id) == 1)
                {
                    signinfo.Date += 1;
                    signinfo.LastDate = Now;
                }
                else
                {
                    throw new SignException("更新至数据库失败!");
                }
            }
        }
        return signinfo;
    }
}
