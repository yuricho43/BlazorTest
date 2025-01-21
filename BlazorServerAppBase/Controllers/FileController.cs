using Microsoft.AspNetCore.Mvc;

namespace BlazorServerAppBase.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        [HttpGet("download")]
        public IActionResult DownloadFile([FromQuery] string remotePath, [FromQuery] string fileName)
        {
            try
            {
                // SFTP 연결 정보
                var sftpHost = "192.168.27.238";
                var sftpPort = 22; // 기본 포트
                var sftpUser = "dxsystem";
                var sftpPassword = "1q2w3e4r!@";

                Console.WriteLine("--- Controller:DownloadFile");
                using var client = new Renci.SshNet.SftpClient(sftpHost, sftpPort, sftpUser, sftpPassword);
                client.Connect();

                // 원격 파일 경로 설정
                var fullPath = $"{remotePath.TrimEnd('/')}/{fileName}";

                if (!client.Exists(fullPath))
                {
                    return NotFound(new { Message = "File not found on remote server." });
                }

                // 파일 다운로드
                using var memoryStream = new MemoryStream();
                client.DownloadFile(fullPath, memoryStream);
                client.Disconnect();

                // 클라이언트로 파일 반환
                memoryStream.Position = 0; // 스트림의 위치를 처음으로 설정
                return File(memoryStream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while downloading the file.", Error = ex.Message });
            }
        }
    }
}
