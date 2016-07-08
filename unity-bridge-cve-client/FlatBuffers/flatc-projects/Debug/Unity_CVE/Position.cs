// automatically generated, do not modify

namespace Unity_CVE
{

using System;
using FlatBuffers;

public sealed class Position : Table {
  public static Position GetRootAsPosition(ByteBuffer _bb) { return GetRootAsPosition(_bb, new Position()); }
  public static Position GetRootAsPosition(ByteBuffer _bb, Position obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Position __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public float X { get { int o = __offset(4); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Y { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Z { get { int o = __offset(8); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Roll { get { int o = __offset(10); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Pitch { get { int o = __offset(12); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Yaw { get { int o = __offset(14); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }

  public static Offset<Position> CreatePosition(FlatBufferBuilder builder,
      float x = 0f,
      float y = 0f,
      float z = 0f,
      float roll = 0f,
      float pitch = 0f,
      float yaw = 0f) {
    builder.StartObject(6);
    Position.AddYaw(builder, yaw);
    Position.AddPitch(builder, pitch);
    Position.AddRoll(builder, roll);
    Position.AddZ(builder, z);
    Position.AddY(builder, y);
    Position.AddX(builder, x);
    return Position.EndPosition(builder);
  }

  public static void StartPosition(FlatBufferBuilder builder) { builder.StartObject(6); }
  public static void AddX(FlatBufferBuilder builder, float x) { builder.AddFloat(0, x, 0f); }
  public static void AddY(FlatBufferBuilder builder, float y) { builder.AddFloat(1, y, 0f); }
  public static void AddZ(FlatBufferBuilder builder, float z) { builder.AddFloat(2, z, 0f); }
  public static void AddRoll(FlatBufferBuilder builder, float roll) { builder.AddFloat(3, roll, 0f); }
  public static void AddPitch(FlatBufferBuilder builder, float pitch) { builder.AddFloat(4, pitch, 0f); }
  public static void AddYaw(FlatBufferBuilder builder, float yaw) { builder.AddFloat(5, yaw, 0f); }
  public static Offset<Position> EndPosition(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Position>(o);
  }
};


}
