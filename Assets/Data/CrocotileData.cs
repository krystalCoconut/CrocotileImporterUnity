using System;
using System.Collections.Generic;

[Serializable]
public class CrocotileData
{
    public Config config { get; set; }
    public TilePalette tilePalette { get; set; }
    public UvAnimation uvAnimation { get; set; }
    public List<Model> model { get; set; }
    public List<Prefab> prefabs { get; set; }
    public List<object> acts { get; set; }
    
  // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Billboard
    {
        public bool enabled { get; set; }
        public bool y { get; set; }
    }

    public class Bloom
    {
        public bool enabled { get; set; }
        public int order { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int strength { get; set; }
        public int radius { get; set; }
        public double threshold { get; set; }
    }

    public class Bokeh
    {
        public bool enabled { get; set; }
        public int order { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int focus { get; set; }
        public double aperture { get; set; }
        public double maxblur { get; set; }
    }

    public class Camera
    {
        public Perspective perspective { get; set; }
        public Orthographic orthographic { get; set; }
        public string cameraType { get; set; }
        public List<double> target { get; set; }
        public float cameraZoom { get; set; }
        public double zoom { get; set; }
    }

    public class Color
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
    }

    public class Config
    {
        public int tilesizeX { get; set; }
        public int tilesizeY { get; set; }
        public string skybox { get; set; }
        public string skyboxCylinder { get; set; }
        public string skyboxSphere { get; set; }
        public string skyboxShape { get; set; }
        public bool showSkybox { get; set; }
        public string backgroundColor { get; set; }
        public int baseUnit { get; set; }
        public Camera camera { get; set; }
        public Effects effects { get; set; }
    }

    public class Effects
    {
        public Fog fog { get; set; }
        public Bloom bloom { get; set; }
        public Bokeh bokeh { get; set; }
    }

    public class Fog
    {
        public bool enabled { get; set; }
        public int order { get; set; }
        public string color { get; set; }
        public double near { get; set; }
        public int far { get; set; }
    }

    public class ImgCustom
    {
        public bool uniqueUVInput { get; set; }
        public int uvSizeX { get; set; }
        public int uvSizeY { get; set; }
        public int uvPaddingLeft { get; set; }
        public int uvPaddingRight { get; set; }
        public int uvPaddingTop { get; set; }
        public int uvPaddingBottom { get; set; }
    }

    public class ImgDecal
    {
        public bool enabled { get; set; }
        public float offset { get; set; }
    }

    public class ImgFile
    {
        public string path { get; set; }
        public string name { get; set; }
    }

    public class Instance
    {
        public string name { get; set; }
        public int id { get; set; }
        public int parentID { get; set; }
        public int childIndex { get; set; }
        public bool collapsed { get; set; }
        public Position position { get; set; }
        public Rotation rotation { get; set; }
        public Scale scale { get; set; }
        public Properties properties { get; set; }
    }

    public class Lines
    {
        public bool enabled { get; set; }
    }

    public class Material
    {
        public string type { get; set; }
        public int color { get; set; }
        public int side { get; set; }
        public int shadowSide { get; set; }
        public bool transparent { get; set; }
    }

    public class Mirror
    {
        public bool enabled { get; set; }
        public string axis { get; set; }
    }

    public class Model
    {
        public Material material { get; set; }
        public string texture { get; set; }
        public ImgFile imgFile { get; set; }
        public string imgWrap { get; set; }
        public int imgDoubleSided { get; set; }
        public ImgDecal imgDecal { get; set; }
        public bool imgTransparent { get; set; }
        public ImgCustom imgCustom { get; set; }
        public List<Object> @object { get; set; }
    }

    public class Object
    {
        public Position position { get; set; }
        public List<Vertex> vertices { get; set; }
        public List<List<int>> faces { get; set; }
        public Uv[][] uvs { get; set; }
        public List<Color> colors { get; set; }
        public List<VertexBone> vertexBone { get; set; }
        public int texture { get; set; }
    }

    public class Uv
    {
        public float x { get; set; }
        public float y { get; set; }
    }

    public class Orthographic
    {
        public List<double> position { get; set; }
        public List<double> quaternion { get; set; }
        public double zoom { get; set; }
        public double near { get; set; }
        public int far { get; set; }
        public double left { get; set; }
        public double right { get; set; }
        public double top { get; set; }
        public double bottom { get; set; }
    }

    public class Perspective
    {
        public List<double> position { get; set; }
        public List<double> quaternion { get; set; }
        public int zoom { get; set; }
        public double near { get; set; }
        public int far { get; set; }
        public double aspect { get; set; }
        public int fov { get; set; }
    }

    public class Point
    {
        public string name { get; set; }
        public Pos pos { get; set; }
    }

    public class Pos
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class Prefab
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<object> content { get; set; }
        public bool collapsed { get; set; }
        public bool visible { get; set; }
        public bool locked { get; set; }
        public string num { get; set; }
        public List<Object> @object { get; set; }
        public List<Instance> instances { get; set; }
        public List<Point> points { get; set; }
        public int? skeletonRoot { get; set; }
        public bool? rootVisible { get; set; }
        public bool? rootLocked { get; set; }
        public bool? rootCollapsed { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public Billboard billboard { get; set; }
        public Mirror mirror { get; set; }
        public Lines lines { get; set; }
        public Shadows shadows { get; set; }
        public Skin skin { get; set; }
        public List<CustomData> custom { get; set; }
        public List<List<int>> boneInverses { get; set; }
    }

    public class CustomData
    {
        public string name;
        public string uuid;
        public string type;
        public string valueType;
        public string value;
    }

    public class Rotation
    {
        public float _x { get; set; }
        public float _y { get; set; }
        public float _z { get; set; }
        public string _order { get; set; }
    }

    public class Scale
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class Shadows
    {
        public bool castShadow { get; set; }
        public bool receiveShadow { get; set; }
    }

    public class Skin
    {
        public bool enabled { get; set; }
    }

    public class TilePalette
    {
        public List<object> set { get; set; }
    }

    public class UvAnimation
    {
        public List<object> action { get; set; }
    }

    public class Vertex
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class VertexBone
    {
        public List<int> id { get; set; }
        public List<int> weight { get; set; }
    }



}