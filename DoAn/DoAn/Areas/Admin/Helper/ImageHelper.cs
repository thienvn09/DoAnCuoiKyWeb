using System.IO;
using Microsoft.AspNetCore.Http;
using System;

namespace DoAn.Areas.Helper
{
    public class ImageHelper
    {
        public static string UploadImage(IFormFile imageFile, string folder)
        {
            try
            {
                // Kiểm tra file null
                if (imageFile == null || imageFile.Length == 0)
                    throw new Exception("File không hợp lệ.");

                // Xác thực loại file
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                if (Array.IndexOf(validExtensions, fileExtension) < 0)
                    throw new Exception("Chỉ chấp nhận file ảnh có định dạng: .jpg, .jpeg, .png, .gif");

                // Giới hạn kích thước file (ví dụ: 2MB)
                if (imageFile.Length > 2 * 1024 * 1024)
                    throw new Exception("File vượt quá kích thước tối đa 2MB.");

                // Tạo tên file duy nhất
                var fileName = Guid.NewGuid().ToString() + fileExtension;

                // Đảm bảo thư mục tồn tại
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folder);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Đường dẫn file đầy đủ
                var filePath = Path.Combine(uploadFolder, fileName);

                // Lưu file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Trả về tên file
                return fileName;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc xử lý phù hợp
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
