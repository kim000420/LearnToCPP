// C:/Users/YourName/ChatClient/Program.cs
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// UDP 클라이언트 로직
namespace ChatClient
{
    class Client
    {
        // [헤더("서버 설정")]
        // [툴팁("Google Cloud VM의 공용 IP 주소")]
        private static string serverIP = "34.22.102.159"; // (중요!) 님의 VM IP로 설정

        // [헤더("포트 설정")]
        // [툴팁("서버 및 방화벽과 동일한 UDP 포트")]
        private static int serverPort = 7777;

        private static UdpClient client;
        private static IPEndPoint serverEndPoint;

        static async Task Main(string[] args)
        {
            // 1. UdpClient 생성
            // 포트 번호를 지정하지 않으면 OS가 사용 가능한 임의의 포트를 할당합니다.
            client = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            Console.WriteLine("========================================");
            Console.WriteLine(" C# UDP 채팅 클라이언트 (릴레이 방식)");
            Console.WriteLine("========================================");
            Console.WriteLine($"[INFO] 서버 {serverIP}:{serverPort}에 접속을 시도합니다...");

            // 2. 서버로부터 메시지를 수신하는 태스크를 별도로 시작
            Task receiveTask = ReceiveMessagesAsync();

            // 3. (필수!) 서버에 "접속" 메시지를 보내 자신을 등록
            // 이 패킷을 보내야 서버가 "아, 이런 IP:Port를 쓰는 클라이언트가 있구나" 하고 리스트에 추가합니다.
            try
            {
                byte[] helloMsg = Encoding.UTF8.GetBytes("...님이 접속했습니다.");
                await client.SendAsync(helloMsg, helloMsg.Length, serverEndPoint);
                Console.WriteLine("[SUCCESS] 서버에 접속(등록) 요청을 보냈습니다.");
                Console.WriteLine("[INFO] 채팅을 시작하세요. (종료: /exit)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 서버에 첫 패킷 전송 실패: {ex.Message}");
                Console.WriteLine("[INFO] 서버 IP, 포트, 방화벽(UDP 7777) 설정을 확인하세요.");
                return;
            }


            // 4. 사용자가 메시지를 입력하고 서버로 "전송"하는 루프
            while (true)
            {
                string? message = Console.ReadLine();
                if (string.IsNullOrEmpty(message)) continue;

                if (message.ToLower() == "/exit")
                {
                    break; // while 루프 종료
                }

                byte[] data = Encoding.UTF8.GetBytes(message);
                // 모든 메시지를 TCP 때처럼 "서버"로 보냅니다.
                await client.SendAsync(data, data.Length, serverEndPoint);
            }

            // 5. 종료
            client.Close();
            Console.WriteLine("[INFO] 클라이언트가 종료되었습니다.");
        }

        /// <summary>
        /// "서버"로부터 오는 릴레이 메시지를 지속적으로 수신합니다.
        /// </summary>
        private static async Task ReceiveMessagesAsync()
        {
            try
            {
                while (true)
                {
                    // 1. 서버로부터 릴레이 패킷 수신 대기
                    UdpReceiveResult result = await client.ReceiveAsync();

                    // 2. (보안) 이 릴레이 모델에서는 *서버*가 보낸 패킷만 처리해야 합니다.
                    if (result.RemoteEndPoint.Equals(serverEndPoint))
                    {
                        string message = Encoding.UTF8.GetString(result.Buffer);
                        // TODO: TCP와 마찬가지로 콘솔 입력 중 수신 처리(줄바꿈)가 필요합니다.
                        Console.WriteLine($">> {message}");
                    }
                    else
                    {
                        Console.WriteLine($"[WARN] 알 수 없는 발신자({result.RemoteEndPoint})로부터 패킷 수신 (무시됨)");
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // 클라이언트가 종료(Close)되면 여기서 예외가 발생하며 태스크가 정상 종료됩니다.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 메시지 수신 중 오류: {ex.Message}");
            }
        }
    }
}