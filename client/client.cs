using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace client
{
    public partial class client : Form
    {
        TcpClient Client;
        NetworkStream stream;

        public client()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                // Kết nối đến Server
                Client = new TcpClient("127.0.0.1", 5000);
                stream = Client.GetStream();

                // Tạo thread để nhận tin nhắn từ Server
                Thread clientThread = new Thread(() =>
                {
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        try
                        {
                            // Đọc dữ liệu từ Server
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0) break; // Ngắt kết nối

                            // Chuyển dữ liệu thành chuỗi
                            string header = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                            // Kiểm tra xem dữ liệu nhận được có phải là hình ảnh hay không
                            if (header.StartsWith("IMG|"))
                            {
                                // Nếu dữ liệu là hình ảnh, xử lý nhận hình ảnh
                                int separatorIndex = header.IndexOf("|", 4);
                                if (separatorIndex != -1)
                                {
                                    // Lấy độ dài của hình ảnh từ header
                                    string lengthStr = header.Substring(4, separatorIndex - 4);
                                    int imageLength = int.Parse(lengthStr);

                                    // Đọc hình ảnh từ stream
                                    byte[] imageBytes = new byte[imageLength];
                                    int totalBytesRead = 0;
                                    while (totalBytesRead < imageLength)
                                    {
                                        int remainingBytes = imageLength - totalBytesRead;
                                        int bytesReadImage = stream.Read(imageBytes, totalBytesRead, remainingBytes);
                                        totalBytesRead += bytesReadImage;
                                    }

                                    // Hiển thị hình ảnh nhận được
                                    Invoke(new Action(() =>
                                    {
                                        using (MemoryStream ms = new MemoryStream(imageBytes))
                                        {
                                            Image receivedImage = Image.FromStream(ms);
                                            Clipboard.SetImage(receivedImage); // Đặt hình ảnh vào Clipboard
                                            richTextBoxMessages.AppendText("Đã nhận một hình ảnh từ server.\n");
                                        }
                                    }));
                                }
                            }
                            else
                            {
                                // Nếu không phải hình ảnh, thì đây là tin nhắn văn bản
                                Invoke(new Action(() =>
                                {
                                    // Căn chỉnh tin nhắn của Server sang bên phải
                                    richTextBoxMessages.SelectionAlignment = HorizontalAlignment.Right;
                                    // Chỉnh lại thứ tự để hiển thị "Message :Server"
                                    richTextBoxMessages.AppendText($"{header} :Server\n");
                                    richTextBoxMessages.SelectionAlignment = HorizontalAlignment.Left; // Khôi phục căn chỉnh mặc định
                                }));
                            }
                        }
                        catch
                        {
                            break; // Kết nối bị ngắt
                        }
                    }
                });
                clientThread.IsBackground = true;
                clientThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối đến Server: {ex.Message}", "Lỗi");
            }
        }



        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                // Gửi tin nhắn tới Server
                string message = textBoxMessage.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);

                // Hiển thị tin nhắn của mình (Client) trên giao diện, căn chỉnh sang bên trái
                Invoke(new Action(() =>
                {
                    richTextBoxMessages.SelectionAlignment = HorizontalAlignment.Left;
                    richTextBoxMessages.AppendText($"Bạn: {message}\n");
                }));

                textBoxMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi tin nhắn: {ex.Message}", "Lỗi");
            }
        }

        private void buttonSendImage_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem client có kết nối không
                if (Client != null && Client.Connected)
                {
                    // Mở hộp thoại để người dùng chọn file hình ảnh
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Định dạng file hình ảnh

                        // Kiểm tra nếu người dùng chọn một file hợp lệ
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Đọc file hình ảnh thành mảng byte
                            byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                            // Tạo header cho hình ảnh, chứa độ dài của ảnh
                            string header = $"IMG|{imageBytes.Length}|";
                            byte[] headerBytes = Encoding.UTF8.GetBytes(header); // Mã hóa header thành mảng byte

                            // Kết hợp header và dữ liệu hình ảnh vào một mảng duy nhất
                            byte[] dataToSend = headerBytes.Concat(imageBytes).ToArray();

                            // Gửi dữ liệu (header + hình ảnh) tới server
                            stream.Write(dataToSend, 0, dataToSend.Length);
                            richTextBoxMessages.AppendText("Bạn đã gửi một hình ảnh.\n");

                            // Hiển thị hình ảnh đã gửi trong Clipboard
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image sentImage = Image.FromStream(ms);
                                Clipboard.SetImage(sentImage); // Đặt hình ảnh vào Clipboard
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Chưa kết nối tới server!", "Lỗi kết nối");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi hình ảnh: {ex.Message}", "Lỗi");
            }
        }

    }
}
