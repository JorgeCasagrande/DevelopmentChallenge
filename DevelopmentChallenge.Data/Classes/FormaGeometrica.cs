/******************************************************************************************************************/
/******* ¿Qué pasa si debemos soportar un nuevo idioma para los reportes, o agregar más formas geométricas? *******/
/******************************************************************************************************************/

/*
 * TODO: 
 * Refactorizar la clase para respetar principios de la programación orientada a objetos.
 * Implementar la forma Trapecio/Rectangulo. 
 * Agregar el idioma Italiano (o el deseado) al reporte.
 * Se agradece la inclusión de nuevos tests unitarios para validar el comportamiento de la nueva funcionalidad agregada (los tests deben pasar correctamente al entregar la solución, incluso los actuales.)
 * Una vez finalizado, hay que subir el código a un repo GIT y ofrecernos la URL para que podamos utilizar la nueva versión :).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelopmentChallenge.Data.Classes
{
    public abstract class FormaGeometrica
    {
        public abstract decimal CalcularArea();
        public abstract decimal CalcularPerimetro();
        public abstract string ObtenerNombre(int idioma, int cantidad);

        public static string Imprimir(List<FormaGeometrica> formas, int idioma = Idiomas.Ingles)
        {
            var sb = new StringBuilder();

            if (!formas.Any())
            {
                sb.Append(Traducir("EmptyList", idioma));
            }
            else
            {
                sb.Append(Traducir("ShapesReport", idioma));

                var grupos = formas.GroupBy(f => f.GetType()).Select(g => new
                {
                    Tipo = g.Key,
                    Cantidad = g.Count(),
                    Area = g.Sum(f => f.CalcularArea()),
                    Perimetro = g.Sum(f => f.CalcularPerimetro()),
                    FormaEjemplo = g.First()
                });

                foreach (var grupo in grupos)
                {
                    string nombreForma = grupo.FormaEjemplo.ObtenerNombre(idioma, grupo.Cantidad);
                    sb.Append($"{grupo.Cantidad} {nombreForma} | {Traducir("Area", idioma)} {grupo.Area:#.##} | {Traducir("Perimeter", idioma)} {grupo.Perimetro:#.##} <br/>");
                }

                var totalFormas = grupos.Sum(g => g.Cantidad);
                var totalArea = grupos.Sum(g => g.Area);
                var totalPerimetro = grupos.Sum(g => g.Perimetro);

                sb.Append(Traducir("Total", idioma) + "<br/>");
                sb.Append($"{totalFormas} {Traducir("Shapes", idioma)} ");
                sb.Append($"{Traducir("Perimeter", idioma)} {totalPerimetro:#.##} ");
                sb.Append($"{Traducir("Area", idioma)} {totalArea:#.##}");
            }

            return sb.ToString();
        }

        private static string Traducir(string clave, int idioma)
        {
            string resultado;

            switch (idioma)
            {
                case Idiomas.Castellano:
                    resultado = Traducciones.Castellano.ContainsKey(clave) ? Traducciones.Castellano[clave] : clave;
                    break;
                case Idiomas.Ingles:
                    resultado = Traducciones.Castellano.ContainsKey(clave) ? Traducciones.Ingles[clave] : clave;
                    break;
                case Idiomas.Italiano:
                    resultado = Traducciones.Castellano.ContainsKey(clave) ? Traducciones.Italiano[clave] : clave;
                    break;
                default:
                    resultado = Traducciones.Castellano.ContainsKey(clave) ? Traducciones.Ingles[clave] : clave;
                    break;

            }

            return resultado;
        }
    }

    public class Cuadrado : FormaGeometrica
    {
        private readonly decimal _lado;
        public Cuadrado(decimal lado) { _lado = lado; }
        public override decimal CalcularArea() => _lado * _lado;
        public override decimal CalcularPerimetro() => _lado * 4;
        public override string ObtenerNombre(int idioma, int cantidad)
        {
            if (idioma == Idiomas.Castellano) return cantidad == 1 ? "Cuadrado" : "Cuadrados";
            if (idioma == Idiomas.Italiano) return cantidad == 1 ? "Quadrato" : "Quadrati";
            return cantidad == 1 ? "Square" : "Squares";
        }
    }

    public class Circulo : FormaGeometrica
    {
        private readonly decimal _radio;
        public Circulo(decimal radio) { _radio = radio; }
        public override decimal CalcularArea() => (decimal)Math.PI * (_radio / 2) * (_radio / 2);
        public override decimal CalcularPerimetro() => (decimal)Math.PI * _radio;
        public override string ObtenerNombre(int idioma, int cantidad)
        {
            if (idioma == Idiomas.Castellano) return cantidad == 1 ? "Círculo" : "Círculos";
            if (idioma == Idiomas.Italiano) return cantidad == 1 ? "Cerchio" : "Cerchi";
            return cantidad == 1 ? "Circle" : "Circles";
        }
    }

    public class TrianguloEquilatero : FormaGeometrica
    {
        private readonly decimal _lado;
        public TrianguloEquilatero(decimal lado) { _lado = lado; }
        public override decimal CalcularArea() => ((decimal)Math.Sqrt(3) / 4) * _lado * _lado;
        public override decimal CalcularPerimetro() => _lado * 3;
        public override string ObtenerNombre(int idioma, int cantidad)
        {
            if (idioma == Idiomas.Castellano) return cantidad == 1 ? "Triángulo" : "Triángulos";
            if (idioma == Idiomas.Italiano) return cantidad == 1 ? "Triangolo" : "Triangoli";
            return cantidad == 1 ? "Triangle" : "Triangles";
        }
    }

    public class Trapecio : FormaGeometrica
    {
        private readonly decimal _baseMayor;
        private readonly decimal _baseMenor;
        private readonly decimal _altura;
        private readonly decimal _lado1;
        private readonly decimal _lado2;

        public Trapecio(decimal baseMayor, decimal baseMenor, decimal altura, decimal lado1, decimal lado2)
        {
            _baseMayor = baseMayor;
            _baseMenor = baseMenor;
            _altura = altura;
            _lado1 = lado1;
            _lado2 = lado2;
        }

        public override decimal CalcularArea() => ((_baseMayor + _baseMenor) / 2) * _altura;
        public override decimal CalcularPerimetro() => _baseMayor + _baseMenor + _lado1 + _lado2;
        public override string ObtenerNombre(int idioma, int cantidad)
        {
            if (idioma == Idiomas.Castellano) return cantidad == 1 ? "Trapecio" : "Trapecios";
            if (idioma == Idiomas.Italiano) return cantidad == 1 ? "Trapezio" : "Trapezi";
            return cantidad == 1 ? "Trapezoid" : "Trapezoids";
        }
    }

    public class Rectangulo : FormaGeometrica
    {
        private readonly decimal _lado1;
        private readonly decimal _lado2;

        public Rectangulo(decimal lado1, decimal lado2)
        {
            _lado1 = lado1;
            _lado2 = lado2;
        }

        public override decimal CalcularArea() => _lado1 * _lado2;
        public override decimal CalcularPerimetro() => 2 * (_lado1 + _lado2);
        public override string ObtenerNombre(int idioma, int cantidad)
        {
            if (idioma == Idiomas.Castellano) return cantidad == 1 ? "Rectángulo" : "Rectángulos";
            if (idioma == Idiomas.Italiano) return cantidad == 1 ? "Rettangolo" : "Rettangoli";
            return cantidad == 1 ? "Rectangle" : "Rectangles";
        }
    }

    public static class Idiomas
    {
        public const int Castellano = 1;
        public const int Ingles = 2;
        public const int Italiano = 3;
    }

    public static class Traducciones
    {
        public static Dictionary<string, string> Castellano { get; } = new Dictionary<string, string>
        {
            { "EmptyList", "<h1>Lista vacía de formas!</h1>" },
            { "ShapesReport", "<h1>Reporte de Formas</h1>" },
            { "Shapes", "formas" },
            { "Perimeter", "Perimetro" },
            { "Area", "Area" },
            { "Total", "TOTAL:" }
        };

        public static Dictionary<string, string> Ingles { get; } = new Dictionary<string, string>
        {
            { "EmptyList", "<h1>Empty list of shapes!</h1>" },
            { "ShapesReport", "<h1>Shapes report</h1>" },
            { "Shapes", "shapes" },
            { "Perimeter", "Perimeter" },
            { "Area", "Area" },
            { "Total", "TOTAL:" }
        };

        public static Dictionary<string, string> Italiano { get; } = new Dictionary<string, string>
        {
            { "EmptyList", "<h1>Elenco vuoto di forme!</h1>" },
            { "ShapesReport", "<h1>Rapporto sulle forme</h1>" },
            { "Shapes", "forme" },
            { "Perimeter", "Perimetro" },
            { "Area", "Area" },
            { "Total", "TOTALE:" }
        };
    }
}
