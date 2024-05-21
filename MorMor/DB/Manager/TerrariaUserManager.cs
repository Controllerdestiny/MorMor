using MorMor.Exceptions;
using MorMor.Extensions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;

namespace MorMor.DB.Manager;

public class TerrariaUserManager
{
    public class User
    {
        public long Id { get; init; }

        public string Name { get; init; }

        public string Server { get; init; }

        public string Password { get; internal set; }

        public long GroupID { get; init; }

    }
    private readonly IDbConnection database;

    public readonly List<User> Users = [];

    public TerrariaUserManager()
    {
        database = MorMorAPI.DB;
        var table = new SqlTable("User",
            new SqlColumn("ID", MySqlDbType.Int64) { Length = 100 },
            new SqlColumn("Server", MySqlDbType.VarChar) {  Length = 100 },
            new SqlColumn("Name", MySqlDbType.VarChar) { Unique = true, Length = 100 },
            new SqlColumn("GroupID", MySqlDbType.Int64) { Length = 100 },
            new SqlColumn("Password", MySqlDbType.VarChar) { Length = 100 }
            );

        var create = new SqlTableCreator(database,
        database.GetSqlType() == SqlType.Sqlite
            ? new SqliteQueryCreator()
            : new MysqlQueryCreator());

        create.EnsureTableStructure(table);
        Users = GetUsers();
    }

    private List<User> GetUsers()
    {
        List<User> users = [];
        using var read = database.QueryReader("select * from `User`");
        while (read.Read())
        {
            var ID = read.Get<long>("ID");
            var Name = read.Get<string>("Name");
            var GroupID = read.Get<long>("GroupID");
            var Server = read.Get<string>("Server");
            var Password = read.Get<string>("Password");
            users.Add(new User()
            {
                Id = ID,
                Name = Name,
                GroupID = GroupID,
                Server = Server,
                Password = Password
            }); ;
        }
        return users;
    }

    public bool HasUser(string server, string Name)
    {
        return Users.Any(x => x.Name == Name && x.Server == server);
    }

    public void Add(long id, long groupid, string Server, string Name, string Password)
    {
        if (Users.Any(x => x.Id == id && x.Name == Name && x.Server == Server))
            throw new TerrariaUserException("此用户已经注册过了!");
        //搜索名字和服务器
        var user = GetUsersByName(Name, Server);
        if (user != null)
            throw new TerrariaUserException($"此名称已经被{user.Id}注册过了!");

        if (database.Query("INSERT INTO `User` (`ID`, `Name`, `GroupID`, `Server`, `Password`) VALUES (@0, @1, @2, @3, @4)", id, Name, groupid, Server, Password) == 1)
        {
            Users.Add(new User()
            {
                Id = id,
                Server = Server,
                Password = Password,
                Name = Name,
                GroupID = groupid
            });
        }
        else
        {
            throw new TerrariaUserException("更新至数据库失败!");
        }
    }
    public void ResetPassword(long id, string servername, string name, string pwd)
    {
        var user = GetUserById(id, servername,name) ?? throw new GroupException("删除权限指向的目标组不存在!");
        if (database.Query("UPDATE `User` SET `Password` = @0 WHERE `User`.`Server` = @1 AND `User`.`ID` = @2 AND `User`.`Name` = @3", pwd, servername, id, name) != 1)
            throw new GroupException("添加至数据库失败!");
        user.Password = pwd;
    }

    public void Remove(string Server, string Name)
    {
        var user = GetUsersByName(Name, Server);
        if (user == null)
            throw new TerrariaUserException($"在{Server} 上没有找到{Name}");
        if (database.Query("DELETE FROM `User` WHERE Server = @0 and Name = @1", Server, Name) == 1)
        {
            Users.RemoveAll(f => f.Server == Server && f.Name == Name);
        }
        else
        {
            throw new TerrariaUserException("更新至数据库失败!");
        }
    }

    public void RemoveByServer(string server)
    {
        var users = GetUsers(server);
        foreach (var user in users)
        {
            Remove(user.Server, user.Name);
        }
    }

    public List<User> GetUsers(string Server)
    {
        return Users.FindAll(f => f.Server == Server);
    }

    public List<User> GetUsers(long id)
    {
        return Users.FindAll(f => f.Id == id);
    }

    public List<User> GetUserById(long id, string server)
    {
        return Users.FindAll(f => f.Server == server && f.Id == id);
    }

    public User? GetUserById(long id, string server, string name)
    {
        return Users.Find(f => f.Server == server && f.Name == name && f.Id == id);
    }

    public User? GetUsersByName(string name, string server)
    {
        return Users.Find(x => x.Name == name && x.Server == server);
    }

    public User? GetUsersByName(string name)
    {
        return Users.Find(x => x.Name == name);
    }

    public void Reset()
    {
        Users.Clear();
        database.Query($"delete from User");
    }
}
