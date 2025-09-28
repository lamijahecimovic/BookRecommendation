using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.IO;

public class RecommenderService
{
    private readonly string _modelPath;
    private readonly string _dataPath;
    private readonly MLContext _mlContext;

    private ITransformer _model;
    private PredictionEngine<BookRating, RatingPrediction> _predictionEngine;

    public RecommenderService()
    {
        _mlContext = new MLContext();
        _modelPath = Path.Combine(Directory.GetCurrentDirectory(), "MLModels", "model.zip");
        _dataPath = Path.Combine(Directory.GetCurrentDirectory(), "MLData", "ratings.csv");


        if (File.Exists(_modelPath))
            UcitajModel();
    }

    public float PredictRating(uint korisnikId, uint knjigaId)
    {
        if (_predictionEngine == null)
            return float.NaN;

        var input = new BookRating
        {
            KorisnikId = korisnikId,
            KnjigaId = knjigaId
        };

        var prediction = _predictionEngine.Predict(input);

        Console.WriteLine($"PredictRating: korisnik {korisnikId}, knjiga {knjigaId} => score {prediction.Score}");

        return prediction.Score;
    }

    public void Treniraj()
    {
        if (!File.Exists(_dataPath))
        {
            Console.WriteLine("⚠️ CSV datoteka ne postoji.");
            return;
        }

        var data = _mlContext.Data.LoadFromTextFile<BookRating>(
            path: _dataPath,
            hasHeader: true,
            separatorChar: ',');

        var dataSplit = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(BookRating.KorisnikId),
            MatrixRowIndexColumnName = nameof(BookRating.KnjigaId),
            LabelColumnName = nameof(BookRating.Ocjena),
            NumberOfIterations = 20,
            ApproximationRank = 100
        };

        var estimator = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
        var model = estimator.Fit(dataSplit.TrainSet);

        using var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
        _mlContext.Model.Save(model, dataSplit.TrainSet.Schema, fs);

        _model = model;
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<BookRating, RatingPrediction>(_model);

        Console.WriteLine("Novi model treniran i učitan.");
    }

    private void UcitajModel()
    {
        using var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        _model = _mlContext.Model.Load(stream, out _);
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<BookRating, RatingPrediction>(_model);
        Console.WriteLine(" Model učitan iz model.zip");
    }

    public class BookRating
    {
        [LoadColumn(0)] public uint KorisnikId { get; set; }
        [LoadColumn(1)] public uint KnjigaId { get; set; }
        [LoadColumn(2)] public float Ocjena { get; set; }
    }

    public class RatingPrediction
    {
        public float Score { get; set; }
    }
}
