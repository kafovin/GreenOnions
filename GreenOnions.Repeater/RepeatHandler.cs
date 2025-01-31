﻿using GreenOnions.Interface;
using GreenOnions.Utility;
using GreenOnions.Utility.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GreenOnions.Repeater
{
    public static class RepeatHandler
    {
        private static Dictionary<long, MessageItem> MessageItems = new Dictionary<long, MessageItem>();

        public async static Task<GreenOnionsBaseMessage> Repeating(GreenOnionsBaseMessage message, long groupId)
        {
            MessageItem tempMessageItem;
            if (message is GreenOnionsImageMessage imgMsg)
            {
                using (MemoryStream ms = await HttpHelper.DownloadImageAsMemoryStreamAsync(imgMsg.Url))
                    tempMessageItem = new MessageItem(imgMsg.GetType(), ms.ToBase64());
            }
            else if (message is GreenOnionsTextMessage txtMsg)
                tempMessageItem = new MessageItem(txtMsg.GetType(), txtMsg.Text);
            else
                return null;
            if (MessageItems.ContainsKey(groupId))
            {
                if (tempMessageItem == MessageItems[groupId])
                {
                    MessageItems[groupId].RepeatedCount++;
                    tempMessageItem = MessageItems[groupId];
                }
                else
                    MessageItems[groupId] = tempMessageItem;
            }
            else
                MessageItems.Add(groupId, tempMessageItem);

            if (!tempMessageItem.IsRepeated)  //已经参与过复读的消息不再参与随机复读
            {
                if (BotInfo.Config.SuccessiveRepeatEnabled)
                {
                    GreenOnionsBaseMessage resultMessage = await SuccessiveRepeat(message, tempMessageItem);
                    if (resultMessage is not null)
                        return resultMessage;
                }
                if (BotInfo.Config.RandomRepeatEnabled)
                {
                    GreenOnionsBaseMessage resultMessage = await RandomRepeat(message, tempMessageItem);
                    if (resultMessage is not null)
                        return resultMessage;
                }
            }
            return null;
        }

        /// <summary>
        /// 随机复读
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="messageItem">消息记录</param>
        /// <param name="UploadPicture">上传图片</param>
        /// <returns></returns>
        private static async Task<GreenOnionsBaseMessage> RandomRepeat(GreenOnionsBaseMessage message, MessageItem messageItem)
        {
            if (new Random(Guid.NewGuid().GetHashCode()).Next(1, 101) <= BotInfo.Config.RandomRepeatProbability)
                return await Pepeat(message, messageItem);
            return null;
        }

        /// <summary>
        /// 连续复读
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="messageItem">消息记录</param>
        /// <param name="UploadPicture">上传图片</param>
        /// <returns></returns>
        private static async Task<GreenOnionsBaseMessage> SuccessiveRepeat(GreenOnionsBaseMessage message, MessageItem messageItem)
        {
            if (messageItem.RepeatedCount >= BotInfo.Config.SuccessiveRepeatCount)
                return await Pepeat(message, messageItem);
            return null;
        }

        private static async Task<GreenOnionsBaseMessage> Pepeat(GreenOnionsBaseMessage message, MessageItem messageItem)
        {
            if (message is GreenOnionsTextMessage)
            {
                messageItem.IsRepeated = true;
                string msg = message.ToString();
                if (BotInfo.Config.ReplaceMeToYou)
                    msg = msg.Replace("我", "你");
                return msg;
            }
            else if (message is GreenOnionsImageMessage imageMessage)
            {
                messageItem.IsRepeated = true;
                MemoryStream ms = await MirrorImage(ImageHelper.ReplaceGroupUrl(imageMessage.Url));
                if (ms is not null)
                    return new GreenOnionsImageMessage(ms);
                else
                    return new GreenOnionsImageMessage(imageMessage.Url);
            }
            return null;
        }

        private static async Task<MemoryStream> MirrorImage(string url)
        {
            bool bRewind = false;
            bool bHorizontalMirror = false;
            bool bVerticalMirror = false;
            if (BotInfo.Config.RewindGifEnabled)
                bRewind = new Random(Guid.NewGuid().GetHashCode()).Next(1, 101) < BotInfo.Config.RewindGifProbability;
            if (!bRewind)
            {
                if (BotInfo.Config.HorizontalMirrorImageEnabled)
                    bHorizontalMirror = new Random(Guid.NewGuid().GetHashCode()).Next(1, 101) < BotInfo.Config.HorizontalMirrorImageProbability;
                if (BotInfo.Config.VerticalMirrorImageEnabled)
                    bVerticalMirror = new Random(Guid.NewGuid().GetHashCode()).Next(1, 101) < BotInfo.Config.VerticalMirrorImageProbability;
            }
            
            if (bRewind || bHorizontalMirror || bVerticalMirror)
            {
                MemoryStream ms = await HttpHelper.DownloadImageAsMemoryStreamAsync(url);

                if (ms is not null)
                {
                    try
                    {
                        //倒放和镜像不会同时发生且倒放优先级高于镜像, 但水平镜像和垂直镜像可能同时发生
                        if (bRewind)
                            ms = ms.RewindGifStream();
                        if (bHorizontalMirror)
                            ms = ms.HorizontalMirrorImageStream();
                        if (bVerticalMirror)
                            ms = ms.VerticalMirrorImageStream();
                        return ms;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteErrorLogWithUserMessage("镜像图片失败", ex);
                    }
                }
            }
            return null;
        }

        private class MessageItem
        {
            public readonly Type MessageType;
            public readonly string MessageValue;
            public int RepeatedCount = 1;
            public bool IsRepeated = false;

            public MessageItem(Type messageType, string messageValue)
            {
                MessageType = messageType;
                MessageValue = messageValue;
            }

            public static bool operator ==(MessageItem left, MessageItem right)
            {
                return left.MessageType == right.MessageType && left.MessageValue == right.MessageValue;
            }

            public static bool operator !=(MessageItem left, MessageItem right)
            {
                return left.MessageType != right.MessageType || left.MessageValue != right.MessageValue;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) 
                    return false;
                if (ReferenceEquals(this, obj)) 
                    return true;
                if (obj.GetType() != GetType()) 
                    return false;
                MessageItem other = (MessageItem)obj;
                if (MessageValue == other.MessageValue)
                    return true;
                return false;
            }

            public override int GetHashCode()
            {
                if (MessageType == typeof(GreenOnionsTextMessage))  //只有文字消息和图片消息才复读
                    return StringComparer.InvariantCulture.GetHashCode(MessageValue);
                return 0;
            }
        }
    }
}
