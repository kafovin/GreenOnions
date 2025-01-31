﻿namespace GreenOnions.BotManagerWindows.Controls
{
    partial class CtrlOriginalPicture
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chkOriginalPictureCheckPornEnabled = new System.Windows.Forms.CheckBox();
            this.pnlOriginalPictureCheckPorn = new System.Windows.Forms.Panel();
            this.pnlOriginalPictureCheckPornMessage = new System.Windows.Forms.Panel();
            this.lblOriginalPictureCheckPornIllegalReply = new System.Windows.Forms.Label();
            this.txbOriginalPictureCheckPornErrorReply = new System.Windows.Forms.TextBox();
            this.txbOriginalPictureCheckPornIllegalReply = new System.Windows.Forms.TextBox();
            this.lblOriginalPictureCheckPornErrorReply = new System.Windows.Forms.Label();
            this.pnlOriginalPictureCheckPornEvent = new System.Windows.Forms.Panel();
            this.rdoOriginalPictureCheckPornSendByForward = new System.Windows.Forms.RadioButton();
            this.rdoOriginalPictureCheckPornDoNothing = new System.Windows.Forms.RadioButton();
            this.rdoOriginalPictureCheckPornReply = new System.Windows.Forms.RadioButton();
            this.lblOriginalPictureCheckPornEvent = new System.Windows.Forms.Label();
            this.lblOriginalPictureCommand = new System.Windows.Forms.Label();
            this.txbOriginalPictureCommand = new System.Windows.Forms.TextBox();
            this.lblOriginalPictureDownloadingReply = new System.Windows.Forms.Label();
            this.txbOriginalPictureDownloadingReply = new System.Windows.Forms.TextBox();
            this.pnlOriginalPictureCheckPorn.SuspendLayout();
            this.pnlOriginalPictureCheckPornMessage.SuspendLayout();
            this.pnlOriginalPictureCheckPornEvent.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkOriginalPictureCheckPornEnabled
            // 
            this.chkOriginalPictureCheckPornEnabled.AutoSize = true;
            this.chkOriginalPictureCheckPornEnabled.Location = new System.Drawing.Point(15, 74);
            this.chkOriginalPictureCheckPornEnabled.Name = "chkOriginalPictureCheckPornEnabled";
            this.chkOriginalPictureCheckPornEnabled.Size = new System.Drawing.Size(75, 21);
            this.chkOriginalPictureCheckPornEnabled.TabIndex = 3;
            this.chkOriginalPictureCheckPornEnabled.Text = "启用鉴黄";
            this.chkOriginalPictureCheckPornEnabled.UseVisualStyleBackColor = true;
            this.chkOriginalPictureCheckPornEnabled.CheckedChanged += new System.EventHandler(this.chkOriginalPictureCheckPornEnabled_CheckedChanged);
            // 
            // pnlOriginalPictureCheckPorn
            // 
            this.pnlOriginalPictureCheckPorn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOriginalPictureCheckPorn.Controls.Add(this.pnlOriginalPictureCheckPornMessage);
            this.pnlOriginalPictureCheckPorn.Controls.Add(this.pnlOriginalPictureCheckPornEvent);
            this.pnlOriginalPictureCheckPorn.Controls.Add(this.lblOriginalPictureCheckPornEvent);
            this.pnlOriginalPictureCheckPorn.Enabled = false;
            this.pnlOriginalPictureCheckPorn.Location = new System.Drawing.Point(3, 100);
            this.pnlOriginalPictureCheckPorn.Name = "pnlOriginalPictureCheckPorn";
            this.pnlOriginalPictureCheckPorn.Size = new System.Drawing.Size(624, 103);
            this.pnlOriginalPictureCheckPorn.TabIndex = 4;
            // 
            // pnlOriginalPictureCheckPornMessage
            // 
            this.pnlOriginalPictureCheckPornMessage.Controls.Add(this.lblOriginalPictureCheckPornIllegalReply);
            this.pnlOriginalPictureCheckPornMessage.Controls.Add(this.txbOriginalPictureCheckPornErrorReply);
            this.pnlOriginalPictureCheckPornMessage.Controls.Add(this.txbOriginalPictureCheckPornIllegalReply);
            this.pnlOriginalPictureCheckPornMessage.Controls.Add(this.lblOriginalPictureCheckPornErrorReply);
            this.pnlOriginalPictureCheckPornMessage.Enabled = false;
            this.pnlOriginalPictureCheckPornMessage.Location = new System.Drawing.Point(3, 39);
            this.pnlOriginalPictureCheckPornMessage.Name = "pnlOriginalPictureCheckPornMessage";
            this.pnlOriginalPictureCheckPornMessage.Size = new System.Drawing.Size(616, 59);
            this.pnlOriginalPictureCheckPornMessage.TabIndex = 6;
            // 
            // lblOriginalPictureCheckPornIllegalReply
            // 
            this.lblOriginalPictureCheckPornIllegalReply.AutoSize = true;
            this.lblOriginalPictureCheckPornIllegalReply.Location = new System.Drawing.Point(9, 9);
            this.lblOriginalPictureCheckPornIllegalReply.Name = "lblOriginalPictureCheckPornIllegalReply";
            this.lblOriginalPictureCheckPornIllegalReply.Size = new System.Drawing.Size(107, 17);
            this.lblOriginalPictureCheckPornIllegalReply.TabIndex = 2;
            this.lblOriginalPictureCheckPornIllegalReply.Text = "鉴黄不通过回复语:";
            // 
            // txbOriginalPictureCheckPornErrorReply
            // 
            this.txbOriginalPictureCheckPornErrorReply.Location = new System.Drawing.Point(116, 35);
            this.txbOriginalPictureCheckPornErrorReply.Name = "txbOriginalPictureCheckPornErrorReply";
            this.txbOriginalPictureCheckPornErrorReply.Size = new System.Drawing.Size(497, 23);
            this.txbOriginalPictureCheckPornErrorReply.TabIndex = 5;
            // 
            // txbOriginalPictureCheckPornIllegalReply
            // 
            this.txbOriginalPictureCheckPornIllegalReply.Location = new System.Drawing.Point(116, 7);
            this.txbOriginalPictureCheckPornIllegalReply.Name = "txbOriginalPictureCheckPornIllegalReply";
            this.txbOriginalPictureCheckPornIllegalReply.Size = new System.Drawing.Size(497, 23);
            this.txbOriginalPictureCheckPornIllegalReply.TabIndex = 3;
            // 
            // lblOriginalPictureCheckPornErrorReply
            // 
            this.lblOriginalPictureCheckPornErrorReply.AutoSize = true;
            this.lblOriginalPictureCheckPornErrorReply.Location = new System.Drawing.Point(9, 38);
            this.lblOriginalPictureCheckPornErrorReply.Name = "lblOriginalPictureCheckPornErrorReply";
            this.lblOriginalPictureCheckPornErrorReply.Size = new System.Drawing.Size(95, 17);
            this.lblOriginalPictureCheckPornErrorReply.TabIndex = 4;
            this.lblOriginalPictureCheckPornErrorReply.Text = "鉴黄错误回复语:";
            // 
            // pnlOriginalPictureCheckPornEvent
            // 
            this.pnlOriginalPictureCheckPornEvent.Controls.Add(this.rdoOriginalPictureCheckPornSendByForward);
            this.pnlOriginalPictureCheckPornEvent.Controls.Add(this.rdoOriginalPictureCheckPornDoNothing);
            this.pnlOriginalPictureCheckPornEvent.Controls.Add(this.rdoOriginalPictureCheckPornReply);
            this.pnlOriginalPictureCheckPornEvent.Location = new System.Drawing.Point(119, 3);
            this.pnlOriginalPictureCheckPornEvent.Name = "pnlOriginalPictureCheckPornEvent";
            this.pnlOriginalPictureCheckPornEvent.Size = new System.Drawing.Size(500, 30);
            this.pnlOriginalPictureCheckPornEvent.TabIndex = 1;
            // 
            // rdoOriginalPictureCheckPornSendByForward
            // 
            this.rdoOriginalPictureCheckPornSendByForward.AutoSize = true;
            this.rdoOriginalPictureCheckPornSendByForward.Checked = true;
            this.rdoOriginalPictureCheckPornSendByForward.Location = new System.Drawing.Point(3, 6);
            this.rdoOriginalPictureCheckPornSendByForward.Name = "rdoOriginalPictureCheckPornSendByForward";
            this.rdoOriginalPictureCheckPornSendByForward.Size = new System.Drawing.Size(146, 21);
            this.rdoOriginalPictureCheckPornSendByForward.TabIndex = 0;
            this.rdoOriginalPictureCheckPornSendByForward.TabStop = true;
            this.rdoOriginalPictureCheckPornSendByForward.Tag = "0";
            this.rdoOriginalPictureCheckPornSendByForward.Text = "以合并转发的方式发送";
            this.rdoOriginalPictureCheckPornSendByForward.UseVisualStyleBackColor = true;
            this.rdoOriginalPictureCheckPornSendByForward.CheckedChanged += new System.EventHandler(this.rdoOriginalPictureCheckPornSendByForward_CheckedChanged);
            // 
            // rdoOriginalPictureCheckPornDoNothing
            // 
            this.rdoOriginalPictureCheckPornDoNothing.AutoSize = true;
            this.rdoOriginalPictureCheckPornDoNothing.Location = new System.Drawing.Point(172, 6);
            this.rdoOriginalPictureCheckPornDoNothing.Name = "rdoOriginalPictureCheckPornDoNothing";
            this.rdoOriginalPictureCheckPornDoNothing.Size = new System.Drawing.Size(86, 21);
            this.rdoOriginalPictureCheckPornDoNothing.TabIndex = 0;
            this.rdoOriginalPictureCheckPornDoNothing.Tag = "1";
            this.rdoOriginalPictureCheckPornDoNothing.Text = "不发送图片";
            this.rdoOriginalPictureCheckPornDoNothing.UseVisualStyleBackColor = true;
            this.rdoOriginalPictureCheckPornDoNothing.CheckedChanged += new System.EventHandler(this.rdoOriginalPictureCheckPornSendByForward_CheckedChanged);
            // 
            // rdoOriginalPictureCheckPornReply
            // 
            this.rdoOriginalPictureCheckPornReply.AutoSize = true;
            this.rdoOriginalPictureCheckPornReply.Location = new System.Drawing.Point(283, 6);
            this.rdoOriginalPictureCheckPornReply.Name = "rdoOriginalPictureCheckPornReply";
            this.rdoOriginalPictureCheckPornReply.Size = new System.Drawing.Size(98, 21);
            this.rdoOriginalPictureCheckPornReply.TabIndex = 0;
            this.rdoOriginalPictureCheckPornReply.Tag = "2";
            this.rdoOriginalPictureCheckPornReply.Text = "回复以下消息";
            this.rdoOriginalPictureCheckPornReply.UseVisualStyleBackColor = true;
            this.rdoOriginalPictureCheckPornReply.CheckedChanged += new System.EventHandler(this.rdoOriginalPictureCheckPornSendByForward_CheckedChanged);
            // 
            // lblOriginalPictureCheckPornEvent
            // 
            this.lblOriginalPictureCheckPornEvent.AutoSize = true;
            this.lblOriginalPictureCheckPornEvent.Location = new System.Drawing.Point(12, 10);
            this.lblOriginalPictureCheckPornEvent.Name = "lblOriginalPictureCheckPornEvent";
            this.lblOriginalPictureCheckPornEvent.Size = new System.Drawing.Size(83, 17);
            this.lblOriginalPictureCheckPornEvent.TabIndex = 0;
            this.lblOriginalPictureCheckPornEvent.Text = "鉴黄不通过时:";
            // 
            // lblOriginalPictureCommand
            // 
            this.lblOriginalPictureCommand.AutoSize = true;
            this.lblOriginalPictureCommand.Location = new System.Drawing.Point(12, 4);
            this.lblOriginalPictureCommand.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOriginalPictureCommand.Name = "lblOriginalPictureCommand";
            this.lblOriginalPictureCommand.Size = new System.Drawing.Size(35, 17);
            this.lblOriginalPictureCommand.TabIndex = 17;
            this.lblOriginalPictureCommand.Text = "命令:";
            // 
            // txbOriginalPictureCommand
            // 
            this.txbOriginalPictureCommand.BackColor = System.Drawing.Color.White;
            this.txbOriginalPictureCommand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbOriginalPictureCommand.Location = new System.Drawing.Point(122, 4);
            this.txbOriginalPictureCommand.Margin = new System.Windows.Forms.Padding(4);
            this.txbOriginalPictureCommand.Name = "txbOriginalPictureCommand";
            this.txbOriginalPictureCommand.Size = new System.Drawing.Size(498, 23);
            this.txbOriginalPictureCommand.TabIndex = 18;
            // 
            // lblOriginalPictureDownloadingReply
            // 
            this.lblOriginalPictureDownloadingReply.AutoSize = true;
            this.lblOriginalPictureDownloadingReply.Location = new System.Drawing.Point(12, 34);
            this.lblOriginalPictureDownloadingReply.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOriginalPictureDownloadingReply.Name = "lblOriginalPictureDownloadingReply";
            this.lblOriginalPictureDownloadingReply.Size = new System.Drawing.Size(95, 17);
            this.lblOriginalPictureDownloadingReply.TabIndex = 19;
            this.lblOriginalPictureDownloadingReply.Text = "开始下载回复语:";
            // 
            // txbOriginalPictureDownloadingReply
            // 
            this.txbOriginalPictureDownloadingReply.BackColor = System.Drawing.Color.White;
            this.txbOriginalPictureDownloadingReply.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbOriginalPictureDownloadingReply.Location = new System.Drawing.Point(122, 34);
            this.txbOriginalPictureDownloadingReply.Margin = new System.Windows.Forms.Padding(4);
            this.txbOriginalPictureDownloadingReply.Name = "txbOriginalPictureDownloadingReply";
            this.txbOriginalPictureDownloadingReply.Size = new System.Drawing.Size(498, 23);
            this.txbOriginalPictureDownloadingReply.TabIndex = 20;
            // 
            // CtrlOriginalPicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblOriginalPictureDownloadingReply);
            this.Controls.Add(this.txbOriginalPictureDownloadingReply);
            this.Controls.Add(this.lblOriginalPictureCommand);
            this.Controls.Add(this.txbOriginalPictureCommand);
            this.Controls.Add(this.chkOriginalPictureCheckPornEnabled);
            this.Controls.Add(this.pnlOriginalPictureCheckPorn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CtrlOriginalPicture";
            this.Size = new System.Drawing.Size(630, 642);
            this.pnlOriginalPictureCheckPorn.ResumeLayout(false);
            this.pnlOriginalPictureCheckPorn.PerformLayout();
            this.pnlOriginalPictureCheckPornMessage.ResumeLayout(false);
            this.pnlOriginalPictureCheckPornMessage.PerformLayout();
            this.pnlOriginalPictureCheckPornEvent.ResumeLayout(false);
            this.pnlOriginalPictureCheckPornEvent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox chkOriginalPictureCheckPornEnabled;
        private Panel pnlOriginalPictureCheckPorn;
        private Panel pnlOriginalPictureCheckPornMessage;
        private Label lblOriginalPictureCheckPornIllegalReply;
        private TextBox txbOriginalPictureCheckPornErrorReply;
        private TextBox txbOriginalPictureCheckPornIllegalReply;
        private Label lblOriginalPictureCheckPornErrorReply;
        private Panel pnlOriginalPictureCheckPornEvent;
        private RadioButton rdoOriginalPictureCheckPornSendByForward;
        private RadioButton rdoOriginalPictureCheckPornDoNothing;
        private RadioButton rdoOriginalPictureCheckPornReply;
        private Label lblOriginalPictureCheckPornEvent;
        private Label lblOriginalPictureCommand;
        private TextBox txbOriginalPictureCommand;
        private Label lblOriginalPictureDownloadingReply;
        private TextBox txbOriginalPictureDownloadingReply;
    }
}
