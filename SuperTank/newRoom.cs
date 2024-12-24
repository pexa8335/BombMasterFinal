using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTank
{
    public partial class newRoom : Form
    {
        public newRoom()
        {
            InitializeComponent();
        }

        Lobby lobby;

        bool checkRoomID(string idRoom)
        {
            if (!string.IsNullOrEmpty(roomID.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Room ID required!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        void TurnForm()
        {
            lobby = new Lobby();
            this.Hide();
            lobby.Show();
        }

        private async void createButton_Click(object sender, EventArgs e)
        {
            SocketClient.SendData($"CREATE_ROOM;{roomID.Text}");

            await WaitFunction();

            if (SocketClient.isCreateRoom)
            {
                TurnForm();
            }
            else
            {
                MessageBox.Show($"Room {roomID.Text} has been created!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            roomID.Text = cbb_listRoom.SelectedItem.ToString();
        }

        private async void showRoomList_Click(object sender, EventArgs e)
        {
            SocketClient.SendData("SEND_ROOM_LIST");

            await WaitFunction();

            cbb_listRoom.Items.Clear();
            int count = SocketClient.lobbies.Count;
            for (int i = 0; i < count; i++)
            {
                cbb_listRoom.Items.Add(SocketClient.lobbies[i].RoomId);
            }
        }

        private async void btn_joinRoom_Click(object sender, EventArgs e)
        {
            if (!checkRoomID(roomID.Text)) return;

            SocketClient.SendData($"JOIN_ROOM;{roomID.Text}");

            await WaitFunction();

            if (SocketClient.isJoinRoom)
            {
                TurnForm();
            }
            else
            {
                MessageBox.Show($"Phòng {roomID.Text} chưa được tạo hoặc đã đủ người hoặc đã bắt đầu chơi!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task WaitFunction()
        {
            await Task.Delay(700);
        }

        private void NewRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            SocketClient.Disconnect();
            SocketClient.ClearLobby();
            
        }

        private void newRoom_Load(object sender, EventArgs e)
        {

        }

       
    }
}
