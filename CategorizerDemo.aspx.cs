/*==========================================================================;
 *
 *  This file is part of the FIRST open-source software. 
 *  See http://project-first.eu/
 *
 *  File:    CategorizerDemo.aspx.cs
 *  Desc:    Categorizer demo request handler
 *  Created: Aug-2012
 *
 *  Author:  Miha Grcar
 *
 ***************************************************************************/

using System;
using System.Web.UI;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Latino;
using Latino.TextMining;
using Latino.Model;

public partial class CategorizerDemo : Page 
{
    private static BowSpace mBowSpace
        = null;
    private static Dictionary<string, IModel<string>> mCategorizer
        = null;
    private static bool mReady
        = false;

    static CategorizerDemo()
    {
        string fileName = HttpContext.Current.Server.MapPath("App_Data\\model.bin");
        BinarySerializer binReader = new BinarySerializer(fileName, FileMode.Open);
        mBowSpace = new BowSpace(binReader);
        mBowSpace.CutLowWeightsPerc = 0.2;
        mCategorizer = Utils.LoadDictionary<string, IModel<string>>(binReader);
        binReader.Close();
        mReady = true;
    }

    //static void OutputPredictedCategoriesAlt(string prefix, double thresh, SparseVector<double> vec, StringBuilder output)
    //{
    //    if (!mCategorizer.ContainsKey(prefix)) 
    //    {
    //        output.AppendLine(prefix);
    //        return;         
    //    }
    //    IModel<string> classifier = mCategorizer[prefix];
    //    Prediction<string> p = ((IModel<string>)classifier).Predict(ModelUtils.ConvertExample(vec, classifier.RequiredExampleType));
    //    double maxSim = p.Count == 0 ? 0 : p.BestScore;
    //    foreach (KeyDat<double, string> item in p)
    //    {
    //        if (item.Key == 0) { break; }
    //        double score = item.Key / maxSim;
    //        if (score < thresh) { break; }
    //        OutputPredictedCategoriesAlt(prefix + item.Dat + '/', thresh, vec, output);
    //    }
    //}

    static void OutputPredictedCategories(string tab, string prefix, double thresh, SparseVector<double> vec, StringBuilder output)
    {
        if (!mCategorizer.ContainsKey(prefix)) { return; }
        IModel<string> classifier = mCategorizer[prefix];
        Prediction<string> p = ((IModel<string>)classifier).Predict(ModelUtils.ConvertExample(vec, classifier.RequiredExampleType));
        double maxSim = p.Count == 0 ? 0 : p.BestScore;
        foreach (KeyDat<double, string> item in p)
        {
            if (item.Key == 0) { break; }
            double score = item.Key / maxSim;
            if (score < thresh) 
            {
                //output.AppendLine(string.Format("{0}({1:0.0000} {2})", tab, score, item.Dat));
                break; 
            }
            output.AppendLine(string.Format("{0}{1:0.0000} {2:0.0000}: {3}", tab, item.Key, score, item.Dat));
            OutputPredictedCategories(tab + '\t', prefix + item.Dat + '/', thresh, vec, output);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string text = Request.Params["text"];
        if (!string.IsNullOrEmpty(text))
        {
            while (!mReady) { Thread.Sleep(1000); }
            SparseVector<double> vec = mBowSpace.ProcessDocument(text);
            StringBuilder output = new StringBuilder();
            OutputPredictedCategories(/*tab=*/"", /*prefix=*/"", /*thresh=*/0.9, vec, output);
            Response.Write(output.ToString());
        }
    }
}
