// Assets/Scripts/Core/ClientCore.cs (C#에서는 파일 경로보다 네임스페이스가 중요합니다)
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// 간단한 채팅 클라이언트 로직을 포함합니다.
namespace ChatClient
{
    class Client
    {
        // TODO: 서버 IP 주소를 여기에 하드코딩하거나, 실행 시 입력받아야 합니다.
        // Google Cloud VM의 *외부* 고정 IP 주소를 사용하세요.
        // [헤더("서버 설정")]
        private static string serverIP = "34.22.102.159"; // (중요!) 이 IP를 님의 VM 공용 IP로 변경하세요.

        // [헤더("포트 설정")]
        // [툴팁("서버와 동일한 포트 번호를 사용해야 합니다.")]
        private static int port = 7777;

        static async Task Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine(" C# 채팅 클라이언트");
            Console.WriteLine("========================================");
            Console.WriteLine($"[INFO] 서버 IP: {serverIP}:{port} 에 접속을 시도합니다...");

            TcpClient client = new TcpClient();

            try
            {
                // 1. 서버에 비동기 접속 시도
                await client.ConnectAsync(serverIP, port);
                Console.WriteLine("[SUCCESS] 서버에 성공적으로 접속했습니다!");

                NetworkStream stream = client.GetStream();

                // 2. 서버로부터 메시지를 수신하는 태스크를 별도로 시작
                // 이 태스크는 프로그램이 끝날 때까지 계속 메시지를 기다립니다.
                Task receiveTask = ReceiveMessagesAsync(stream);

                Console.WriteLine("[INFO] 접속 완료. 채팅을 시작하세요. (종료: /exit)");

                // 3. 사용자가 메시지를 입력하고 서버로 전송하는 루프 (메인 스레드)
                while (true)
                {
                    string? message = Console.ReadLine(); // C# 8.0 이상 (Nullable)

                    if (string.IsNullOrEmpty(message)) continue;

                    // 종료 명령어
                    if (message.ToLower() == "/exit")
                    {
                        break; // while 루프 종료
                    }

                    // 메시지를 UTF-8 바이트로 변환하여 서버에 전송
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (SocketException ex)
            {
                // [ERROR] 서버에 연결할 수 없는 경우 (IP, 포트, 방화벽 문제)
                Console.WriteLine($"[ERROR] 서버 연결 실패: {ex.Message}");
                Console.WriteLine("[INFO] 서버가 실행 중인지, VM 방화벽(7777)이 열려 있는지 확인하세요.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 예기치 않은 오류 발생: {ex.Message}");
            }
            finally
            {
                // 4. 프로그램 종료 시 클라이언트 자원 정리
                client.Close();
                Console.WriteLine("[INFO] 서버와 연결이 종료되었습니다. 아무 키나 눌러 창을 닫으세요.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 서버로부터 메시지를 지속적으로 수신하여 콘솔에 출력합니다.
        /// </summary>
        /// <param name="stream">서버와 연결된 네트워크 스트림</param>
        private static async Task ReceiveMessagesAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[1024]; // 1KB 버퍼

            try
            {
                while (true)
                {
                    // 비동기로 서버 메시지 대기
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        // 서버가 연결을 끊음 (bytesRead == 0)
                        throw new Exception("서버 연결 종료");
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // [중요] 사용자가 입력하는 줄과 겹치지 않게 처리
                    // 현재 입력 중인 내용을 잠시 지우고, 받은 메시지를 출력한 뒤, 다시 입력 중인 내용을 복원
                    // (간단한 콘솔 앱에서는 완벽하지 않을 수 있으나, 없는 것보다 훨씬 좋습니다.)

                    // 현재 커서 위치 저장
                    int currentCursorTop = Console.CursorTop;
                    int currentCursorLeft = Console.CursorLeft;

                    // 커서를 맨 왼쪽으로 이동시키고, 현재 줄을 지웁니다.
                    Console.SetCursorPosition(0, currentCursorTop);
                    Console.Write(new string(' ', Console.WindowWidth)); // 현재 줄 클리어

                    // 커서를 다시 원래 줄로 돌려놓고 메시지 출력
                    Console.SetCursorPosition(0, currentCursorTop);
                    Console.WriteLine($">> {message}"); // 수신된 메시지 출력

                    // 사용자가 다시 입력할 수 있도록 커서를 복원
                    Console.SetCursorPosition(currentCursorLeft, currentCursorTop);
                }
            }
            catch (Exception)
            {
                // 서버가 비정상 종료되었거나 연결이 끊겼을 때
                Console.WriteLine("\n[INFO] 서버와의 연결이 끊어졌습니다.");
            }
            // 이 태스크가 끝나면 (연결이 끊기면) Main 함수의 finally가 실행됩니다.
        }
    }
}