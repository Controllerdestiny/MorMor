# MorMor
# 基于[LLOneBot](https://github.com/LLOneBot/LLOneBot) 开发的.NET TerrariaServerBot
* MomoAPI基于[Sora](https://github.com/Hoshikawa-Kaguya/Sora)更改而来。
* MorMor项目中数据库与分页工具部使用了[TShock](https://github.com/Pryaxis/TShock)代码<br>
 ## 使用前你可能需要配置Mail服务机器人会用到
 ## 部署[LLOneBot](https://github.com/LLOneBot/LLOneBot)
1. 你需要安装NTQQ
2. 你需要部署[LiteLoaderQQNT](https://github.com/LiteLoaderQQNT/LiteLoaderQQNT) 推荐使用[LiteLoaderQQNT_Install](https://github.com/Mzdyl/LiteLoaderQQNT_Install/)自动安装程序
3. 将LLOneBot解压至plugin文件夹内
4. 下载MorMor启动后配置生成的配置文件
5. 需要配置Mail服务，机器人会用到，列如给玩家发送注册密码等服务。
 # 预计支持功能(部分功能可能需要插件支持)
## 群功能
- [x] 群禁言
- [ ] 群点歌
- [x] 上下管理
- [x] wiki查询
- [ ] reply(自定义回复)
- [x] 群签到
- [x] 查缩写
- [x] 设置群名
- [ ] 加群请求自定义处理
## 服务器管理
- [x] 执行服务器命令
- [x] 查看玩家背包
- [x] 查看服务器进度
- [ ] 自动重置服务器
- [x] 查看服务器地图
- [x] 在线排行
- [x] 死亡排行
- [x] 用户注册管理
- [ ] 泰拉商店
- [ ] 泰拉奖池
- [x] 查询用户详细
## 其他功能
- [x] 服务器消息与群互相转发(需插件)
## 指令列表
|名称|是否需要TShock|描述|
|-------------|:-------:|:-------:|
|/help      |否|查看指令列表|
|/签到       |否|每日签到|
|/reload    |否|重读配置|
|/group    |否|权限组管理|
|/account   |否|账户组管理|
|/星币       |否|货币管理|
|/scmdperm  |否|查询指令权限|
|/缩写       |否|查询中文缩写|
|/禁        |否|禁言|
|/解        |否|解禁|
|/生成地图   |是|生成Tshock服务器地图|
|/进度查询   |是|查询服务器进度|
|/user      |否|注册用户管理|
|/全禁       |否|全体禁言|
|/设置群名   |否|设置群名|
|/设置管理   |否|设置管理员|
|/取消管理   |否|取消管理员|
|/设置昵称   |否|设置群成员昵称|
|/切换      |否|切换至某个服务器|
|/在线      |否|查询服务器在线玩家|
|/注册      |否|注册服务器用户|
|/注册列表   |否|查询注册列表|
|/查背包    |是|查询服务器玩家背包|
|/在线排行  |是|在线时长排行榜|
|/死亡排行  |是|死亡次数排行榜|
|/消息转发  |否|管理群消息与服务器消息转发|
|/我的信息   |否|查询用户信息|
|/wiki   |否|wiki查询|
## 该机器采依然才用tshock的管理模式，使用RABQ模型，指令设计上不会与tshock有多少区别，另外也采用了插件加载模式，可以自写插件实现更多功能。
