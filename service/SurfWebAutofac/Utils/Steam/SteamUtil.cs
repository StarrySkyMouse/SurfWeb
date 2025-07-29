using System.Net;
using System.Net.Sockets;
using System.Text;
using static Utils.Steam.SteamServerQuery;

namespace Utils.Steam;

public static class SteamUtil
{
    public static ServerInfo GetServerInfo(string ip, int port)
    {
        var info = QueryServer(ip, port); // 替换为服务器IP和端口
        return info;
    }

    public static List<PlayerInfo> GetServerPlayerList(string ip, ushort port)
    {
        var players = QueryPlayers(ip, port);
        return players;
    }
}

public class SteamServerQuery
{
    public static List<PlayerInfo> QueryPlayers(string ip, int port, int timeout = 3000)
    {
        var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        using (var udp = new UdpClient())
        {
            udp.Client.SendTimeout = timeout;
            udp.Client.ReceiveTimeout = timeout;
            udp.Client.ReceiveBufferSize = 4096;

            // 1. 获取挑战值
            var challenge = GetChallengeValue(udp, endPoint);

            // 2. 发送带挑战值的玩家查询请求
            var request = new byte[9];
            request[0] = 0xFF;
            request[1] = 0xFF;
            request[2] = 0xFF;
            request[3] = 0xFF;
            request[4] = 0x55; // A2S_PLAYER
            Buffer.BlockCopy(challenge, 0, request, 5, 4);

            udp.Send(request, request.Length, endPoint);

            // 3. 接收并解析响应
            IPEndPoint remote = null;
            var response = udp.Receive(ref remote);

            if (response.Length < 5 || response[4] != 0x44) // 'D' 表示玩家数据
                throw new Exception($"无效的玩家列表响应 (头部: 0x{response[4]:X2})");

            var players = new List<PlayerInfo>();
            var reader = new ResponseReader(response, 5);

            var playerCount = reader.ReadByte();
            for (var i = 0; i < playerCount; i++)
                players.Add(new PlayerInfo
                {
                    Index = reader.ReadByte(),
                    Name = reader.ReadString(),
                    Score = reader.ReadInt32(),
                    Duration = reader.ReadFloat()
                });

            return players;
        }
    }

    private static byte[] GetChallengeValue(UdpClient udp, IPEndPoint endPoint)
    {
        // 发送初始挑战请求
        byte[] challengeRequest = { 0xFF, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0xFF };
        udp.Send(challengeRequest, challengeRequest.Length, endPoint);

        IPEndPoint remote = null;
        var response = udp.Receive(ref remote);

        if (response.Length < 9 || response[4] != 0x41) // 'A' 表示挑战响应
            throw new Exception("获取挑战值失败");

        var challenge = new byte[4];
        Buffer.BlockCopy(response, 5, challenge, 0, 4);
        return challenge;
    }

    public static ServerInfo QueryServer(string ip, int port, int timeout = 3000)
    {
        var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        using (var udp = new UdpClient())
        {
            udp.Client.SendTimeout = timeout;
            udp.Client.ReceiveTimeout = timeout;
            udp.Client.ReceiveBufferSize = 4096;

            // 1. 发送初始查询请求
            byte[] request =
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69,
                0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
            };
            udp.Send(request, request.Length, endPoint);

            byte[] response;
            IPEndPoint remote = null;

            try
            {
                response = udp.Receive(ref remote);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
            {
                throw new Exception("服务器响应超时");
            }

            // 2. 检查是否需要挑战值
            if (response.Length >= 5 && response[4] == 0x41) // 'A' 表示需要挑战值
            {
                if (response.Length < 9)
                    throw new Exception("无效的挑战响应");

                // 提取挑战值 (4字节)
                var challenge = new byte[4];
                Buffer.BlockCopy(response, 5, challenge, 0, 4);

                // 构造带挑战值的新请求
                var newRequest = new byte[request.Length + 4];
                Buffer.BlockCopy(request, 0, newRequest, 0, request.Length);
                Buffer.BlockCopy(challenge, 0, newRequest, request.Length, 4);

                udp.Send(newRequest, newRequest.Length, endPoint);
                response = udp.Receive(ref remote);
            }

            // 3. 验证响应头
            if (response.Length < 5 || response[4] != 0x49) // 0x49 = 'I'
                throw new Exception($"无效的服务器响应 (头部: 0x{response[4]:X2})");

            var reader = new ResponseReader(response, 5);
            return new ServerInfo
            {
                Protocol = reader.ReadByte(),
                Name = reader.ReadString(),
                Map = reader.ReadString(),
                Folder = reader.ReadString(),
                Game = reader.ReadString(),
                Players = reader.ReadInt16(), // 修复：小端序读取
                MaxPlayers = reader.ReadInt16(), // 修复：小端序读取
                Bots = reader.ReadByte(),
                ServerType = GetServerType(reader.ReadByte()),
                Environment = GetEnvironment(reader.ReadByte()),
                Visibility = reader.ReadByte() == 0,
                VAC = reader.ReadByte() == 1,
                Version = reader.ReadString()
            };
        }
    }

    private static string GetServerType(byte type)
    {
        return type switch
        {
            0x64 => "专用服务器",
            0x6C => "监听服务器",
            0x70 => "SourceTV中转",
            _ => $"未知 (0x{type:X2})"
        };
    }

    private static string GetEnvironment(byte env)
    {
        return env switch
        {
            0x77 => "Windows",
            0x6C => "Linux",
            0x6D => "macOS",
            _ => $"未知 (0x{env:X2})"
        };
    }

    public class ServerInfo
    {
        public byte Protocol { get; set; }
        public string Name { get; set; }
        public string Map { get; set; }
        public string Folder { get; set; }
        public string Game { get; set; }
        public short Players { get; set; } // 当前玩家数
        public short MaxPlayers { get; set; } // 最大玩家数
        public byte Bots { get; set; } // 机器人数量
        public string ServerType { get; set; } // 服务器类型
        public string Environment { get; set; } // 操作系统环境
        public bool Visibility { get; set; } // 可见性
        public bool VAC { get; set; } // VAC保护
        public string Version { get; set; } // 服务器版本

        public override string ToString()
        {
            return $@"服务器信息:
  名称: {Name}
  地图: {Map}
  游戏: {Game}
  玩家: {Players}/{MaxPlayers} (机器人: {Bots})
  类型: {ServerType}
  系统: {Environment}
  可见性: {(Visibility ? "公开" : "私有")}
  VAC保护: {(VAC ? "启用" : "禁用")}
  版本: {Version}";
        }
    }

    public class PlayerInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public float Duration { get; set; } // 在线时长(秒)

        public override string ToString()
        {
            return $"[{Index}] {Name} - 得分: {Score} - 在线: {TimeSpan.FromSeconds(Duration):hh\\:mm\\:ss}";
        }
    }

    // 增强的二进制读取器
    private class ResponseReader
    {
        private readonly byte[] _data;
        private int _index;

        public ResponseReader(byte[] data, int start = 0)
        {
            _data = data;
            _index = start;
        }

        public byte ReadByte()
        {
            CheckBounds(1);
            return _data[_index++];
        }

        // 修复的小端序读取
        public short ReadInt16()
        {
            var value = (short)(_data[_index] | (_data[_index + 1] << 8));
            _index += 2;
            return value;
        }

        public int ReadInt32()
        {
            CheckBounds(4);
            var value = _data[_index] | (_data[_index + 1] << 8) |
                        (_data[_index + 2] << 16) | (_data[_index + 3] << 24);
            _index += 4;
            return value;
        }

        public float ReadFloat()
        {
            CheckBounds(4);
            var value = BitConverter.ToSingle(_data, _index);
            _index += 4;
            return value;
        }

        public string ReadString()
        {
            if (_index >= _data.Length)
                return string.Empty;

            var start = _index;
            while (_index < _data.Length && _data[_index] != 0)
                _index++;

            var result = Encoding.UTF8.GetString(_data, start, _index - start);
            _index++; // 跳过终止符
            return result;
        }

        private void CheckBounds(int bytesRequired)
        {
            if (_index + bytesRequired > _data.Length)
                throw new Exception("响应数据不完整");
        }
    }
}