// automatically generated, do not modify

namespace Unity_CVE
{

using System;
using FlatBuffers;

public sealed class Orientation : Table {
  public static Orientation GetRootAsOrientation(ByteBuffer _bb) { return GetRootAsOrientation(_bb, new Orientation()); }
  public static Orientation GetRootAsOrientation(ByteBuffer _bb, Orientation obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Orientation __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public float Roll { get { int o = __offset(4); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Pitch { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Yaw { get { int o = __offset(8); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }

  public static Offset<Orientation> CreateOrientation(FlatBufferBuilder builder,
      float roll = 0f,
      float pitch = 0f,
      float yaw = 0f) {
    builder.StartObject(3);
    Orientation.AddYaw(builder, yaw);
    Orientation.AddPitch(builder, pitch);
    Orientation.AddRoll(builder, roll);
    return Orientation.EndOrientation(builder);
  }

  public static void StartOrientation(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddRoll(FlatBufferBuilder builder, float roll) { builder.AddFloat(0, roll, 0f); }
  public static void AddPitch(FlatBufferBuilder builder, float pitch) { builder.AddFloat(1, pitch, 0f); }
  public static void AddYaw(FlatBufferBuilder builder, float yaw) { builder.AddFloat(2, yaw, 0f); }
  public static Offset<Orientation> EndOrientation(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Orientation>(o);
  }
};


}
