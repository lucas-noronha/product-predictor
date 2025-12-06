using Microsoft.ML.Data;

namespace ProductPrediction.Train.Models;

public class RawPurchase
{
    [LoadColumn(0)]
    public string uid { get; set; }

    [LoadColumn(1)]
    public string date { get; set; }

    [LoadColumn(2)]
    public string item { get; set; }
}
