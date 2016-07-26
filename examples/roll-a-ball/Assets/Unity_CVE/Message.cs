// automatically generated, do not modify

namespace Unity_CVE
{

using System;
using FlatBuffers;

public sealed class Message : Table {
  public static Message GetRootAsMessage(ByteBuffer _bb) { return GetRootAsMessage(_bb, new Message()); }
  public static Message GetRootAsMessage(ByteBuffer _bb, Message obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Message __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public Data MessageDataType { get { int o = __offset(4); return o != 0 ? (Data)bb.Get(o + bb_pos) : Data.NONE; } }
  public TTable GetMessageData<TTable>(TTable obj) where TTable : Table { int o = __offset(6); return o != 0 ? __union(obj, o) : null; }
  public string Channel { get { int o = __offset(8); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetChannelBytes() { return __vector_as_arraysegment(8); }

  public static Offset<Message> CreateMessage(FlatBufferBuilder builder,
      Data messageData_type = Data.NONE,
      int messageDataOffset = 0,
      StringOffset channelOffset = default(StringOffset)) {
    builder.StartObject(3);
    Message.AddChannel(builder, channelOffset);
    Message.AddMessageData(builder, messageDataOffset);
    Message.AddMessageDataType(builder, messageData_type);
    return Message.EndMessage(builder);
  }

  public static void StartMessage(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddMessageDataType(FlatBufferBuilder builder, Data messageDataType) { builder.AddByte(0, (byte)messageDataType, 0); }
  public static void AddMessageData(FlatBufferBuilder builder, int messageDataOffset) { builder.AddOffset(1, messageDataOffset, 0); }
  public static void AddChannel(FlatBufferBuilder builder, StringOffset channelOffset) { builder.AddOffset(2, channelOffset.Value, 0); }
  public static Offset<Message> EndMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Message>(o);
  }
};


}
