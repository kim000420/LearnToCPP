// /home/rsa-key-20251110/ChatServer/Program.cs
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

// UDP 릴레이 서버 로직
namespace ChatServer
{
    class Server
    {
        // [헤더("서버 설정")]
        // [툴팁("UDP 릴레이 포트입니다. 클라이언트 및 방화벽과 일치해야 합니다.")]
        private static int port = 7777;

        // "접속"된 클라이언트 목록 (실제로는 IP와 포트의 목록)
        // TCP와 달리 "연결"이 없으므로, 서버에 한 번이라도 패킷을 보낸 상대를 "클라이언트"로 간주합니다.
        private static readonly List<IPEndPoint> clients = new List<IPEndPoint>();
        private static readonly object lockObject = new object();
        private static UdpClient udpServer;

        static async Task Main(string[] args)
        {
            Console.WriteLine($"[INFO] UDP 릴레이 서버가 포트 {port}에서 시작되었습니다...");
            // 지정된 포트에서 수신 대기
            udpServer = new UdpClient(port);

            while (true)
            {
                try
                {
                    // 1. 클라이언트로부터 UDP 패킷을 비동기로 수신
                    // result.Buffer: 수신된 데이터
                    // result.RemoteEndPoint: 데이터를 보낸 곳의 주소 (IP:Port)
                    UdpReceiveResult result = await udpServer.ReceiveAsync();
                    IPEndPoint clientEndPoint = result.RemoteEndPoint;

                    // 2. 새 클라이언트인지 확인하고 목록에 등록
                    bool isNewClient = false;
                    lock (lockObject)
                    {
                        // IPEndPoint는 Equals 비교가 필요할 수 있으나, List.Contains가 잘 작동합니다.
                        if (!clients.Contains(clientEndPoint))
                        {
                            clients.Add(clientEndPoint);
                            Console.WriteLine($"[INFO] 새 클라이언트 등록: {clientEndPoint}");
                            isNewClient = true;
                        }
                    }

                    // 3. 메시지 로깅
                    string message = Encoding.UTF8.GetString(result.Buffer);
                    // 접속 메시지는 릴레이만 하고 로그는 찍지 않음 (선택 사항)
                    if (!isNewClient || !message.Contains("...님이 접속했습니다."))
                    {
                        Console.WriteLine($"[MSG] {clientEndPoint}로부터 수신: {message}");
                    }

                    // 4. 수신된 패킷을 모든 클라이언트에게 릴레이(브로드캐스트)
                    await BroadcastMessageAsync(result.Buffer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] 수신 대기 중 오류: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 등록된 모든 클라이언트에게 메시지(바이트 배열)를 릴레이합니다.
        /// </summary>
        private static async Task BroadcastMessageAsync(byte[] data)
        {
            IPEndPoint[] currentClients;
            lock (lockObject)
            {
                currentClients = clients.ToArray(); // 스냅샷 복사
            }

            foreach (var clientEP in currentClients)
            {
                try
                {
                    // 각 클라이언트에게 받은 데이터를 그대로 전송
                    await udpServer.SendAsync(data, data.Length, clientEP);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARN] {clientEP}로 릴레이 실패: {ex.Message}");
                    // TODO: 전송에 실패한 클라이언트(연결이 끊겼거나 IP가 바뀜)를
                    // 주기적으로 리스트에서 제거하는 "Heartbeat(심장박동)" 시스템이 필요합니다.
                    // UDP는 연결이 끊겼는지 알 수 없기 때문입니다.
                }
            }
        }
    }
}