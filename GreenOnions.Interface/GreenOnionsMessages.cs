﻿namespace GreenOnions.Interface
{
    /// <summary>
    /// 表示一条消息的组
    /// 如果是一组合并转发消息, 则是多条消息, 否则是一条消息(一条消息中包含文字消息, 图片消息, @消息等)
    /// </summary>
    public class GreenOnionsMessages : List<GreenOnionsBaseMessage>
    {
        public static implicit operator GreenOnionsMessages(GreenOnionsBaseMessage onceMsg)
        {
            return new GreenOnionsMessages(onceMsg);
        }
        public static implicit operator GreenOnionsMessages(GreenOnionsBaseMessage[] arrMsg)
        {
            return new GreenOnionsMessages(arrMsg);
        }
        public static implicit operator GreenOnionsMessages(string text)
        {
            return new GreenOnionsTextMessage(text);
        }
        /// <summary>
        /// 空消息组
        /// </summary>
        public GreenOnionsMessages()
        {

        }
        /// <summary>
        /// 消息组中包含一条且只有一种类型的消息
        /// </summary>
        /// <param name="messages"></param>
        public GreenOnionsMessages(GreenOnionsBaseMessage messages)
        {
            if (messages != null)
                Add(messages);
        }
        /// <summary>
        /// 消息组中包含一条多种类型的
        /// </summary>
        /// <param name="messages"></param>
        public GreenOnionsMessages(IEnumerable<GreenOnionsBaseMessage> messages)
        {
            if (messages != null)
                AddRange(messages);
        }
        /// <summary>
        /// [接收消息时]QQ消息的ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// [发送消息时]是否以回复的方式发送消息
        /// </summary>
        public bool Reply { get; set; } = true;
        /// <summary>
        /// [发送消息时]指定回复引用的消息ID，null则会自动处理
        /// </summary>
        public int? ReplyId { get; set; } = null;
        /// <summary>
        /// [发送消息时]撤回时间, 单位:秒, 为0时不撤回, 超过120会撤回失败
        /// </summary>
        public int RevokeTime { get; set; } = 0;
        /// <summary>
        /// [接收消息时]发送者的QQ号
        /// </summary>
        public long SenderId { get; set; }
        /// <summary>
        /// [接收消息时]发送者的昵称或群名片
        /// </summary>
        public string? SenderName { get; set; }
        /// <summary>
        /// 是否为葱葱命令消息（true时，不会进行葱葱预定义标签替换）
        /// </summary>
        public bool IsGreenOnionsCommand { get; set; } = false;
    }
}
