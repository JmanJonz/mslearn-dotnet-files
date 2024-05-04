using Newtonsoft.Json; 

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);   

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }
    
    return salesTotal;
}

void GenerateSalesReport(IEnumerable<string> salesFiles){
    File.WriteAllText(Path.Combine(salesTotalDir, "SalesReport.txt"), $"{"Total Sales Accross All Stores: "}{salesTotal.ToString("C2")}{Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "SalesReport.txt"), $"------------------------------------------ {Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "SalesReport.txt"), $"Details: {Environment.NewLine}");

        double salesTotal2 = 0;
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // check if store ending is sales.json and not salestotals and then add up and make report
            if(file.EndsWith("sales.json")){
                                Console.WriteLine(data?.Total);
                                Console.WriteLine(file, data?.Total);
                // add to running total
                    salesTotal2 += data?.Total ?? 0;

                // write total for each store to document
                    File.AppendAllText(Path.Combine(salesTotalDir, "SalesReport.txt"), $"{Path.GetFileName(file)}: {data?.Total} {Environment.NewLine}");
            }
    }
}

GenerateSalesReport(salesFiles);

record SalesData (double Total);