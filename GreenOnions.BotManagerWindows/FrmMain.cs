﻿using GreenOnions.BotMain;
using GreenOnions.BotMain.CqHttp;
using GreenOnions.BotMain.MiraiApiHttp;
using GreenOnions.Interface;
using GreenOnions.Utility;
using GreenOnions.Utility.Helper;

namespace GreenOnions.BotManagerWindows
{
    public partial class FrmMain : Form
	{
		private WebBrowserForm webBrowserForm;
		private bool _connecting;
		public FrmMain()
		{
			InitializeComponent();

            webBrowserForm = new WebBrowserForm();
            EventHelper.GetDocumentByBrowserEvent += webBrowserForm.GetDocument;

			#region -- 读取配置 --
			try
			{
				if (!File.Exists("config.json"))
				{
					MessageBox.Show("初次使用本机器人，请先配置相关参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
					OpenSetting();
				}

				txbQQ.Text = BotInfo.Config.QQId.ToString();
				txbIP.Text = BotInfo.Config.IP;
				txbPort.Text = BotInfo.Config.Port.ToString();
				txbVerifyKey.Text = BotInfo.Config.VerifyKey;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"读取配置发生异常，{ex.Message}，请删除应用目录下的config.json和cache.json文件后重启应用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion -- 读取配置 --
		}

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

			Task.Run(() =>
			{
				int iLoadCount = PluginManager.Load();
				if (iLoadCount > 0)
					Invoke(new Action(() => btnPlugins.Text = $"插件列表({iLoadCount})"));

				//自动连接到机器人平台
				if (BotInfo.Config.AutoConnectEnabled)
				{
					Task.Delay(BotInfo.Config.AutoConnectDelay * 1000).Wait();
					WorkingTimeRecorder.DoWork = true;
					if (BotInfo.Config.AutoConnectProtocol == 0)
						ConnectToMiraiApiHttp();
					else
						ConnectToCqHttp();
				}
			});
		}

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
			if (WindowState == FormWindowState.Minimized)
			{
				if (chkMinimizeToTray.Checked)
				{
					Hide();
					ShowInTaskbar = false;
					notifyIcon.Visible = true;
				}
			}
		}

		private void ConnectToPlatform(int platform)
		{
            switch (platform)
            {
                case 0:
					ConnectToMiraiApiHttp();
					break;
				case 1:
					ConnectToCqHttp();
					break;
			}
        }

		public void ConnectToMiraiApiHttp(long qqId, string ip, ushort port, string verifyKey)
		{
            try
            {
                MiraiApiHttpMain.Connect(qqId, ip, port, verifyKey, (bConnect, nickNameOrErrorMessage) => Invoke(new Action(() => Connecting(bConnect, qqId, ip, port, verifyKey, nickNameOrErrorMessage, 0, "mirai-api-http"))));
            }
            catch (Exception ex)
			{
				LogHelper.WriteErrorLogWithUserMessage("连接到mirai-api-http发生异常", ex);
                MessageBox.Show("连接mirai-api-http失败" + ex.Message);
            }
			_connecting = false;
		}

		public void ConnectToCqHttp(long qqId, string ip, ushort port, string verifyKey)
		{
			try
			{
                CqHttpMain.Connect(qqId, ip, port, verifyKey, (bConnect, nickNameOrErrorMessage) => Invoke(new Action(() => Connecting(bConnect, qqId, ip, port, verifyKey, nickNameOrErrorMessage, 1, "cqhttp"))));
			}
			catch (Exception ex)
			{
				LogHelper.WriteErrorLogWithUserMessage("连接到cqhttp发生异常", ex);
                MessageBox.Show("连接cqhttp失败" + ex.Message);
            }
			_connecting = false;
		}

		private void btnDeconnect_Click(object? sender, EventArgs e)
		{
			WorkingTimeRecorder.DoWork = false;
			Disconnect();
		}

		private void Disconnect()
        {
			TextReader sr = new StringReader("exit");
			Console.SetIn(sr);

			btnConnectToMiraiApiHttp.Click -= btnDeconnect_Click;
			btnConnectToMiraiApiHttp.Click += btnConnectToMiraiApiHttp_Click;

			btnConnectToCqHttp.Click -= btnDeconnect_Click;
			btnConnectToCqHttp.Click += btnConnectToCqHttp_Click;
		}

		private void btnConnectToMiraiApiHttp_Click(object? sender, EventArgs e)
		{
			WorkingTimeRecorder.DoWork = true;
			ConnectToMiraiApiHttp();
		}

		private void ConnectToMiraiApiHttp()
		{
			if (!_connecting)
			{
				if (CheckInfo())
				{
					_connecting = true;
					ConnectToMiraiApiHttp(Convert.ToInt64(txbQQ.Text), txbIP.Text, Convert.ToUInt16(txbPort.Text), txbVerifyKey.Text);
				}
			}
		}

		private void btnConnectToCqHttp_Click(object? sender, EventArgs e)
		{
			WorkingTimeRecorder.DoWork = true;
			ConnectToCqHttp();
        }

		private void ConnectToCqHttp()
		{
			if (!_connecting)
			{
				if (CheckInfo())
				{
					_connecting = true;
					ConnectToCqHttp(Convert.ToInt64(txbQQ.Text), txbIP.Text, Convert.ToUInt16(txbPort.Text), txbVerifyKey.Text);
				}
			}
		}

		private void btnBotSettings_Click(object sender, EventArgs e) => OpenSetting();

        private void OpenSetting() => new FrmAppSetting().ShowDialog();

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
			Visible = true;
			ShowInTaskbar = true;
			WindowState = FormWindowState.Normal;
			notifyIcon.Visible = false;
		}

		private void Connecting(bool bConnect, long qqId, string ip, ushort port, string verifyKey, string nickNameOrErrorMessage, int platform, string protocolName)
        {
			if (bConnect)
			{
				lblState.Text = $"连接状态: 已连接到{protocolName}, 登录昵称:{nickNameOrErrorMessage}";
				lblState.ForeColor = Color.Black;

				btnConnectToMiraiApiHttp.Text = "断开连接";
				btnConnectToMiraiApiHttp.Click -= btnConnectToMiraiApiHttp_Click;
				btnConnectToMiraiApiHttp.Click += btnDeconnect_Click;

				btnConnectToCqHttp.Text = "断开连接";
				btnConnectToCqHttp.Click -= btnConnectToCqHttp_Click;
				btnConnectToCqHttp.Click += btnDeconnect_Click;

				notifyIcon.Text = $"葱葱机器人:{nickNameOrErrorMessage}";

                BotInfo.Config.QQId = qqId;
                BotInfo.Config.IP = ip;
                BotInfo.Config.Port = port;
                BotInfo.Config.VerifyKey = verifyKey;

                BotInfo.SaveConfigFile();

				WorkingTimeRecorder.StartRecord(platform, ConnectToPlatform, Disconnect);

				webBrowserForm.Show();
			}
			else if (nickNameOrErrorMessage is null)  //连接失败且没有异常
			{
				MessageBox.Show($"连接失败，请检查{protocolName}是否已经正常启动并已配置IP端口相关参数, 以及机器人QQ是否成功登录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else  //发生异常或主动断开连接
			{
				btnConnectToMiraiApiHttp.Text = "连接到mirai-api-http";
				btnConnectToCqHttp.Text = "连接到cqhttp";
				lblState.Text = $"连接状态: 未连接到机器人平台";
				lblState.ForeColor = Color.Red;
				notifyIcon.Text = $"葱葱机器人";
				if (nickNameOrErrorMessage.Length > 0)  //发生异常
					MessageBox.Show("连接失败，" + nickNameOrErrorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool CheckInfo()
        {
            if (BotInfo.IsLogin)
            {
				return false;
            }
			if (string.IsNullOrEmpty(txbQQ.Text))
			{
				MessageBox.Show("请先输入机器人QQ号。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			if (string.IsNullOrEmpty(txbIP.Text))
			{
				MessageBox.Show("请先输入机器人平台 IP。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			if (string.IsNullOrEmpty(txbPort.Text))
			{
				MessageBox.Show("请先输入机器人平台端口号。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			if (string.IsNullOrEmpty(txbVerifyKey.Text))
			{
				MessageBox.Show("请先输入机器人平台连接凭证。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
            }

            BotInfo.Config.QQId = Convert.ToInt64(txbQQ.Text);
            BotInfo.Config.IP = txbIP.Text;
            BotInfo.Config.Port = Convert.ToUInt16(txbPort.Text);
            BotInfo.Config.VerifyKey = txbVerifyKey.Text;
			return true;
		}

        private void btnPlugins_Click(object sender, EventArgs e)
        {
			new FrmPlugins().ShowDialog();
		}
    }
}
