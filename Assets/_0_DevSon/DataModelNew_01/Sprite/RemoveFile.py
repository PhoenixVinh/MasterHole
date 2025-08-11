


import os

# Đường dẫn đến folder A
folder_path = "D:\Hole Em All Collect Master\Assets\Texture2D"  # Ví dụ: "C:/Users/YourName/FolderA" hoặc "/home/user/folderA"

# Lấy danh sách file có đuôi .xx
for file in os.listdir(folder_path):
    if file.endswith("(SDFTexture).png"):
        file_path = os.path.join(folder_path, file)
        try:
            os.remove(file_path)  # Xóa file
            print(f"Đã xóa: {file_path}")
        except Exception as e:
            print(f"Lỗi khi xóa {file_path}: {e}")