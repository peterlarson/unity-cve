// automatically generated, do not modify

namespace Unity_CVE
{

using System;
using FlatBuffers;

public sealed class Location : Table {
  public static Location GetRootAsLocation(ByteBuffer _bb) { return GetRootAsLocation(_bb, new Location()); }
  public static Location GetRootAsLocation(ByteBuffer _bb, Location obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Location __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public float X { get { int o = __offset(4); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Y { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }
  public float Z { get { int o = __offset(8); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0f; } }

  public static Offset<Location> CreateLocation(FlatBufferBuilder builder,
      float x = 0f,
      float y = 0f,
      float z = 0f) {
    builder.StartObject(3);
    Location.AddZ(builder, z);
    Location.AddY(builder, y);
    Location.AddX(builder, x);
    return Location.EndLocation(builder);
  }

  public static void StartLocation(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddX(FlatBufferBuilder builder, float x) { builder.AddFloat(0, x, 0f); }
  public static void AddY(FlatBufferBuilder builder, float y) { builder.AddFloat(1, y, 0f); }
  public static void AddZ(FlatBufferBuilder builder, float z) { builder.AddFloat(2, z, 0f); }
  public static Offset<Location> EndLocation(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Location>(o);
  }
};


}
