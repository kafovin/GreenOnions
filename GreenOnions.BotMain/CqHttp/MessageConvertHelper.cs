﻿using GreenOnions.Interface;
using GreenOnions.Utility.Helper;
using Sora.Entities;
using Sora.Entities.Base;
using Sora.Entities.Info;
using Sora.Entities.Segment;
using Sora.Entities.Segment.DataModel;

namespace GreenOnions.BotMain.CqHttp
{
    public static class MessageConvertHelper
    {
        public static async Task<GreenOnionsMessages> ToOnionsMessages(this MessageBody miraiMessage, long senderId, string senderName, long? senderGroup, SoraApi api)
        {
            GreenOnionsMessages greenOnionsMessages = new GreenOnionsMessages();
            for (int i = 0; i < miraiMessage.Count; i++)
            {
                try
                {
                    if (miraiMessage[i].Data is AtSegment atMsg)
                    {
                        //获取@群名片
                        if (long.TryParse(atMsg.Target, out long atId))
                        {
                            var apiResult = await api.GetGroupMemberList(senderGroup.Value);
                            List<GroupMemberInfo> groupMemberInfos = apiResult.groupMemberList;
                            GroupMemberInfo targetQQ = groupMemberInfos.Where(m => m.UserId == atId).FirstOrDefault();
                            string nickName = targetQQ?.Nick;
                            greenOnionsMessages.Add(new GreenOnionsAtMessage(atId, nickName));
                        }
                        else
                        {
                            greenOnionsMessages.Add(new GreenOnionsAtMessage(atId, atMsg.Name));
                        }
                    }
                    else if (miraiMessage[i].Data is TextSegment textMsg)
                        greenOnionsMessages.Add(textMsg.Content);
                    else if (miraiMessage[i].Data is ImageSegment imageMsg)
                        greenOnionsMessages.Add(new GreenOnionsImageMessage(imageMsg.Url, imageMsg.ImgFile));
                    else if (miraiMessage[i].Data is FaceSegment faceMsg)
                        greenOnionsMessages.Add(new GreenOnionsFaceMessage(faceMsg.Id, faceMsg.ToString()));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLogWithUserMessage($"转换为GreenOnions消息失败, 原消息类型为:{miraiMessage[i].Data.GetType()}", ex);
                }
            }

            greenOnionsMessages.SenderId = senderId;
            greenOnionsMessages.SenderName = senderName;
            return greenOnionsMessages;
        }

        public static MessageBody ToCqHttpMessages(this GreenOnionsMessages greenOnionsMessage, int? RelpyId)
        {
            MessageBody cqHttpMessages = new MessageBody();
            if (greenOnionsMessage.Reply && RelpyId != null)
                cqHttpMessages.Add(SoraSegment.Reply(RelpyId.Value));

            for (int i = 0; i < greenOnionsMessage.Count; i++)
            {
                try
                {
                    if (greenOnionsMessage[i] is GreenOnionsTextMessage txtMsg)
                    {
                        cqHttpMessages.Add(SoraSegment.Text(txtMsg.Text));
                    }
                    else if (greenOnionsMessage[i] is GreenOnionsImageMessage imgMsg)
                    {
                        string data = string.IsNullOrEmpty(imgMsg.Url) ? ("base64://" + imgMsg.Base64Str) : imgMsg.Url;
                        cqHttpMessages.Add(SoraSegment.Image(data));
                    }
                    else if (greenOnionsMessage[i] is GreenOnionsAtMessage atMsg)
                    {
                        if (atMsg.AtId == -1)
                            cqHttpMessages.Add(SoraSegment.AtAll());
                        else
                            cqHttpMessages.Add(SoraSegment.At(atMsg.AtId));
                    }
                    else if (greenOnionsMessage[i] is GreenOnionsForwardMessage forwardMsg)
                    {
                        for (int j = 0; j < forwardMsg.ItemMessages.Count; j++)
                        {
                            var itemMsg = ToCqHttpMessages(forwardMsg.ItemMessages[i].itemMessage, RelpyId);
                            if (itemMsg!= null)
                                cqHttpMessages.AddRange(itemMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLogWithUserMessage("转换为CqHttp消息失败!!!", ex);
                    continue;
                }
                
            }
            return cqHttpMessages;
        }

        public static List<CustomNode> ToCqHttpForwardMessage(this GreenOnionsMessages msgs)
        {
            List<CustomNode> nodes = new List<CustomNode>();
            for (int i = 0; i < msgs.Count; i++)
            {
                if (msgs[i] is GreenOnionsForwardMessage forwardMsg)
                {
                    for (int j = 0; j < forwardMsg.ItemMessages.Count; j++)
                    {
                        nodes.Add(new CustomNode(forwardMsg.ItemMessages[j].NickName, forwardMsg.ItemMessages[j].QQid, forwardMsg.ItemMessages[j].itemMessage.ToCqHttpMessages(null)));
                    }
                }
            }
            return nodes;
        }
    }
}
