using SuperTank;
using SuperTank.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTank
{
    public partial class Lobby : Form
    {
        private CancellationTokenSource _cts;

        public Lobby()
        {
            InitializeComponent();
            namePlayer1.AutoEllipsis = true;
            namePlayer2.AutoEllipsis = true;
            namePlayer3.AutoEllipsis = true;
            namePlayer4.AutoEllipsis = true;
            this.Load += Lobby_Load;

        }

        private async void Lobby_Load(object sender, EventArgs e)
        {
            _cts = new CancellationTokenSource();
            await RunContinuouslyAsync(_cts.Token);
        }

        private async Task RunContinuouslyAsync(CancellationToken token)
        {
            SocketClient.SendData($"SEND_LOBBY;{SocketClient.joinedRoom}");
            await Task.Delay(100, token);

            while (!token.IsCancellationRequested)
            {
                InitLobby();

                if (SocketClient.isStartGame)
                {
                    // Synchronize player list from `joinedLobby` to `SocketClient.players` once game starts
                    if (SocketClient.players.Count == 0)
                    {
                        foreach (var lobbyPlayer in SocketClient.joinedLobby.Players)
                        {
                            //phần này là phần tạo constructor khởi tạo cho playertank giá trị ban đầu, đéo hiểu, k làm dc

                            /*SocketClient.players.Add(new PlayerTank
                            {
                                Name = lobbyPlayer.Name,
                                Position = new PointF(0, 0) // Set initial spawn point or position
                            });*/
                        }
                        Debug.WriteLine("All players added to SocketClient.players for MainGame start.");
                    }

                    // Cancel lobby task and start game -> show form game
                    _cts.Cancel();
                    frmGameMulti multi = new frmGameMulti();
                    this.Hide();
                    multi.Show();
                }

                try
                {
                    await Task.Delay(500, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private void InitLobby()
        {
            if (SocketClient.localPlayer.Name == SocketClient.joinedLobby.Host.Name)
            {
                btn_Start.Enabled = true;
                btn_Start.Visible = true;
            }
            else
            {
                btn_Start.Enabled = false;
                btn_Start.Visible = false;
            }

            string[] playersName = new string[4];
            lb_Total.Text = "SỐ LƯỢNG: " + SocketClient.joinedLobby.PlayersName.Count.ToString();
            lb_roomID.Text = "MÃ PHÒNG: " + SocketClient.joinedLobby.RoomId;
            int countPlayer = SocketClient.joinedLobby.PlayersName.Count;

            for (int i = 0; i < countPlayer; i++)
            {
                switch (i)
                {
                    case 0:
                        namePlayer1.Text = SocketClient.joinedLobby.PlayersName[i];
                        //ptb_player1.Image = Properties.Resources.ares;
                        lbReady1.Visible = true;
                        if (SocketClient.CheckIsReady(namePlayer1.Text))
                        {
                            lbReady1.Text = "Sẵn sàng";
                            lbReady1.ForeColor = Color.Lime;
                        }
                        break;

                    case 1:
                        namePlayer2.Text = SocketClient.joinedLobby.PlayersName[i];
                       // ptb_player2.Image = Properties.Resources.knight;
                        lbReady2.Visible = true;
                        if (SocketClient.CheckIsReady(namePlayer2.Text))
                        {
                            lbReady2.Text = "Sẵn sàng";
                            lbReady2.ForeColor = Color.Lime;
                        }
                        break;

                    case 2:
                        namePlayer3.Text = SocketClient.joinedLobby.PlayersName[i];
                        //ptb_player3.Image = Properties.Resources.serial_killer;
                        lbReady3.Visible = true;
                        if (SocketClient.CheckIsReady(namePlayer3.Text))
                        {
                            lbReady3.Text = "Sẵn sàng";
                            lbReady3.ForeColor = Color.Lime;
                        }
                        break;

                    case 3:
                        namePlayer4.Text = SocketClient.joinedLobby.PlayersName[i];
                        //ptb_player4.Image = Properties.Resources.player1;
                        lbReady4.Visible = true;
                        if (SocketClient.CheckIsReady(namePlayer4.Text))
                        {
                            lbReady4.Text = "Sẵn sàng";
                            lbReady4.ForeColor = Color.Lime;
                        }
                        break;
                }
            }
            for (int i = countPlayer; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        namePlayer1.Text = "Player1";
                        //ptb_player1.Image = Properties.Resources.anonymous;
                        lbReady1.Visible = false;
                        lbReady1.Text = "Chưa sẵn sàng";
                        lbReady1.ForeColor = Color.Red;
                        break;

                    case 1:
                        namePlayer2.Text = "Player2";
                        //ptb_player2.Image = Properties.Resources.anonymous;
                        lbReady2.Visible = false;
                        lbReady2.Text = "Chưa sẵn sàng";
                        lbReady2.ForeColor = Color.Red;
                        break;

                    case 2:
                        namePlayer3.Text = "Player3";
                       // ptb_player3.Image = Properties.Resources.anonymous;
                        lbReady3.Visible = false;
                        lbReady3.Text = "Chưa sẵn sàng";
                        lbReady3.ForeColor = Color.Red;
                        break;

                    case 3:
                        namePlayer4.Text = "Player4";
                       // ptb_player4.Image = Properties.Resources.anonymous;
                        lbReady4.Visible = false;
                        lbReady4.Text = "Chưa sẵn sàng";
                        lbReady4.ForeColor = Color.Red;
                        break;
                }
            }
        }

        private void ptb_player1_Click(object sender, EventArgs e)
        {

        }
    }
}
