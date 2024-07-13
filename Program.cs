using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

public class CsvData
{
    [Name("year")]
    public string Year { get; set; }

    [Name("industry_code_ANZSIC")]
    public string IndustryCodeANZSIC { get; set; }

    [Name("industry_name_ANZSIC")]
    public string IndustryNameANZSIC { get; set; }

    [Name("rme_size_grp")]
    public string RmeSizeGroup { get; set; }

    [Name("variable")]
    public string Variable { get; set; }

    [Name("value")]
    public string Value { get; set; }

    [Name("unit")]
    public string Unit { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string arquivoCsv = @"D:\csv\oi.csv";

        var dados = LerDadosCsv(arquivoCsv);

        if (dados == null)
        {
            Console.WriteLine($"Arquivo '{arquivoCsv}' não encontrado ou vazio.");
            return;
        }

        Console.WriteLine("\nDados originais:");
        ExibirDadosFormatados(dados);

        Console.WriteLine("\nDigite o ano para filtrar os dados (ou deixe em branco para pular):");
        string anoDesejado = Console.ReadLine().Trim();
        if (!string.IsNullOrWhiteSpace(anoDesejado))
        {
            var dadosFiltrados = FiltrarDados(dados, d => d.Year == anoDesejado);
            Console.WriteLine($"\nDados filtrados para o ano '{anoDesejado}':");
            ExibirDadosFormatados(dadosFiltrados);
        }

        Console.WriteLine("\nDigite o critério de ordenação (Year, IndustryCodeANZSIC, IndustryNameANZSIC, RmeSizeGroup, Variable, Value, Unit):");
        string criterioOrdenacao = Console.ReadLine().Trim();
        if (!string.IsNullOrWhiteSpace(criterioOrdenacao))
        {
            var dadosOrdenados = OrdenarDados(dados, criterioOrdenacao);
            Console.WriteLine($"\nDados ordenados por '{criterioOrdenacao}':");
            ExibirDadosFormatados(dadosOrdenados);
        }
    }

    static List<CsvData> LerDadosCsv(string caminhoArquivo)
    {
        try
        {
            using (var reader = new StreamReader(caminhoArquivo))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<CsvData>().ToList();
            }
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    static void ExibirDadosFormatados(List<CsvData> dados)
    {
        foreach (var item in dados)
        {
            Console.WriteLine($"{item.Year,-10} | {item.IndustryCodeANZSIC,-15} | {item.IndustryNameANZSIC,-30} | {item.RmeSizeGroup,-15} | {item.Variable,-20} | {item.Value,-10} | {item.Unit,-10}");
        }
    }

    static List<CsvData> FiltrarDados(List<CsvData> dados, Func<CsvData, bool> filtro)
    {
        return dados.Where(filtro).ToList();
    }

    static List<CsvData> OrdenarDados(List<CsvData> dados, string chaveOrdenacao)
    {
        switch (chaveOrdenacao.ToLower())
        {
            case "year":
                return dados.OrderBy(d => d.Year).ToList();
            case "industrycodeanzsic":
                return dados.OrderBy(d => d.IndustryCodeANZSIC).ToList();
            case "industrynameanzsic":
                return dados.OrderBy(d => d.IndustryNameANZSIC).ToList();
            case "rmesizegrp":
                return dados.OrderBy(d => d.RmeSizeGroup).ToList();
            case "variable":
                return dados.OrderBy(d => d.Variable).ToList();
            case "value":
                return dados.OrderBy(d => float.Parse(d.Value)).ToList(); // Ordenação por float
            case "unit":
                return dados.OrderBy(d => d.Unit).ToList();
            default:
                Console.WriteLine("Critério de ordenação inválido. Retornando dados não ordenados.");
                return dados;
        }
    }
}
