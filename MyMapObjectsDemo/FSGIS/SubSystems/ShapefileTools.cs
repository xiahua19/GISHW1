using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using MyMapObjects;

namespace FSGIS.SubSystems
{
    internal class ShapefileTools
    {
        public static moMapLayer ReadShapefile(string filePath)
        {
            // Ensure the .dbf file exists, as ShapefileDataReader relies on it
            string dbfPath = Path.ChangeExtension(filePath, ".dbf");
            if (!File.Exists(dbfPath))
            {
                throw new FileNotFoundException("The associated .dbf file is missing.", dbfPath);
            }

            var geometryFactory = new GeometryFactory();
            moGeometryTypeConstant geoType = moGeometryTypeConstant.NotDefined;
            moMapLayer sMapLayer = new moMapLayer();    
            using (var shapeDataReader = new ShapefileDataReader(filePath, geometryFactory))
            {
                
                DbaseFileHeader header = shapeDataReader.DbaseHeader;
                geoType = GetInternalGeometryTypeFromPackageType(shapeDataReader.ShapeHeader.ShapeType);
                IList<Feature> records = new List<Feature>();

                // Construct fields
                Int32 sFieldCount = shapeDataReader.FieldCount - 1; //字段数量
                MyMapObjects.moFields sFields = new MyMapObjects.moFields();
                for (Int32 i = 0; i <= sFieldCount - 1; i++)
                {
                    string sName = header.Fields[i].Name;
                    MyMapObjects.moValueTypeConstant sValueType = GetInternalTypeFromSystemType(header.Fields[i].Type);
                    MyMapObjects.moField sField = new MyMapObjects.moField(sName, sValueType);
                    sFields.Append(sField);
                }

                // Read all the features from the shapefile
                MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
                while (shapeDataReader.Read())
                {
                    IAttributesTable attributesTable = new AttributesTable();
                    object[] values = new object[shapeDataReader.FieldCount - 1];

                    // Read all attributes except the first (geometry)
                    MyMapObjects.moAttributes sAttributes = new MyMapObjects.moAttributes();
                    for (int i = 0; i < shapeDataReader.FieldCount - 1; i++)
                    {
                        string name = shapeDataReader.GetName(i + 1);
                        object value = shapeDataReader.GetValue(i + 1);
                        if (sFields.GetItem(i).ValueType == moValueTypeConstant.dInt16) value = Convert.ToInt16(value);
                        if (sFields.GetItem(i).ValueType == moValueTypeConstant.dInt32) value = Convert.ToInt32(value);
                        if (sFields.GetItem(i).ValueType == moValueTypeConstant.dInt64) value = Convert.ToInt64(value);
                        if (sFields.GetItem(i).ValueType == moValueTypeConstant.dSingle) value = Convert.ToSingle(value);
                        if (sFields.GetItem(i).ValueType == moValueTypeConstant.dDouble) value = Convert.ToDouble(value);
                        if (sFields.GetItem(i).ValueType == moValueTypeConstant.dText) value = Convert.ToString(value);
                        sAttributes.Append(value);
                    }

                    // Read the geometry for the current feature
                    Geometry geometry = shapeDataReader.Geometry;
                    moGeometry sGeometry = LoadGeometry(geometry);
           
                    // Create a feature from the geometry and attributes
                    MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature(geoType, sGeometry, sAttributes);
                    sFeatures.Add(sFeature);
                    Feature feature = new Feature(geometry, attributesTable);
                    records.Add(feature);

                }
                sMapLayer = new moMapLayer(Path.GetFileNameWithoutExtension(filePath), geoType, sFields, filePath);
                sMapLayer.Features = sFeatures;

                
                // Do something with the list of features, like processing them or displaying them on a map.
            }

            return sMapLayer;
        }




        private static moGeometry LoadGeometry(Geometry geometry)
        {
            if (geometry is Polygon polygon)
            {
                return LoadMultiPolygon(polygon);
            }
            return null;

        }


        private static MyMapObjects.moMultiPolygon LoadMultiPolygon(Polygon polygon)
        {
            MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon();
            Int32 sPartCount = polygon.NumGeometries;
            for (Int32 i = 0; i <= sPartCount - 1; i++)
            {
                var singlePolygon = polygon.GetGeometryN(i);
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                foreach (var coord in singlePolygon.Coordinates)
                {
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(coord.X, coord.Y);
                    sPoints.Add(sPoint);
                }
                sMultiPolygon.Parts.Add(sPoints);
            }
            sMultiPolygon.UpdateExtent();
            return sMultiPolygon;
        }

        /// <summary>
        /// 从系统的Type类型转换为程序的dType类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static MyMapObjects.moValueTypeConstant GetInternalTypeFromSystemType(Type type)
        {
            if (type == Type.GetType("System.Int16")) { return MyMapObjects.moValueTypeConstant.dInt16; }
            else if (type == Type.GetType("System.Int32")) { return MyMapObjects.moValueTypeConstant.dInt32; }
            else if (type == Type.GetType("System.Int64")) { return MyMapObjects.moValueTypeConstant.dInt64; }
            else if (type == Type.GetType("System.Single")) { return MyMapObjects.moValueTypeConstant.dSingle; }
            else if (type == Type.GetType("System.Double")) { return MyMapObjects.moValueTypeConstant.dDouble; }
            else if (type == Type.GetType("System.String")) { return MyMapObjects.moValueTypeConstant.dText; }
            return MyMapObjects.moValueTypeConstant.dText;
        }


        private static MyMapObjects.moGeometryTypeConstant GetInternalGeometryTypeFromPackageType(ShapeGeometryType type)
        {
            if (type == ShapeGeometryType.Polygon) { return moGeometryTypeConstant.MultiPolygon; }
            //else if (type == Type.GetType("System.Int32")) { return MyMapObjects.moValueTypeConstant.dInt32; }
            //else if (type == Type.GetType("System.Int64")) { return MyMapObjects.moValueTypeConstant.dInt64; }
            //else if (type == Type.GetType("System.Single")) { return MyMapObjects.moValueTypeConstant.dSingle; }
            //else if (type == Type.GetType("System.Double")) { return MyMapObjects.moValueTypeConstant.dDouble; }
            //else if (type == Type.GetType("System.String")) { return MyMapObjects.moValueTypeConstant.dText; }
            return moGeometryTypeConstant.NotDefined;
        }


        public static void WriteToShapefile(moMapLayer sLayer, string filePath)
        {
            // Construct Features
            FeatureCollection features = new FeatureCollection(); 
            for(Int32 i = 0;i<sLayer.Features.Count;i++)
            {
                var attributesTable = new AttributesTable();
                for(Int32 j = 0;j < sLayer.AttributeFields.Count; ++j)
                {
                    attributesTable.Add(sLayer.AttributeFields.GetItem(j).Name, sLayer.Features.GetItem(i).Attributes.GetItem(j));
                }
                Feature feature = new Feature(FromInternalTypeToOuterType(sLayer.Features.GetItem(i).Geometry, moGeometryTypeConstant.MultiPolygon), attributesTable);
                features.Add(feature);
            }


            var geometryFactory = new GeometryFactory();

            // Create a ShapefileDataWriter object with the output path
            var writer = new ShapefileDataWriter(filePath, geometryFactory)
            {
                // Define the header based on the features you are writing
                Header = ShapefileDataWriter.GetHeader(features[0], features.Count)
            };



            // Write the shapefile using the features and the attributes schema
            FeatureCollection fc = new FeatureCollection();
            
            writer.Write(features);
        }

        private static Geometry FromInternalTypeToOuterType(moGeometry g, moGeometryTypeConstant gType)
        {
            if(gType == moGeometryTypeConstant.MultiPolygon)
            {
                moMultiPolygon sPolygon= (moMultiPolygon)g;
                List<Coordinate> points = new List<Coordinate>();
                
                moPoints sPoints = sPolygon.Parts.GetItem(0);
                for (Int32 i = 0; i< sPoints.Count;++i)
                {
                    points.Add(new Coordinate(sPoints.GetItem(i).X, sPoints.GetItem(i).Y));
                }
                points.Add(new Coordinate(sPoints.GetItem(0).X, sPoints.GetItem(0).Y));
                LinearRing tmpLR = new LinearRing(points.ToArray());
                Polygon resPolygon = new Polygon(tmpLR);
                return resPolygon;
            }
            return null;
        }
    }

}



