using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CHAT_LENAM
{
    public partial class server : Form
    {
        TcpListener Server;
        List<TcpClient> clients = new List<TcpClient>();

        public server()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            Thread serverThread = new Thread(() =>
            {
                Server = new TcpListener(IPAddress.Any, 5000);
                Server.Start();

                // Thay đổi màu chữ khi hiển thị "Server started..."
                Invoke(new Action(() =>
                {
                    richTextBoxMessages.SelectionColor = Color.Green;
                    richTextBoxMessages.AppendText("Server started...\n");
                    richTextBoxMessages.SelectionColor = richTextBoxMessages.ForeColor; // Khôi phục màu chữ mặc định
                }));

                while (true)
                {
                    TcpClient client = Server.AcceptTcpClient();
                    clients.Add(client);

                    // Hiển thị Client vào CheckedListBox
                    Invoke(new Action(() => checkedListBoxClients.Items.Add(client.Client.RemoteEndPoint.ToString())));

                    // Tạo thread để xử lý Client
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
            });
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string header = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    }
                    else
                    {
                        // Nếu không phải hình ảnh, hiển thị tin nhắn
                        Invoke(new Action(() =>
                        {
                            richTextBoxMessages.SelectionAlignment = HorizontalAlignment.Right;
                            richTextBoxMessages.SelectionColor = Color.Blue;
                            richTextBoxMessages.AppendText($"{header} :Client\n");
                            richTextBoxMessages.SelectionAlignment = HorizontalAlignment.Left;
                        }));
                    }
                }
                catch
                {
                    break;
                }
            }

            // Xóa Client khi ngắt kết nối
            clients.Remove(client);
            Invoke(new Action(() =>
            {
                checkedListBoxClients.Items.Remove(client.Client.RemoteEndPoint.ToString());
            }));
        }
        





        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBoxMessage.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (string selectedClient in checkedListBoxClients.CheckedItems)
            {
                TcpClient client = clients.FirstOrDefault(c =>
                    c.Client.RemoteEndPoint.ToString() == selectedClient);

                if (client != null)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() =>
                            richTextBoxMessages.AppendText($"Lỗi gửi tin nhắn đến {selectedClient}: {ex.Message}\n")));
                    }
                }
            }
            Invoke(new Action(() =>
            {
                richTextBoxMessages.SelectionColor = Color.Red; // Màu đỏ cho tin nhắn Server
                richTextBoxMessages.AppendText($"Server: {message}\n");
            }));

            textBoxMessage.Clear();
        }

        private void buttonSendImage_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có client nào đang kết nối không
                if (clients.Count > 0)
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                            // Mã hóa hình ảnh thành base64
                            string base64Image = Convert.ToBase64String(imageBytes);

                            string header = $"IMG|{base64Image.Length}|";
                            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                            byte[] dataToSend = headerBytes.Concat(Encoding.UTF8.GetBytes(base64Image)).ToArray();

                            // Gửi hình ảnh đến tất cả các client
                            foreach (TcpClient client in clients)
                            {
                                NetworkStream stream = client.GetStream();
                                stream.Write(dataToSend, 0, dataToSend.Length);
                            }

                            richTextBoxMessages.AppendText("Server đã gửi một hình ảnh.\n");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không có client nào kết nối!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi hình ảnh: {ex.Message}");
            }
        }



    }
}
