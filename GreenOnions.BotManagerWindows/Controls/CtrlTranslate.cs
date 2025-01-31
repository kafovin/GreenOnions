﻿using System.Diagnostics;
using GreenOnions.Interface.Configs.Enums;
using GreenOnions.Translate;
using GreenOnions.Utility;

namespace GreenOnions.BotManagerWindows.Controls
{
    public partial class CtrlTranslate : UserControl, IConfigSetting
    {
        public CtrlTranslate()
        {
            InitializeComponent();
        }

        public void LoadConfig()
        {
            cboTranslateEngine.SelectedIndex = (int)BotInfo.Config.TranslateEngineType;

            txbTranslateAPPID.Text = BotInfo.Config.TranslateAPPID;
            txbTranslateAPPKey.Text = BotInfo.Config.TranslateAPPKey;

            txbTranslateToChinese.Text = BotInfo.Config.TranslateToChineseCMD;
            txbTranslateTo.Text = BotInfo.Config.TranslateToCMD;
            txbTranslateFromToCMD.Text = BotInfo.Config.TranslateFromToCMD;

            foreach (long item in BotInfo.Config.AutoTranslateGroupMembersQQ)
                lstAutoTranslateGroupMemoriesQQ.Items.Add(item.ToString());

            txbTranslateAPPID.Enabled = txbTranslateAPPKey.Enabled = BotInfo.Config.TranslateEngineType != TranslateEngine.YouDao && BotInfo.Config.TranslateEngineType != TranslateEngine.Google3rdPartyApi;
        }

        public void SaveConfig()
        {
            BotInfo.Config.TranslateEngineType = (TranslateEngine)cboTranslateEngine.SelectedIndex;

            BotInfo.Config.TranslateAPPID = txbTranslateAPPID.Text;
            BotInfo.Config.TranslateAPPKey = txbTranslateAPPKey.Text;

            BotInfo.Config.TranslateToChineseCMD = txbTranslateToChinese.Text;
            BotInfo.Config.TranslateToCMD = txbTranslateTo.Text;
            BotInfo.Config.TranslateFromToCMD = txbTranslateFromToCMD.Text;

            HashSet<long> tempAutoTranslateGroupMemoriesQQ = new HashSet<long>();
            foreach (ListViewItem item in lstAutoTranslateGroupMemoriesQQ.Items)
            {
                tempAutoTranslateGroupMemoriesQQ.Add(Convert.ToInt64(item.SubItems[0].Text));
            }
            BotInfo.Config.AutoTranslateGroupMembersQQ = tempAutoTranslateGroupMemoriesQQ;
        }

        public void UpdateCache()
        {

        }

        private void AddAutoTranslateGroupMemoryQQ_Click(object sender, EventArgs e) => ((IConfigSetting)this).AddItemToListView(lstAutoTranslateGroupMemoriesQQ, txbAddAutoTranslateGroupMemoryQQ.Text);

        private void RemoveAutoTranslateGroupMemoryQQ_Click(object sender, EventArgs e) => ((IConfigSetting)this).RemoveItemFromListView(lstAutoTranslateGroupMemoriesQQ);

        private void TranslateGotoAPP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (cboTranslateEngine.SelectedIndex)
            {
                case (int)TranslateEngine.YouDao:
                    MessageBox.Show("有道网页翻译无需APPID和APPKey", "提示");
                    break;
                case (int)TranslateEngine.YouDaoApi:
                    Process.Start("explorer.exe", "https://ai.youdao.com/console/#/");
                    break;
                case (int)TranslateEngine.BaiduApi:
                    Process.Start("explorer.exe", "https://api.fanyi.baidu.com/api/trans/product/desktop");
                    break;
                case (int)TranslateEngine.TencentApi:
                    Process.Start("explorer.exe", "https://console.cloud.tencent.com/cam/capi");
                    break;
                case (int)TranslateEngine.Google3rdPartyApi:
                    MessageBox.Show("第三方谷歌翻译API无需APPID和APPKey", "提示");
                    break;
            }
        }

        private void TranslateEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTranslateEngine.SelectedIndex == 0)
            {
                MessageBox.Show("谷歌网页翻译已失效，请选择其他翻译引擎", "提示");
                cboTranslateEngine.SelectedIndex = 1;
            }
            txbTranslateAPPID.Enabled = txbTranslateAPPKey.Enabled = cboTranslateEngine.SelectedIndex != (int)TranslateEngine.YouDao && cboTranslateEngine.SelectedIndex != (int)TranslateEngine.Google3rdPartyApi;
        }

        private void ShowSupportedLanguages_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(string.Join(", ", cboTranslateEngine.SelectedIndex switch
                {
                    (int)TranslateEngine.YouDao => Constants.YouDaoWebLanguages.Keys,
                    (int)TranslateEngine.YouDaoApi => Constants.YouDaoLanguages.Keys,
                    (int)TranslateEngine.BaiduApi => Constants.BaiduLanguages.Keys,
                    (int)TranslateEngine.TencentApi => Constants.TencentLanguages.Keys,
                    (int)TranslateEngine.Google3rdPartyApi => Constants.GoogleLanguages.Keys,
                    _ => throw new NotImplementedException("没有找到当前引擎对应的语言列表，请联系机器人作者。"),
                }), "当前引擎支持语种", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnTranslateTest_Click(object sender, EventArgs e)
        {
            TranslateEngine engine = BotInfo.Config.TranslateEngineType;
            string? appid = BotInfo.Config.TranslateAPPID;
            string? appkey = BotInfo.Config.TranslateAPPKey;

            BotInfo.Config.TranslateEngineType = (TranslateEngine)cboTranslateEngine.SelectedIndex;
            BotInfo.Config.TranslateAPPID = txbTranslateAPPID.Text;
            BotInfo.Config.TranslateAPPKey = txbTranslateAPPKey.Text;

            try
            {
                string result = await TranslateHandler.TranslateToChinese("Hello");
                MessageBox.Show("调用成功，当前设置可用", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"调用失败，{ex.Message}", "错误");
            }
            finally
            {
                BotInfo.Config.TranslateEngineType = engine;
                BotInfo.Config.TranslateAPPID = appid;
                BotInfo.Config.TranslateAPPKey = appkey;
            }
        }
    }
}
