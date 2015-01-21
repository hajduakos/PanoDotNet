using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Pano.Net.Model
{
    /// <summary>
    /// Helper class for geometric operations
    /// Source: http://www.codeproject.com/Articles/24727/WPF-D-Part-of-n
    /// </summary>
    public static class GeometryHelper
    {
        public static Point3D GetPosition(double theta, double phi, double radius)
        {
            double x = radius * Math.Sin(theta) * Math.Sin(phi);
            double z = radius * Math.Cos(phi);
            double y = radius * Math.Cos(theta) * Math.Sin(phi);

            return new Point3D(x, y, z);
        }

        public static Vector3D GetNormal(double theta, double phi)
        {
            return (Vector3D)GetPosition(theta, phi, 1.0);
        }

        public static double Deg2Rad(double degrees)
        {
            return (degrees / 180.0) * Math.PI;
        }

        public static Point GetTextureCoordinate(double theta, double phi)
        {
            Point p = new Point(theta / (2 * Math.PI), phi / (Math.PI));
            return p;
        }

        public static MeshGeometry3D CreateSphereMesh(int tDiv, int pDiv, double radius)
        {
            double dt = Deg2Rad(360.0) / tDiv;
            double dp = Deg2Rad(180.0) / pDiv;

            MeshGeometry3D mesh = new MeshGeometry3D();

            for (int pi = 0; pi <= pDiv; pi++)
            {
                double phi = pi * dp;

                for (int ti = 0; ti <= tDiv; ti++)
                {
                    double theta = ti * dt;

                    mesh.Positions.Add(GetPosition(theta, phi, radius));
                    mesh.Normals.Add(GetNormal(theta, phi));
                    mesh.TextureCoordinates.Add(GetTextureCoordinate(theta, phi));
                }
            }

            for (int pi = 0; pi < pDiv; pi++)
            {
                for (int ti = 0; ti < tDiv; ti++)
                {
                    int x0 = ti;
                    int x1 = (ti + 1);
                    int y0 = pi * (tDiv + 1);
                    int y1 = (pi + 1) * (tDiv + 1);

                    mesh.TriangleIndices.Add(x0 + y0);
                    mesh.TriangleIndices.Add(x0 + y1);
                    mesh.TriangleIndices.Add(x1 + y0);

                    mesh.TriangleIndices.Add(x1 + y0);
                    mesh.TriangleIndices.Add(x0 + y1);
                    mesh.TriangleIndices.Add(x1 + y1);
                }
            }

            mesh.Freeze();
            return mesh;
        }
    }
}
