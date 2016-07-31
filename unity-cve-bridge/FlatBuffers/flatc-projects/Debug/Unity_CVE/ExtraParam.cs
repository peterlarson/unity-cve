// automatically generated, do not modify

namespace Unity_CVE
{

using System;
using FlatBuffers;

public sealed class ExtraParam : Table {
  public static ExtraParam GetRootAsExtraParam(ByteBuffer _bb) { return GetRootAsExtraParam(_bb, new ExtraParam()); }
  public static ExtraParam GetRootAsExtraParam(ByteBuffer _bb, ExtraParam obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public ExtraParam __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Name { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __vector_as_arraysegment(4); }
  public float Value { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }

  public static Offset<ExtraParam> CreateExtraParam(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset),
      float value = 0f) {
    builder.StartObject(2);
    ExtraParam.AddValue(builder, value);
    ExtraParam.AddName(builder, nameOffset);
    return ExtraParam.EndExtraParam(builder);
  }

  public static void StartExtraParam(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddValue(FlatBufferBuilder builder, float value) { builder.AddFloat(1, value, 0f); }
  public static Offset<ExtraParam> EndExtraParam(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<ExtraParam>(o);
  }
};


}
